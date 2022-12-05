using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Services;

public interface IAuthenticationService
{
    Task<User> Authenticate(LoginDto loginDto);
    Task<User?> Authenticate(int id, string refreshToken);
    Task<User?> Register(CreateUserDto registerDto);
    Task AddRefreshToken(User user, string refreshToken, string ip, DateTime expiry);
    Task CreatePasswordReset(PasswordResetDto passwordResetDto);
    Task GetPasswordReset(string id, string identity);
    Task ResetPassword(ResetPasswordDto resetPasswordDto);
}