using AutoMapper;
using Property.Api.BusinessLogic.MappingProfiles.ValueConverters;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.MappingProfiles;

public class CompanyMapping : Profile
{
    public CompanyMapping()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(x => x.Id, opt => opt.ConvertUsing<IntToHash, int?>());
    }
}