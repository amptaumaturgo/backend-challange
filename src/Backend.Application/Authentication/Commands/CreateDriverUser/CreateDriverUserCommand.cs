using Backend.Domain.Entities.Enums;
using Backend.Shared.CQRS.Commands;
using FluentValidation.Results;

namespace Backend.Application.Authentication.Commands.CreateDriverUser;

public class CreateDriverUserCommand : Command
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string DriverLicenseNumber { get; set; } = string.Empty;
    public DriverLicenseType DriverLicenseType { get; set; } 
}