using Backend.Shared.Data;

namespace Backend.Infrastructure;

public class UnitOfWork(ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    public async Task<bool> CommitAsync()
    {
        return (await applicationDbContext.SaveChangesAsync()) > 0;
    }

    public bool Commit()
    {
        return (applicationDbContext.SaveChanges()) > 0;
    }
}