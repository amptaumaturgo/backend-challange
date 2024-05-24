using Backend.Shared.CQRS.Commands;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection; 
using Backend.Application.Authentication.Commands.Login;
using Backend.Application.Authentication.Commands.CreateDriverUser;
using Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;

namespace Backend.Application;

public static class ApplicationConfigurations
{
    public static void AddApplicationConfigurations(this IServiceCollection services)
    {
        services.AddScoped<IValidatableCommand, LoginCommand>();  
        services.AddScoped<IValidatableCommand, CreateDriverUserCommand>();

        services.AddScoped<ISpecification<Domain.Entities.Driver>, DriverSpecification>();
        services.AddScoped<ISpecification<Domain.Entities.Rent>, RentSpecification>();
        services.AddScoped<ISpecification<Domain.Entities.Motorcycle>, MotorcycleSpecification>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()); 
        });

        services.AddFluentValidationAutoValidation();

        services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
    }
}