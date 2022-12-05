namespace Property.Api.Contracts.Services;

public interface IEmailService
{
    Task SendNewUserEmail(string email, string name, string? password);
    Task SendPasswordResetEmail(string email, string name, string passwordResetToken, string passwordResetId);
}