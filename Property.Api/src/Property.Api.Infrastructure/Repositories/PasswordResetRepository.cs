namespace Property.Api.Infrastructure.Repositories;

public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly ApiContext _context;

    public PasswordResetRepository(ApiContext context)
    {
        _context = context;
    }

    public async Task<PasswordReset> GetPasswordResetAsync(string email)
    {
        return await _context.PasswordResets.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<PasswordReset?> GetPasswordResetAsync(int id)
        => await _context.PasswordResets
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<PasswordReset?> CreatePasswordReset(PasswordReset passwordReset)
    {
        var entity = await _context.PasswordResets.AddAsync(passwordReset);
        await _context.SaveChangesAsync();

        return entity.Entity;
    }

    public async Task UpdatePasswordReset(PasswordReset passwordReset)
    {
        _context.PasswordResets.Update(passwordReset);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePasswordReset(PasswordReset passwordReset)
    {
        _context.PasswordResets.Remove(passwordReset);
        await _context.SaveChangesAsync();
    }
}