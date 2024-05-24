using Backend.Application.Authentication.Commands.CreateDriverUser;
using Backend.Application.Authentication.Commands.Login;
using Backend.Application.Driver.Commands.SaveDriverLicenseImage;
using Backend.Presentation.API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.API.Controllers;

public class AuthenticationController(IMediator mediator) : MainController(mediator)
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command) => await SendCommandAsync(command);

    [HttpPost("register-driver")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterDriver(CreateDriverUserCommand command) => await SendCommandAsync(command);

    [Authorize(Roles = "Driver")]
    [HttpPost("send-driver-license")]
    public async Task<IActionResult> SendDriverLicense(SaveDriverLicenseImageCommand command) => await SendCommandAsync(command);
}