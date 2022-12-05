using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Repositories;

public interface IRentalAgreementRepository
{
    Task<RentalAgreement?> GetRentalAgreementAsync(int rentalAgreementId);
    Task<RentalAgreement?> GetRentalAgreementAsync(int companyId, int propertyId);
    Task<IEnumerable<RentalAgreement>> GetRentalAgreements(int companyId, int page, int amount);
    Task<RentalAgreement?> CreateRentalAgreement(RentalAgreement rentalAgreement);
    Task<RentalAgreement?> UpdateRentalAgreement(RentalAgreement rentalAgreement);
    Task DeleteRentalAgreement(RentalAgreement rentalAgreement);
}