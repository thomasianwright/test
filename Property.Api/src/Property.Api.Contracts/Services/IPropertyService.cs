using Property.Api.Core.Models;

namespace Property.Api.Contracts.Services;

public interface IPropertyService
{
    Task<PropertyDto?> GetPropertyByIdAsync(int id);
    Task<IEnumerable<PropertyDto>> GetProperties(int companyId);
    Task<PropertyDto> UpdateProperty(UpdatePropertyDto updatePropertyDto, int propertyId);
    Task<PropertyDto> CreateProperty(CreatePropertyDto createPropertyDto);
    Task DeleteProperty(int propertyId);
}