using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetCompanyAsync(Guid id);
    Task<IEnumerable<Company>> GetCompaniesAsync();
    Task AddCompanyAsync(Company company);
    Task UpdateCompanyAsync(Company company);
    Task DeleteCompanyAsync(Company company);
}