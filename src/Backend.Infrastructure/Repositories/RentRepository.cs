using Backend.Domain.Entities;
using Backend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class RentRepository(ApplicationDbContext applicationDbContext) : IRentRepository
{
    public async Task<bool> HasRentToMotorcycle(Guid motorcycleId)
    {
        var hasRentToMotorcycle = await applicationDbContext.Rents.AnyAsync(x => x.MotorcycleId == motorcycleId);
        return hasRentToMotorcycle;
    }

    public async Task<Plan?> GetPlanById(Guid planId)
    {
        return await applicationDbContext.Plans.FirstOrDefaultAsync(x => x.Id == planId);
    }

    public async Task<IEnumerable<Plan>> GetPlans()
    {
        var plans = await applicationDbContext.Plans.ToListAsync();
        return plans;
    }

    public async Task<Rent?> GetRentByDriverId(Guid driverId)
    {
        var rent =
            await applicationDbContext.Rents.FirstOrDefaultAsync(
                x => x.DriverId == driverId && x.EndDate == null);

        return rent;
    }

    public void Add(Rent rent)
    {
        applicationDbContext.Rents.Add(rent);
    }

    public async Task<Rent?> GetByMotorcycleId(Guid motorcycleId)
    {
        var motorcycle = await applicationDbContext.Rents.FirstOrDefaultAsync(x => x.MotorcycleId == motorcycleId);

        return motorcycle;
    }
}