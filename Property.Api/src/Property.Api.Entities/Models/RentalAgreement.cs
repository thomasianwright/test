namespace Property.Api.Entities.Models;

public class RentalAgreement
{
    public int Id { get; set; }
    public int RentalAgreementPropertyId { get; set; }
    public Property Property { get; set; }

    public int? RentalAgreementAccountId { get; set; }
    public Account? Account { get; set; }

    public int RentalAgreementCompanyId { get; set; }
    public Company Company { get; set; }

    public List<Guid> Files { get; set; }
}