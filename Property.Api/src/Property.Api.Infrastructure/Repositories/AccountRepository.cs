namespace Property.Api.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApiContext _apiContext;

    public AccountRepository(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }

    public async Task<Account?> GetAccountAsync(int id)
        => await _apiContext.Accounts
            .Include(x => x.AccountOwner)
            .Include(x => x.RentalAgreements)
            .Include(x=> x.Users)
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Account?> CreateAccount(Account account)
    {
        var newAccount = await _apiContext.Accounts.AddAsync(account);

        await _apiContext.SaveChangesAsync();

        return newAccount.Entity;
    }

    public async Task<Account?> UpdateAccount(Account account)
    {
        var updatedAccount = _apiContext.Accounts.Update(account);

        await _apiContext.SaveChangesAsync();

        return updatedAccount.Entity;
    }

    public async Task DeleteAccount(Account account)
    {
        _apiContext.Accounts.Remove(account);

        await _apiContext.SaveChangesAsync();
    }
}