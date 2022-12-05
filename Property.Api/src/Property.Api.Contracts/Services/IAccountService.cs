using Property.Api.Core.Models;

namespace Property.Api.Contracts.Services;

public interface IAccountService
{
    Task<AccountDto?> GetAccount(int id);
    Task<AccountDto> CreateAccount(CreateAccountDto accountDto);
    Task<AccountDto> UpdateAccount(int id, UpdateAccountDto accountDto);
    Task DeleteAccount(int id);
}