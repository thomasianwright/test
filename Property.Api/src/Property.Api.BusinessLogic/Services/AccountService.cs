using Ardalis.GuardClauses;
using AutoMapper;
using Property.Api.Contracts.Repositories;
using Property.Api.Contracts.Services;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto?> GetAccount(int id)
    {
        var account = await _accountRepository.GetAccountAsync(id);

        var accountDto = _mapper.Map<AccountDto>(account);

        return accountDto;
    }
    
    public async Task<AccountDto> CreateAccount(CreateAccountDto accountDto)
    {
        var account = _mapper.Map<Account>(accountDto);

        var createdAccount = await _accountRepository.CreateAccount(account);

        var createdAccountDto = _mapper.Map<AccountDto>(createdAccount);

        return createdAccountDto;
    }
    
    public async Task<AccountDto> UpdateAccount(int id, UpdateAccountDto accountDto)
    {
        var account = _mapper.Map<Account>(accountDto);

        var updatedAccount = await _accountRepository.UpdateAccount(account);

        var updatedAccountDto = _mapper.Map<AccountDto>(updatedAccount);

        return updatedAccountDto;
    }
    
    public async Task DeleteAccount(int id)
    {
        var account = await _accountRepository.GetAccountAsync(id);
        
        Guard.Against.Null(account, nameof(account), "Account not found");
        
        await _accountRepository.DeleteAccount(account);
    }
}