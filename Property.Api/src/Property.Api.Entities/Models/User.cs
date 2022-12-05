namespace Property.Api.Entities.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleNames { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int UserAddressId { get; set; }
    public Address Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public ICollection<RentalAgreement> RentalAgreements { get; set; }
    public ICollection<Account> Accounts { get; set; }
    public int? UserCompanyId { get; set; }
    public Company? Company { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? LastLoginIp { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}