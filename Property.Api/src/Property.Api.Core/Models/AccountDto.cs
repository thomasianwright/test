using System.Diagnostics.CodeAnalysis;

namespace Property.Api.Core.Models;

public class AccountDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ICollection<UserDto>? Users { get; set; }
    public ICollection<RentalAgreementDto>? RentalAgreements { get; set; }

    public string? AccountUserOwnerId { get; set; }
    public UserDto? AccountOwner { get; set; }
}