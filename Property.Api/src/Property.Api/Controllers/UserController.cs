using Ardalis.GuardClauses;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Property.Api.Contracts.Services;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHashids _hashids;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, IHashids hashids, ILogger<UserController> logger)
    {
        _userService = userService;
        _hashids = hashids;
        _logger = logger;
    }

    [HttpGet]
    // [Authorize(Policy = "CanGetUsersAdmin")]
    public async Task<IActionResult> AdminGetUser([FromQuery] string id)
    {
        try
        {
            var userId = _hashids.Decode(id).First();

            var user = await _userService.GetUserAsync(userId);

            return Ok(user);
        }
        catch (Exception e)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            _logger.LogError(e, "error getting user, request by {}", userId ?? "unknown");

            return BadRequest("An unknown error has occured");
        }
    }

    [HttpGet]
    // [Authorize(Policy = "CanReadCompanyUsers")]
    public async Task<IActionResult> GetUser([FromQuery] string id, [FromQuery] string companyId)
    {
        try
        {
            Guard.Against.NullOrWhiteSpace(id);
            Guard.Against.NullOrWhiteSpace(companyId);

            var userId = _hashids.Decode(id).First();
            var companyIdDecoded = _hashids.Decode(companyId).First();

            var user = await _userService.GetUserAsync(userId, companyIdDecoded);

            return Ok(user);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    // [Authorize(Policy = "CanReadCompanyUsers")]
    public async Task<IActionResult> GetUsers([FromQuery] string companyId)
    {
        try
        {
            Guard.Against.NullOrWhiteSpace(companyId);

            var decodedCompanyId = _hashids.DecodeSingle(companyId);

            var users = await _userService.GetUsers(decodedCompanyId);

            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error getting users");

            return BadRequest("An unknown error has occured");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            var newUser = await _userService.CreateUserAsync(userDto);

            return Ok(newUser);
        }
        catch(Exception e)
        {
            // Need check for duplicate email
            return BadRequest("An unknown error has occured");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userDto, [FromQuery] string? userId = null)
    {
        try
        {
            var userIdDecoded = userId == null
                ? _hashids.DecodeSingle(User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value)
                : _hashids.DecodeSingle(userId);

            var updatedUser = await _userService.UpdateUserAsync(userIdDecoded, userDto);

            return Ok(updatedUser);
        }
        catch
        {
            // TODO add logging and check for roles

            return BadRequest();
        }
    }
}