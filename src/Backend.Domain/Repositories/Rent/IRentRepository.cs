using Backend.Domain.Entities;
using Backend.Shared.Data;

namespace Backend.Domain.Repositories.Rent;

public interface IRentRepository : IRepository<Entities.Rent>
{
    Task<bool> HasRentToMotorcycle(Guid motorcycleId);
    Task<Plan?> GetPlanById(Guid planId);
    Task<IEnumerable<Plan>> GetPlans();
    Task<Entities.Rent?> GetRentByDriverId(Guid driverId);
    void Add(Entities.Rent rent);
    Task<Entities.Rent?> GetByMotorcycleId(Guid motorcycleId);
}