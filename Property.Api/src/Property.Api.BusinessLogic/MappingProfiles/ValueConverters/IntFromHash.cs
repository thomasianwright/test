using AutoMapper;
using HashidsNet;

namespace Property.Api.BusinessLogic.MappingProfiles.ValueConverters;

public class IntFromHash : IValueConverter<string, int>
{
    private readonly IHashids hashids;

    public IntFromHash(IHashids hashids)
    {
        this.hashids = hashids;
    }

    public int Convert(string source, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source)) return -1;
        return hashids.DecodeSingle(source);
    }
}