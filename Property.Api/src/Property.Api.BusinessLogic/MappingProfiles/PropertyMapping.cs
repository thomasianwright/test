using AutoMapper;
using Property.Api.BusinessLogic.MappingProfiles.ValueConverters;
using Property.Api.Core.Models;
using PropertyModel = Property.Api.Entities.Models.Property;

namespace Property.Api.BusinessLogic.MappingProfiles;

public class PropertyMapping : Profile
{
    public PropertyMapping()
    {
        CreateMap<PropertyModel, PropertyDto>()
            .ForMember(x => x.Id, opt => opt.ConvertUsing<IntToHash, int?>());

        CreateMap<CreatePropertyDto, PropertyModel>();

        CreateMap<UpdatePropertyDto, PropertyModel>()
            .ForMember(x => x.Id, opt => opt.ConvertUsing<IntFromHash, string?>());
    }
}