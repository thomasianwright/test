using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Repositories;

public interface IPasswordResetRepository
{
    Task<PasswordReset?> GetPasswordResetAsync(int id);
    Task<PasswordReset?> CreatePasswordReset(PasswordReset passwordReset);
    Task UpdatePasswordReset(PasswordReset passwordReset);
    Task DeletePasswordReset(PasswordReset passwordReset);
}
