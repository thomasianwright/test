using System.Data;
using AutoMapper;
using Property.Api.Contracts.Repositories;
using Property.Api.Contracts.Services;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<UserDto?> GetUserAsync(int id)
    {
        var user = await _userRepository.GetAsync(id); 
        
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserAsync(int id, int companyId)
    {
        var user = await _userRepository.GetAsync(id, companyId); 
        
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsers(int companyId)
    {
        var users = await _userRepository.GetUsers(companyId);
        
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserAsync(string email)
    {
        var user = await _userRepository.GetAsync(email); 
        
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto?> CreateUserAsync(CreateUserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        
        var createdUser = await _userRepository.AddAsync(user);
        
        return createdUser == null ? null : _mapper.Map<UserDto>(createdUser);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto userDto)
    {
        var user = await _userRepository.GetAsync(id);
        
        if (user == null)
        {
            return null;
        }
        
        _mapper.Map(userDto, user);
        
        if(id != user.Id)
        {
            throw new ConstraintException("User id does not match");
        }
        
        user.UpdatedAt = DateTime.Now;

        var updatedUser = await _userRepository.UpdateAsync(user);
        
        return updatedUser == null ? null : _mapper.Map<UserDto>(updatedUser);
    }

    public async Task DeleteUser(int userId, int requestedBy)
    {
        var user = await _userRepository.GetAsync(userId);

        // todo: check if user is allowed to delete this user
        
        await _userRepository.DeleteAsync(user);
    }
}