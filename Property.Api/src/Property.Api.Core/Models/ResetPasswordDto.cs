namespace Property.Api.Core.Models;

public class ResetPasswordDto
{
    public string Id { get; set; }
    public string Identity { get; set; }
    public string Password { get; set; }
}