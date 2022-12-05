using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetAccountAsync(int id);
    Task<Account?> CreateAccount(Account account);
    Task<Account?> UpdateAccount(Account account);
    Task DeleteAccount(Account account);
}