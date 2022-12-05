namespace Property.Api.Entities.Models;

public class AccountUser
{
    public int AccountId { get; set; }
    public Account Account { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}