namespace Property.Api.Entities.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? CompanyNumber { get; set; }
    public string? VatNumber { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    
    public int TradingAddressId { get; set; }
    public Address TradingAddress { get; set; }

    public ICollection<Property> Properties { get; set; }
    public ICollection<RentalAgreement> RentalAgreements { get; set; }
}