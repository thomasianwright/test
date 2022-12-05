using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Property.Api.Contracts.Services;
using Property.Api.Core.Models;

namespace Property.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IHashids _hashids;
    private readonly ILogger<PropertyController> _logger;

    public PropertyController(IPropertyService propertyService, IHashids hashids, ILogger<PropertyController> logger)
    {
        _propertyService = propertyService;
        _hashids = hashids;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PropertyDto), 200)]
    public async Task<IActionResult> GetProperty([FromQuery] string id)
    {
        try
        {
            var propertyId = _hashids.DecodeSingle(id);

            var property = await _propertyService.GetPropertyByIdAsync(propertyId);

            return Ok(property);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PropertyDto>), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> GetProperties([FromQuery] string id)
    {
        try
        {
            var companyId = _hashids.DecodeSingle(id);

            var properties = await _propertyService.GetProperties(companyId);

            return Ok(properties);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(PropertyDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> Create([FromBody] CreatePropertyDto request)
    {
        try
        {
            var property = await _propertyService.CreateProperty(request);

            return Ok(property);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(PropertyDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> Update([FromBody] UpdatePropertyDto request, [FromQuery] string id)
    {
        try
        {
            var propertyId = _hashids.DecodeSingle(id);
            var property = await _propertyService.UpdateProperty(request, propertyId);

            return Ok(property);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpDelete]
    [ProducesResponseType(typeof(PropertyDto), 200)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
        try
        {
            var propertyId = _hashids.DecodeSingle(id);
            await _propertyService.DeleteProperty(propertyId);

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}