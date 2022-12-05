namespace Property.Api.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApiContext _apiContext;

    public CompanyRepository(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }
    
    public async Task<Company> GetCompanyAsync(Guid id)
    {
        return await _apiContext.Companies.FindAsync(id);
    }
    
    public async Task<IEnumerable<Company>> GetCompaniesAsync()
    {
        return await _apiContext.Companies.ToListAsync();
    }
    
    public async Task AddCompanyAsync(Company company)
    {
        await _apiContext.Companies.AddAsync(company);
        await _apiContext.SaveChangesAsync();
    }
    
    public async Task UpdateCompanyAsync(Company company)
    {
        _apiContext.Companies.Update(company);
        await _apiContext.SaveChangesAsync();
    }
    
    public async Task DeleteCompanyAsync(Company company)
    {
        _apiContext.Companies.Remove(company);
        await _apiContext.SaveChangesAsync();
    }
}