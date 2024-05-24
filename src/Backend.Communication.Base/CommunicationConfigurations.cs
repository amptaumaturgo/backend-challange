using Backend.Communication.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Communication.Base;

public static class CommunicationConfigurations
{
    public static void AddCommunicationSettings(this IServiceCollection services, string hostname, string username, string password)
    {
        services.AddSingleton<IRabbitMqConnectionManager>(_ => new RabbitMqConnectionManager(hostname, username, password));

        services.AddScoped<IMessagePublisher, RabbitMqPublisher>(); 
    }
}