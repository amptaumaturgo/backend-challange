using Backend.Domain.Entities.Enums;
using Backend.Shared.Domain.Base;

namespace Backend.Domain.Entities;

public class Rent : Entity, IAggregateRoot
{
    public Guid PlanId { get; private set; }
    public Plan Plan { get; private set; }

    public Guid DriverId { get; private set; }
    public Driver Driver { get; private set; }

    public Guid MotorcycleId { get; private set; }
    public Motorcycle Motorcycle { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime ExpectedEndDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public RentStatus? Status { get; private set; }

    public Rent( Guid driverId, Guid motorcycleId, Plan plan)
    {
        PlanId = plan.Id;
        Plan = plan;

        DriverId = driverId;
        MotorcycleId = motorcycleId;
        StartDate = CreatedAt.AddDays(1);

        ExpectedEndDate = StartDate.AddDays(plan.Days);

        Status = RentStatus.PendingConfirmation;
    }

    protected Rent() { }

    public void Finish()
    {
        EndDate = DateTime.Now;
        Status = RentStatus.Ended;
    }

    public void Cancel()
    {
        Status = RentStatus.Canceled;
    }
}