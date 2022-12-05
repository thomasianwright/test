namespace Property.Api.Core.Models;

public class CompanyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? CompanyNumber { get; set; }
    public string? VatNumber { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    
    public AddressDto TradingAddress { get; set; }

    public ICollection<PropertyDto> Properties { get; set; }
    public ICollection<RentalAgreementDto> RentalAgreements { get; set; }
}