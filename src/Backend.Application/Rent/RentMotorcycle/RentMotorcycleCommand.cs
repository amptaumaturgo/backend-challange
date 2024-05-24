using System.Text.Json.Serialization;
using Backend.Shared.CQRS.Commands;

namespace Backend.Application.Rent.RentMotorcycle;

public class RentMotorcycleCommand : Command
{
    [JsonIgnore]
    public Guid DriverId { get; set; }
    public Guid MotorcycleId { get; set; }
    public Guid PlanId { get; set; } 

    public DateTime ExpectedDevolutionDate { get; set; }
}