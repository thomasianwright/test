using Property.Api.Core.Models;
using PropertyModel = Property.Api.Entities.Models.Property;

namespace Property.Api.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ApiContext _apiContext;

    public PropertyRepository(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }
    
    public async Task<PropertyModel?> GetPropertyByIdAsync(int id)
    {
        return await _apiContext.Properties.FirstOrDefaultAsync(x=> x!.Id == id);
    }
    
    public async Task<IEnumerable<PropertyModel?>> GetPropertiesAsync()
    {
        return await _apiContext.Properties.ToListAsync();
    }
    
    public async Task<IEnumerable<PropertyModel?>> GetPropertiesAsync(int companyId)
    {
        return await _apiContext.Properties.Where(x => x!.PropertyCompanyId == companyId).ToListAsync();
    }

    public async Task<PropertyModel?> AddPropertyAsync(PropertyModel? property)
    {
        var addedProperty = await _apiContext.Properties.AddAsync(property);
        await _apiContext.SaveChangesAsync();

        return addedProperty.Entity;
    }
    
    public async Task<PropertyModel?> UpdatePropertyAsync(PropertyModel? property)
    {
        var updatedProperty = _apiContext.Properties.Update(property);
        await _apiContext.SaveChangesAsync();

        return updatedProperty.Entity;
    }
    
    public async Task DeletePropertyAsync(PropertyModel? property)
    {
        _apiContext.Properties.Remove(property);
        await _apiContext.SaveChangesAsync();
    }
}