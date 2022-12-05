namespace Property.Api.Core.Models;

public class UpdateAccountDto
{
    public string Name { get; set; }
    public ICollection<UserDto> Users { get; set; }
    public ICollection<RentalAgreementDto> RentalAgreements { get; set; }

    public string AccountUserOwnerId { get; set; }
}