using System.Text.Json;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Property.Api.Contracts.Services;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IHashids _hashids;
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IHashids hashids, IAccountService accountService, ILogger<AccountController> logger)
    {
        _hashids = hashids;
        _accountService = accountService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(AccountDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> GetAccount([FromQuery] string id)
    {
        try
        {
            var accountId = _hashids.DecodeSingle(id);

            var account = await _accountService.GetAccount(accountId);

            return Ok(account);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto request)
    {
        try
        {
            var account = await _accountService.CreateAccount(request);

            return Ok(account);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "An error occured while creating an account");
            return BadRequest(JsonSerializer.Serialize(e));
        }
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(AccountDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDto request, [FromQuery] string id)
    {
        try
        {
            var accountId = _hashids.DecodeSingle(id);
            
            var account = await _accountService.UpdateAccount(accountId, request);

            return Ok(account);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    [ProducesResponseType(typeof(AccountDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> RemoveAccount([FromQuery] string id)
    {
        try
        {
            var accountId = _hashids.DecodeSingle(id);

            await _accountService.DeleteAccount(accountId);

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}