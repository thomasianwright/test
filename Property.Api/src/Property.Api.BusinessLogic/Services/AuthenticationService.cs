using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Text;
using Ardalis.GuardClauses;
using AutoMapper;
using HashidsNet;
using Microsoft.Extensions.Logging;
using Property.Api.BusinessLogic.Exceptions;
using Property.Api.Contracts.Repositories;
using Property.Api.Contracts.Services;
using Property.Api.Core.Helpers;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IHashids _hashids;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IUserRepository userRepository, IMapper mapper, IEmailService emailService,
        IPasswordResetRepository passwordResetRepository, IHashids hashids, ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _emailService = emailService;
        _passwordResetRepository = passwordResetRepository;
        _hashids = hashids;
        _logger = logger;
    }

    public async Task<User> Authenticate(LoginDto loginDto)
    {
        var user = await _userRepository.GetAsync(loginDto.Email);
        if (user == null)
            throw new NullReferenceException("User not found");

        if (!ComparePassword(loginDto.Password, user.Password))
            throw new LoginException("Password does not match", ErrorCodes.InvalidCredentials);

        return user;
    }

    public async Task<User?> Authenticate(int userId, string refreshToken)
    {
        var user = await _userRepository.GetAsync(userId);
        if (user == null)
            throw new NullReferenceException("User not found");

        Guard.Against.NullOrWhiteSpace(user.RefreshToken);

        if (!ComparePassword(refreshToken, user!.RefreshToken))
            throw new AuthenticationException("Refresh token does not match");

        return user;
    }

    public async Task AddRefreshToken(User user, string refreshToken, string ip, DateTime expiry)
    {
        user.RefreshToken = HashPassword(refreshToken);
        user.LastLoginIp = ip;
        user.LastLoginDate = DateTime.UtcNow;
        user.RefreshTokenExpiryTime = expiry;

        await _userRepository.UpdateAsync(user);
    }

    public async Task<User?> Register(CreateUserDto registerDto)
    {
        var user = await _userRepository.GetAsync(registerDto.Email);
        if (user != null)
            throw new AuthenticationException("User already exists");

        var newUser = new User();

        if (string.IsNullOrWhiteSpace(registerDto.Password))
        {
            var password = CreatePassword(10);
            var name = $"{registerDto.FirstName} {registerDto.LastName}";
            registerDto.Password = HashPassword(password);

            await _emailService.SendNewUserEmail(registerDto.Email, name, password);
        }

        _mapper.Map(registerDto, newUser);

        newUser.Password = HashPassword(registerDto.Password);

        await _userRepository.AddAsync(newUser);
        return newUser;
    }

    public async Task CreatePasswordReset(PasswordResetDto passwordResetDto)
    {
        var user = await _userRepository.GetAsync(passwordResetDto.Email);
        if (user == null)
            throw new NullReferenceException("User not found");

        var passwordReset = new PasswordReset
        {
            PasswordResetUserId = user.Id,
            Identity = Guid.NewGuid(),
            Expiry = DateTime.UtcNow.AddHours(1),
            User = user,
            Email = passwordResetDto.Email,
            CreatedAt = DateTime.Now,
        };
        
        var createdPasswordReset = await _passwordResetRepository.CreatePasswordReset(passwordReset);
        Guard.Against.Null(createdPasswordReset, nameof(createdPasswordReset));
        
        var passwordResetId = _hashids.Encode(createdPasswordReset.Id);
#if DEBUG
        _logger.LogWarning("Password reset identity: {Identity} Id: {Id}", passwordReset.Identity,
            passwordResetId);
#endif
        await _emailService.SendPasswordResetEmail(passwordResetDto.Email, $"{user.FirstName} {user.LastName}",
            passwordReset.Identity.ToString(), passwordResetId);
    }

    public async Task GetPasswordReset(string id, string identity)
    {
        if (!Guid.TryParse(identity, out var guid))
            throw new ArgumentException("Invalid identity");
        var passwordResetId = _hashids.DecodeSingle(id);

        var passwordReset = await _passwordResetRepository.GetPasswordResetAsync(passwordResetId);

        Guard.Against.Null(passwordReset);

        if (passwordReset.Expiry < DateTime.UtcNow)
            throw new ArgumentException("Password reset has expired");

        if (passwordReset.Identity != guid)
            throw new ArgumentException("Invalid identity");
    }

    public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var passwordResetId = _hashids.DecodeSingle(resetPasswordDto.Id);

        var passwordReset = await _passwordResetRepository.GetPasswordResetAsync(passwordResetId);

        if (passwordReset == null)
            throw new NullReferenceException("Password reset not found");

        if (passwordReset.Expiry < DateTime.UtcNow)
            throw new AuthenticationException("Password reset has expired");

        if (Guid.TryParse(resetPasswordDto.Identity, out var identity))
            if (passwordReset.Identity != identity)
                throw new ValidationException("Password reset identity does not match");

        var user = await _userRepository.GetAsync(passwordReset.PasswordResetUserId);

        if (user == null)
            throw new NullReferenceException("User not found");

        user.Password = HashPassword(resetPasswordDto.Password);
        user.UpdatedAt = DateTime.Now;

        passwordReset.UsedAt = DateTime.Now;

        await _userRepository.UpdateAsync(user);
        await _passwordResetRepository.UpdatePasswordReset(passwordReset);
        await _passwordResetRepository.DeletePasswordReset(passwordReset);
    }

    private static bool ComparePassword(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);

    private static string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    private static string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!$%&/()=?*#";
        var res = new StringBuilder();
        var rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }

        return res.ToString();
    }
}