using HashidsNet;
using Microsoft.Extensions.Logging;

namespace Property.Api.BusinessLogic.Tests.Services;

public class AuthenticationServiceTests
{
    public AuthenticationServiceTests()
    {
    }

    [Fact]
    public async Task Assert_Signup_Of_User_Without_Password_Sends_Password_Via_EmailService()
    {
        var emailService = new Mock<IEmailService>();
        var userRepository = new Mock<IUserRepository>();
        var passwordResetRepository = new Mock<IPasswordResetRepository>();
        var logger = new Mock<ILogger<AuthenticationService>>();
        
        var hashids = new Hashids("test");
        
        var mapper = new Mapper(new MapperConfiguration((config =>
        {
            config.AddProfiles(new Profile[]
            {
                new UserMapping(), new AddressMapping(), new AccountMapping(), new CompanyMapping(),
                new PropertyMapping(), new AccountMapping(), new RentalAgreementMapping()
            });
        })));

        var authenticationService = new AuthenticationService(userRepository.Object, mapper, emailService.Object, passwordResetRepository.Object, hashids, logger.Object);

        emailService.Setup(x => x.SendNewUserEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var registerDto = new CreateUserDto
        {
            FirstName = "Thomas",
            LastName = "Wright",
            Email = "thomas.wright@example.com",
            Address = new CreateAddressDto
            {
                LineOne = "123 Fake Street",
                LineTwo = "Fake Town",
                Postcode = "FAKE1",
                Country = "Fake Country",
                County = "Fake County"
            },
            Password = null
        };

        var user = await authenticationService.Register(registerDto);

        emailService.Verify(x => x.SendNewUserEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);

        user!.FirstName.Should().BeSameAs(registerDto.FirstName);
        user.LastName.Should().BeSameAs(registerDto.LastName);
        user.Email.Should().BeSameAs(registerDto.Email);
        user.Address.LineOne.Should().BeSameAs(registerDto.Address.LineOne);
        user.Address.LineTwo.Should().BeSameAs(registerDto.Address.LineTwo);
        user.Address.Postcode.Should().BeSameAs(registerDto.Address.Postcode);
        user.Address.Country.Should().BeSameAs(registerDto.Address.Country);
        user.Address.County.Should().BeSameAs(registerDto.Address.County);
    }

    [Fact]
    public async Task Assert_Signup_Of_User_With_Password_Does_Not_Send_Password_Via_EmailService()
    {
        var emailService = new Mock<IEmailService>();
        var userRepository = new Mock<IUserRepository>();
        var passwordResetRepository = new Mock<IPasswordResetRepository>();
        var hashids = new Hashids("test");
        var logger = new Mock<ILogger<AuthenticationService>>();

        var mapper = new Mapper(new MapperConfiguration((config =>
        {
            config.AddProfiles(new Profile[]
            {
                new UserMapping(), new AddressMapping(), new AccountMapping(), new CompanyMapping(),
                new PropertyMapping(), new AccountMapping(), new RentalAgreementMapping()
            });
        })));

        var authenticationService = new AuthenticationService(userRepository.Object, mapper, emailService.Object, passwordResetRepository.Object, hashids, logger.Object);

        emailService.Setup(x => x.SendNewUserEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var registerDto = new CreateUserDto
        {
            FirstName = "Thomas",
            LastName = "Wright",
            Email = "thomas.wright@example.com",
            Address = new CreateAddressDto
            {
                LineOne = "123 Fake Street",
                LineTwo = "Fake Town",
                Postcode = "FAKE1",
                Country = "Fake Country",
                County = "Fake County"
            },
            Password = "TestPass"
        };

        var user = await authenticationService.Register(registerDto);

        emailService.Verify(x => x.SendNewUserEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);

        user!.FirstName.Should().BeSameAs(registerDto.FirstName);
        user.LastName.Should().BeSameAs(registerDto.LastName);
        user.Email.Should().BeSameAs(registerDto.Email);
    }
}