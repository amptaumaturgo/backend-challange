using Backend.Application.Authentication.Commands.Login;
using Backend.Shared.CQRS.Commands;

namespace Backend.Application.Motorcycle.Commands.RemoveMotorcycle;

public class RemoveMotorcycleCommand : Command
{
    public Guid Id { get; set; }
}