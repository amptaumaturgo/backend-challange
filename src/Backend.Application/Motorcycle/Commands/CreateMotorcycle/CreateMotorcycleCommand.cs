using Backend.Domain.Repositories;
using Backend.Shared.CQRS.Commands;
using Backend.Shared.Data;
using MediatR;

namespace Backend.Application.Motorcycle.Commands.CreateMotorcycle;

public class CreateMotorcycleCommand : Command
{
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
}