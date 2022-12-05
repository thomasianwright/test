namespace Property.Api.Core.Models;

public class UserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleNames { get; set; }
    public string Email { get; set; }

    public AddressDto? Address { get; set; }

    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }

    public ICollection<RentalAgreementDto>? RentalAgreements { get; set; }

    public ICollection<AccountDto>? Accounts { get; set; }

    public string? UserCompanyId { get; set; }
    public CompanyDto? Company { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}