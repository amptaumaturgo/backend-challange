using Backend.Domain.Entities;
using Backend.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class DriverRepository(ApplicationDbContext applicationDbContext) : IDriverRepository
{
    public async Task<Driver?> GetByIdAsync(Guid driverId)
    {
        var driver = await applicationDbContext.Drivers.FirstOrDefaultAsync(x => x.Id == driverId);

        return driver;
    }

    public async Task<bool> ExistentCnpj(string cnpj)
    {
        return await applicationDbContext.Drivers.AnyAsync(x => x.Cnpj.Value == cnpj);
    }

    public void Add(Driver driver)
    {
        applicationDbContext.Drivers.Add(driver);
    }

    public void Update(Driver driver)
    {
        applicationDbContext.Drivers.Update(driver);
    }

    public async Task<Driver?> GetByUserId(Guid userId)
    {
        var driver = await applicationDbContext.Drivers.FirstOrDefaultAsync(x => x.UserId == userId);
        return driver;
    }
}