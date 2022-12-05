using AutoMapper;
using Property.Api.BusinessLogic.MappingProfiles.ValueConverters;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.MappingProfiles;

public class UserMapping : Profile
{
    public UserMapping()
    {
        
        CreateMap<User, UserDto>()
            .ForMember(d => d.Id, opt => opt.ConvertUsing<IntToHash, int?>())
            .ForMember(d => d.UserCompanyId, opt => opt.ConvertUsing<IntToHash, int?>());

        CreateMap<UpdateUserDto, User>();

        CreateMap<CreateUserDto, User>();
    }
}