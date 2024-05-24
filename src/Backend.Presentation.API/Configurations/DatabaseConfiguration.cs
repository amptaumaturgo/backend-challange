using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Presentation.API.Configurations;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()  
                .LogTo(Console.WriteLine, LogLevel.Information));

        services.AddTransient<InitializerSeedData>();

        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var initializer = serviceProvider.GetRequiredService<InitializerSeedData>();
        initializer.ApplyMigrationsAndSeedData().Wait();
    }
}