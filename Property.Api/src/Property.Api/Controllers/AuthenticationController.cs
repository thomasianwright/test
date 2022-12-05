using System.Security.Authentication;
using Ardalis.GuardClauses;
using AutoMapper;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Property.Api.BusinessLogic.Exceptions;
using Property.Api.Contracts.Services;
using Property.Api.Core.Helpers;
using Property.Api.Core.Models;

namespace Property.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IHashids _hashids;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService,
        IMapper mapper, IHashids hashids, ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _mapper = mapper;
        _hashids = hashids;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AuthenticateResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            Guard.Against.Null(request);
            Guard.Against.NullOrWhiteSpace(request.Email);
            Guard.Against.NullOrWhiteSpace(request.Password);

            var user = await _authenticationService.Authenticate(request);
            var expiry = DateTime.Now.AddMinutes(2);
            
            var refreshTokenExpiry = request.RememberMe ? DateTime.Now.AddDays(14) : DateTime.Now.AddHours(2);

            var (token, securityToken) = _tokenService.GenerateToken(user, expiry);

            var refreshToken = _tokenService.GenerateRefreshToken();
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _authenticationService.AddRefreshToken(user, refreshToken, ip, refreshTokenExpiry);
            var userDto = _mapper.Map<UserDto>(user);
            
            return Ok(new AuthenticateResponseDto(token, securityToken.ValidTo, refreshToken, refreshTokenExpiry, userDto.Id, userDto));
        }
        catch (LoginException e)
        {
            _logger.LogError(e, "Login failed");

            return BadRequest(new ErrorResponseDto(e.Message, e.Code));
        }
        catch (AuthenticationException e)
        {
            _logger.LogError(e, "Authentication failed {Email}", request.Email);
            
            return BadRequest(new ErrorResponseDto("Authentication failed", ErrorCodes.InvalidCredentials));
        }
        catch (NullReferenceException e)
        {
            _logger.LogDebug("Null reference exception: {EMessage}", e.Message);
            
            return BadRequest(new ErrorResponseDto(e.Message, ErrorCodes.NullProperty));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while logging in {Email}", request.Email);
            
            return BadRequest(new ErrorResponseDto("An unknown error occurred", ErrorCodes.UnknownError));
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(RegenerateTokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(RegenerateTokenDto regenerateTokenDto)
    {
        try
        {
            var userId = _hashids.DecodeSingle(regenerateTokenDto.UserId);

            var user = await _authenticationService.Authenticate(userId, regenerateTokenDto.Token);

            var expiry = DateTime.Now.AddMinutes(2);

            var (token, securityToken) = _tokenService.GenerateToken(user, expiry);
            
            _logger.LogInformation("Token refreshed for user {UserId}", userId);
            
            return Ok(new RegenerateTokenResponseDto(token, securityToken.ValidTo));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error refreshing token");
            
            return BadRequest(new ErrorResponseDto("An unknown error has occured", ErrorCodes.UnknownError));
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(CreateUserDto userDto)
    {
        try
        {
            var user = await _authenticationService.Register(userDto);

            var newUser = _mapper.Map<UserDto>(user);

            return Ok(newUser);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while registering user {Email}", userDto.Email);
            
            return BadRequest(new ErrorResponseDto("An unknown error has occured", ErrorCodes.UnknownError));
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPasswordReset([FromQuery] string id, [FromQuery] string identity)
    {
        try
        {
            await _authenticationService.GetPasswordReset(id, identity);

            return Ok();
        }
        catch (NullReferenceException e)
        {
            _logger.LogInformation(e, "Null reference exception: {EMessage}", e.Message);
            
            return BadRequest(new ErrorResponseDto(e.Message, ErrorCodes.NullProperty));
        }
        catch (ArgumentException e)
        {
            _logger.LogInformation(e, "Argument exception: {EMessage}", e.Message);

            return BadRequest(new ErrorResponseDto(e.Message, ErrorCodes.InvalidRequest));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting password reset");
            
            return BadRequest(new ErrorResponseDto("An unknown error has occured", ErrorCodes.UnknownError));
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePasswordReset(PasswordResetDto resetDto)
    {
        try
        {
            await _authenticationService.CreatePasswordReset(resetDto);
        }
        catch (NullReferenceException e)
        {
            _logger.LogInformation("User not found {Email}", resetDto.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured");
        }

        return Ok();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetDto)
    {
        try
        {
            await _authenticationService.ResetPassword(resetDto);

            return Ok();
        }
        catch (NullReferenceException e)
        {
            if (e.Message != null) _logger.LogInformation(e.Message);

            return BadRequest(new ErrorResponseDto("User not found", ErrorCodes.InvalidRequest));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured");
            
            return BadRequest(new ErrorResponseDto("An unknown error has occured", ErrorCodes.UnknownError));
        }
    }
}