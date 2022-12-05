namespace Property.Api.Core.Models;

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleNames { get; set; }
    public string Email { get; set; }

    public AddressDto Address { get; set; }

    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
}