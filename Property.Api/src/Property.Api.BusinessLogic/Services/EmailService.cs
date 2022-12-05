using Property.Api.Contracts.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Property.Api.BusinessLogic.Services;

public class EmailService : IEmailService
{
    public Task SendNewUserEmail(string email, string name, string? password)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetEmail(string email, string name, string passwordResetToken, string passwordResetId)
    {
        return Task.CompletedTask;
    }
}