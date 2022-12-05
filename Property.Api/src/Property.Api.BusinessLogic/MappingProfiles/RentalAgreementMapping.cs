using AutoMapper;
using Property.Api.BusinessLogic.MappingProfiles.ValueConverters;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.MappingProfiles;

public class RentalAgreementMapping : Profile
{
    public RentalAgreementMapping()
    {
        CreateMap<RentalAgreement, RentalAgreementDto>()
            .ForMember(x => x.Id, 
                opt => opt.ConvertUsing<IntToHash, int?>())
            .ForMember(x => x.RentalAgreementAccountId, 
                opt => opt.ConvertUsing<IntToHash, int?>())
            .ForMember(x => x.RentalAgreementPropertyId, 
                opt => opt.ConvertUsing<IntToHash, int?>())
            .ForMember(x => x.RentalAgreementCompanyId, 
                opt => opt.ConvertUsing<IntToHash, int?>());
    }
}