namespace Property.Api.Infrastructure.Repositories;

public class RentalAgreementRepository : IRentalAgreementRepository
{
    private readonly ApiContext _apiContext;

    public RentalAgreementRepository(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }

    public async Task<RentalAgreement?> GetRentalAgreementAsync(int rentalAgreementId)
        => await _apiContext.RentalAgreements
            .Include(x => x.Company)
            .Include(x => x.Property)
            .FirstOrDefaultAsync(x => x.Id == rentalAgreementId);


    public async Task<RentalAgreement?> GetRentalAgreementAsync(int companyId, int propertyId)
        => await _apiContext.RentalAgreements
            .Include(x => x.Company)
            .Include(x => x.Property)
            .FirstOrDefaultAsync(x =>
                x.RentalAgreementCompanyId == companyId && x.RentalAgreementPropertyId == propertyId);


    public async Task<IEnumerable<RentalAgreement>> GetRentalAgreements(int companyId, int page, int amount)
        => await _apiContext.RentalAgreements
            .Include(x => x.Company)
            .Include(x => x.Property)
            .Where(x => x.RentalAgreementCompanyId == companyId)
            .Skip(page * amount)
            .Take(amount)
            .ToListAsync();

    public async Task<RentalAgreement?> CreateRentalAgreement(RentalAgreement rentalAgreement)
    {
        var newRentalAgreement = await _apiContext.RentalAgreements.AddAsync(rentalAgreement);
        
        await _apiContext.SaveChangesAsync();
        
        return newRentalAgreement.Entity;
    }

    public async Task<RentalAgreement?> UpdateRentalAgreement(RentalAgreement rentalAgreement)
    {
        var updatedEntity = _apiContext.RentalAgreements.Update(rentalAgreement);
        
        await _apiContext.SaveChangesAsync();
        
        return updatedEntity.Entity;
    }
    
    public async Task DeleteRentalAgreement(RentalAgreement rentalAgreement)
    {
        _apiContext.RentalAgreements.Remove(rentalAgreement);
        
        await _apiContext.SaveChangesAsync();
    }
}