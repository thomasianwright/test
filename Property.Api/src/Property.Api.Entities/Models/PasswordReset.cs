namespace Property.Api.Entities.Models;

public class PasswordReset
{
    public int Id { get; set; }
    public Guid Identity { get; set; }
    public string Email { get; set; }
    public int PasswordResetUserId { get; set; }
    public User User { get; set; }


    public DateTime? UsedAt { get; set; }
    public DateTime Expiry { get; set; }
    public DateTime CreatedAt { get; set; }
}