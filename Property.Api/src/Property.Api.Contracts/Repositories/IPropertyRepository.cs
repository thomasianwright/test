namespace Property.Api.Contracts.Repositories;
using PropertyModel = Entities.Models.Property;

public interface IPropertyRepository
{
    Task<PropertyModel?> GetPropertyByIdAsync(int id);
    Task<IEnumerable<PropertyModel?>> GetPropertiesAsync();
    Task<IEnumerable<PropertyModel?>> GetPropertiesAsync(int companyId);
    Task<PropertyModel?> AddPropertyAsync(PropertyModel? property);
    Task<PropertyModel?> UpdatePropertyAsync(PropertyModel? property);
    Task DeletePropertyAsync(PropertyModel? property);
}