namespace Property.Api.Entities.Models;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<RentalAgreement> RentalAgreements { get; set; }

    public int AccountUserOwnerId { get; set; }
    public User AccountOwner { get; set; }
}