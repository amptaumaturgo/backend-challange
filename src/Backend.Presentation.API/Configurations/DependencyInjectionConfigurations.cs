using Backend.Application;
using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Repositories;
using Backend.Infrastructure;
using Backend.Infrastructure.Repositories;
using Backend.Shared.Data;

namespace Backend.Presentation.API.Configurations;

public static class DependencyInjectionConfigurations
{
    public static void AddDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IRentRepository, RentRepository>();

        services.AddTransient<IRentCalculationStrategyFactory, RentCalculationStrategyFactory>();

        services.AddApplicationConfigurations();

    }
}