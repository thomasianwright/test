namespace Property.Api.Core.Models;

public class UpdatePropertyDto
{
    public AddressDto PropertyAddress { get; set; }

    public string PropertyCompanyId { get; set; }
    public CompanyDto Company { get; set; }

    public string? PropertyRentalAgreementId { get; set; }
    public RentalAgreementDto? RentalAgreement { get; set; }
}