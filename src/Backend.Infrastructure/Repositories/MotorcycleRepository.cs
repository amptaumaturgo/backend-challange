using System.Runtime.InteropServices;
using Backend.Domain.Entities;
using Backend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class MotorcycleRepository(ApplicationDbContext applicationDbContext) : IMotorcycleRepository
{
    public void Add(Motorcycle motorcycle)
    {
        applicationDbContext.Motorcycles.Add(motorcycle);
    }

    public void Update(Motorcycle motorcycle)
    {
        applicationDbContext.Motorcycles.Update(motorcycle);
    }

    public void Remove(Motorcycle motorcycle)
    {
        applicationDbContext.Motorcycles.Remove(motorcycle);
    }

    public async Task<Motorcycle?> GetByIdAsync(Guid id)
    {
        var motorcycle = await applicationDbContext.Motorcycles.FirstOrDefaultAsync(x => x.Id == id);

        return motorcycle;
    }

    public async Task<Motorcycle?> GetByPlate(string plate)
    {
        var motorcycle = await applicationDbContext.Motorcycles.FirstOrDefaultAsync(x => x.Plate == plate);

        return motorcycle;
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync()
    {
        var motorcycles = await applicationDbContext.Motorcycles.ToListAsync();

        return motorcycles;
    }
}