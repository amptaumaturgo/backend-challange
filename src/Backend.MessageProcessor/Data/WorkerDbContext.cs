using Backend.MessageProcessor.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.MessageProcessor.Data;

public class WorkerDbContext  : DbContext
{
    public WorkerDbContext(DbContextOptions<WorkerDbContext> opt) : base(opt)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
    public DbSet<Motorcycle> Motorcycles { get; set; }
}