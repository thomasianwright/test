using AutoMapper;
using Property.Api.BusinessLogic.MappingProfiles.ValueConverters;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.MappingProfiles;

public class AddressMapping : Profile
{
    public AddressMapping()
    {
        CreateMap<Address, AddressDto>()
            .ForMember(x => x.Id, opt => opt.ConvertUsing<IntToHash, int?>());

        CreateMap<CreateAddressDto, Address>();
    }
}