using Ardalis.GuardClauses;
using AutoMapper;
using Property.Api.Contracts.Repositories;
using Property.Api.Contracts.Services;
using Property.Api.Core.Models;
using PropertyModel = Property.Api.Entities.Models.Property;

namespace Property.Api.BusinessLogic.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }
    
    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = await _propertyRepository.GetPropertyByIdAsync(id);
        return _mapper.Map<PropertyDto>(property);
    }

    public async Task<IEnumerable<PropertyDto>> GetProperties(int companyId)
    {
        var properties = await _propertyRepository.GetPropertiesAsync(companyId);

        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto> UpdateProperty(UpdatePropertyDto updatePropertyDto, int propertyId)
    {
        var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);

        Guard.Against.Null(property);
        
        property = _mapper.Map(updatePropertyDto, property);
        
        var updatedProperty = await _propertyRepository.UpdatePropertyAsync(property);

        return _mapper.Map<PropertyDto>(updatedProperty);
    }
    
    public async Task<PropertyDto> CreateProperty(CreatePropertyDto createPropertyDto)
    {
        var property = _mapper.Map<PropertyModel>(createPropertyDto);
        
        var createdProperty = await _propertyRepository.AddPropertyAsync(property);

        return _mapper.Map<PropertyDto>(createdProperty);
    }
    
    public async Task DeleteProperty(int propertyId)
    {
        var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
        
        await _propertyRepository.DeletePropertyAsync(property);
    }
}