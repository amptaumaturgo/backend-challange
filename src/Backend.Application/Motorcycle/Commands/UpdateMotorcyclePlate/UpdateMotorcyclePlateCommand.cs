using System.Text.Json.Serialization;
using Backend.Shared.CQRS.Commands;

namespace Backend.Application.Motorcycle.Commands.UpdateMotorcyclePlate;

public class UpdateMotorcyclePlateCommand : Command
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Plate { get; set; } = string.Empty;
}