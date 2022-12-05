namespace Property.Api.Core.Models;

public class CreatePropertyDto
{
    public CreateAddressDto PropertyAddress { get; set; }

    public string PropertyCompanyId { get; set; }
    public CompanyDto Company { get; set; }
}