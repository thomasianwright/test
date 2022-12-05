namespace Property.Api.Core.Models;

public class CreateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleNames { get; set; }
    public string? Password { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public CreateAddressDto Address { get; set; }
}