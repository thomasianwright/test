namespace Property.Api.Entities.Models;

public class Property
{
    public int Id { get; set; }

    public int PropertyAddressId { get; set; }
    public Address PropertyAddress { get; set; }

    public int PropertyCompanyId { get; set; }
    public Company Company { get; set; }

    public int? PropertyRentalAgreementId { get; set; }
    public RentalAgreement? RentalAgreement { get; set; }
}