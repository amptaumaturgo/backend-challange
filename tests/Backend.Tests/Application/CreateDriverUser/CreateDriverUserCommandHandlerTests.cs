using Backend.Application.Authentication.Commands.CreateDriverUser;
using Backend.Domain.Entities.Enums;
using Backend.Domain.Repositories.Driver;
using Backend.Shared.Data;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Backend.Tests.Application.CreateDriverUser;

public class CreateDriverUserCommandHandlerTests
{
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IDriverRepository> _driverRepositoryMock;
    private readonly CreateDriverUserCommandHandler _handler;

    public CreateDriverUserCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _driverRepositoryMock = new Mock<IDriverRepository>();
        Mock<IUnitOfWork> unitOfWorkMock = new();

        _handler = new CreateDriverUserCommandHandler(
            _userManagerMock.Object,
            _driverRepositoryMock.Object,
            unitOfWorkMock.Object);
    }

    private CreateDriverUserCommand GenerateCommandPayload()
     => new CreateDriverUserCommand
     {
         Name = "User Tester",
         BirthDate = DateTime.Now.AddDays(-36500),
         Email = "user@example.com",
         Password = "password",
         Cnpj = "12345678901234",
         DriverLicenseNumber = "1234567",
         DriverLicenseType = DriverLicenseType.A
     };

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnValidationError()
    {
        // Arrange
        var command = new CreateDriverUserCommand { Email = "", Password = "" };

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(response.Errors, e => e == "Name is required.");
        Assert.Contains(response.Errors, e => e == "Birth date is required.");
        Assert.Contains(response.Errors, e => e == "Driver's license number is required.");
        Assert.Contains(response.Errors, e => e == "Driver's license number must be 7 characters long.");
        Assert.Contains(response.Errors, e => e == "Driver's license type is required.");
    }

    [Fact]
    public async Task Handle_ExistentCnpj_ShouldReturnFailResponse()
    {
        // Arrange
        var command = GenerateCommandPayload();

        _driverRepositoryMock.Setup(repo => repo.ExistentCnpj(command.Cnpj)).ReturnsAsync(true);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(response.Errors, e => e == "Cnpj existent, please try with other Cnpj.");
    }

    [Fact]
    public async Task Handle_CreateUserFails_ShouldReturnFailResponse()
    {
        // Arrange
        var command = GenerateCommandPayload();

        _driverRepositoryMock.Setup(repo => repo.ExistentCnpj(command.Cnpj)).ReturnsAsync(false);
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), command.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(response.Errors, e => e == "User creation failed");
    }

    [Fact]
    public async Task Handle_Success_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = GenerateCommandPayload();
         
        _driverRepositoryMock.Setup(repo => repo.ExistentCnpj(command.Cnpj)).ReturnsAsync(false);
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "Driver"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Success to create your access.", response.Result );
    }
}
