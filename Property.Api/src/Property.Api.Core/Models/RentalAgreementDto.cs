namespace Property.Api.Core.Models;

public class RentalAgreementDto
{
    public string Id { get; set; }
    public string RentalAgreementPropertyId { get; set; }
    public PropertyDto Property { get; set; }

    public string? RentalAgreementAccountId { get; set; }
    public AccountDto? Account { get; set; }

    public string RentalAgreementCompanyId { get; set; }
    public CompanyDto Company { get; set; }

    public List<Guid> Files { get; set; }
}