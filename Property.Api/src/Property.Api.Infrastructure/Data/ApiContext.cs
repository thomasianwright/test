using System.Text.Json;
using PropertyModel = Property.Api.Entities.Models.Property;

namespace Property.Api.Infrastructure.Data;

public class ApiContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<PropertyModel?> Properties { get; set; }
    public DbSet<RentalAgreement?> RentalAgreements { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PasswordReset?> PasswordResets { get; set; }
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        var jsonOptions = new JsonSerializerOptions();

        mb.Entity<Account>().ToTable("Account");
        mb.Entity<Address>().ToTable("Address");
        mb.Entity<Company>().ToTable("Company");
        mb.Entity<PropertyModel>().ToTable("Property");
        mb.Entity<RentalAgreement>().ToTable("RentalAgreement");
        mb.Entity<User>().ToTable("User");
        mb.Entity<PasswordReset>().ToTable("PasswordReset");
        
        mb.Entity<Account>()
            .HasMany(x => x.Users)
            .WithMany(x => x.Accounts)
            .UsingEntity<AccountUser>(x =>
            {
                x.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.NoAction);
                x.HasKey(x => new { x.UserId, x.AccountId });
            });

        mb.Entity<Account>()
            .HasMany(x => x.RentalAgreements)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.RentalAgreementAccountId);

        mb.Entity<Company>()
            .HasOne(x => x.TradingAddress)
            .WithOne()
            .HasForeignKey<Company>(x => x.TradingAddressId)
            .OnDelete(DeleteBehavior.NoAction);

        mb.Entity<PropertyModel>()
            .HasOne(x => x.PropertyAddress)
            .WithOne()
            .HasForeignKey<PropertyModel>(x => x.PropertyAddressId)
            .OnDelete(DeleteBehavior.NoAction);

        mb.Entity<PropertyModel>()
            .HasOne(x => x.Company)
            .WithMany(x => x.Properties)
            .HasForeignKey(x => x.PropertyCompanyId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        mb.Entity<RentalAgreement>()
            .HasOne(x => x.Account)
            .WithMany(x => x.RentalAgreements)
            .HasForeignKey(x => x.RentalAgreementAccountId);

        mb.Entity<RentalAgreement>()
            .HasOne(x => x.Property)
            .WithOne(x => x.RentalAgreement)
            .HasForeignKey<RentalAgreement>(x => x.RentalAgreementPropertyId)
            .OnDelete(DeleteBehavior.NoAction);

        mb.Entity<RentalAgreement>()
            .HasOne(x => x.Company)
            .WithMany(x => x.RentalAgreements)
            .HasForeignKey(x => x.RentalAgreementCompanyId);

        mb.Entity<RentalAgreement>()
            .Property(x => x.Files)
            .HasConversion(x => JsonSerializer.Serialize(x, jsonOptions),
                x => JsonSerializer.Deserialize<List<Guid>>(x, jsonOptions) ?? new List<Guid>());

        mb.Entity<Account>()
            .HasOne(x => x.AccountOwner)
            .WithOne()
            .HasForeignKey<Account>(x => x.AccountUserOwnerId);

        mb.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        mb.Entity<PasswordReset>()
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<PasswordReset>(x => x.PasswordResetUserId)
            .OnDelete(DeleteBehavior.NoAction);

        mb.Entity<PasswordReset>()
            .HasIndex(x => x.PasswordResetUserId);

        base.OnModelCreating(mb);
    }
}