using Property.Api.Entities.Models;

namespace Property.Api.Contracts.Repositories;

public interface IUserRepository
{
    Task<User?> GetAsync(int id);
    Task<User?> GetAsync(int id, int companyId);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task<IEnumerable<User>> GetUsers(int companyId);
    Task<User?> GetAsync(string email);
    Task<User?> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task DeleteAsync(User user);
}