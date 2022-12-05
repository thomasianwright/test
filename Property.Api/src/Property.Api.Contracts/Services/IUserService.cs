using Property.Api.Core.Models;

namespace Property.Api.Contracts.Services;

public interface IUserService
{
    Task<UserDto?> GetUserAsync(int id);
    Task<UserDto?> GetUserAsync(int id, int companyId);
    Task<IEnumerable<UserDto>> GetUsers(int companyId);
    Task<UserDto?> GetUserAsync(string email);
    Task<UserDto?> CreateUserAsync(CreateUserDto userDto);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto);
    Task DeleteUser(int userId, int requestedBy);
}