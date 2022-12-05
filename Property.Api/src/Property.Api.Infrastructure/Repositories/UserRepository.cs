namespace Property.Api.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApiContext _apiContext;

    public UserRepository(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }

    public async Task<User?> GetAsync(int id)
        => await _apiContext.Users
            .Include(x => x.Address)
            .Include(x => x.Company)
            .Include(x=> x.Accounts)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetAsync(int id, int companyId)
        => await _apiContext.Users
            .Include(x=> x.Address)
            .Include(x=>x.Company)
            .Include(x=> x.Accounts)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserCompanyId == companyId);

    public Task<IEnumerable<User>> GetUsers()
        => Task.FromResult(_apiContext.Users.AsEnumerable());

    public Task<IEnumerable<User>> GetUsers(int companyId)
        => Task.FromResult(_apiContext.Users.Where(x => x.UserCompanyId == companyId).AsEnumerable());

    public async Task<User?> GetAsync(string email)
        => await _apiContext.Users
            .Include(x => x.Address)
            .Include(x => x.Company)
            .Include(x=> x.Accounts)
            .FirstOrDefaultAsync(x => x.Email == email);
    
    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        => await _apiContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.RefreshTokenExpiryTime > DateTime.UtcNow);

    public async Task<User?> AddAsync(User user)
    {
        var newUser = await _apiContext.Users.AddAsync(user);

        await _apiContext.SaveChangesAsync();

        return newUser.Entity;
    }

    public async Task<User?> UpdateAsync(User user)
    {
        var newUser = _apiContext.Users.Update(user);

        await _apiContext.SaveChangesAsync();

        return newUser.Entity;
    }

    public async Task DeleteAsync(User user)
    {
        _apiContext.Users.Remove(user);

        await _apiContext.SaveChangesAsync();
    }
}