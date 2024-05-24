using Backend.Domain.Entities;
using Backend.Shared.Data;

namespace Backend.Domain.Repositories;

public interface IRentRepository : IRepository<Rent>
{
    Task<bool> HasRentToMotorcycle(Guid motorcycleId);
    Task<Plan?> GetPlanById(Guid planId);
    Task<IEnumerable<Plan>> GetPlans();
    Task<Rent?> GetRentByDriverId(Guid driverId);
    void Add(Rent rent);
    Task<Rent?> GetByMotorcycleId(Guid motorcycleId);
}