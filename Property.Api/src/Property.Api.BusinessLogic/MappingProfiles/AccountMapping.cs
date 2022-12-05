using AutoMapper;
using Property.Api.BusinessLogic.MappingProfiles.ValueConverters;
using Property.Api.Core.Models;
using Property.Api.Entities.Models;

namespace Property.Api.BusinessLogic.MappingProfiles;

public class AccountMapping : Profile
{
    public AccountMapping()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(d => d.Id, opt => opt.ConvertUsing<IntToHash, int?>())
            .ForMember(d => d.AccountUserOwnerId, opt => opt.ConvertUsing<IntToHash, int?>());

        CreateMap<CreateAccountDto, Account>()
            .ForMember(x => x.AccountUserOwnerId, opt => opt.ConvertUsing<IntFromHash, string?>());

        CreateMap<UpdateAccountDto, Account>()
            .ForMember(x => x.Id, opt => opt.ConvertUsing<IntFromHash, string?>())
            .ForMember(x => x.AccountUserOwnerId, opt => opt.ConvertUsing<IntFromHash, string?>());
    }
}