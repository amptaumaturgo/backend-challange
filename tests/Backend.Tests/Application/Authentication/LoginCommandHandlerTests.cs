using Backend.Application.Authentication.Commands.Login;
using Backend.Domain.Entities;
using Backend.Domain.Repositories;
using Backend.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace Backend.Tests.Application.Authentication;

public class LoginCommandHandlerTests
{
    private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
    private readonly Mock<IDriverRepository> _driverRepositoryMock;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
        _signInManagerMock = new Mock<SignInManager<IdentityUser>>(_userManagerMock.Object, contextAccessorMock.Object, claimsFactoryMock.Object, null!, null!, null!, null!);

        _driverRepositoryMock = new Mock<IDriverRepository>();

        IOptions<AuthenticationSettings> authOptions = Options.Create(new AuthenticationSettings
        {
            Secret = "super_secret_key_1234567890123456",
            ExpirationTime = 1
        });

        _handler = new LoginCommandHandler(
            _signInManagerMock.Object,
            _driverRepositoryMock.Object,
            _userManagerMock.Object,
            authOptions);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnValidationError()
    {
        // Arrange
        var command = new LoginCommand { Email = "", Password = "" };

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(response.Errors, e => e == "'Email' must not be empty.");
    }

    [Fact]
    public async Task Handle_InvalidCredentials_ShouldReturnFailResponse()
    {
        // Arrange
        var command = new LoginCommand { Email = "user@example.com", Password = "wrongpassword" };
        _signInManagerMock
            .Setup(s => s.PasswordSignInAsync(command.Email, command.Password, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(response.Errors, e => e == "Invalid login attempt.");
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnSuccessResponseWithToken()
    {
        // Arrange
        var command = new LoginCommand { Email = "user@example.com", Password = "password" };
        var user = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = command.Email };

        _signInManagerMock
            .Setup(s => s.PasswordSignInAsync(command.Email, command.Password, false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.GetClaimsAsync(user))
            .ReturnsAsync(new List<Claim>());

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        _driverRepositoryMock
            .Setup(d => d.GetByUserId(It.IsAny<Guid>()))
            .ReturnsAsync((Driver)null!);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.IsType<LoginCommandResponse>(response.Result);
        var token = ((LoginCommandResponse)response.Result!).Token;
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }
}