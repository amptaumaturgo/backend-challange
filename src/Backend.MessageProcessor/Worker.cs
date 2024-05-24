using Backend.Communication.Base;
using Backend.MessageProcessor.Data;
using Backend.MessageProcessor.Data.Entities;
using Backend.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace Backend.MessageProcessor;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IOptions<RabbitMqSettings> rabbitOptions) : SubscriberRabbit<CreateMotorcycleEvent>(serviceProvider)
{ 
    protected override string Queue => "motorcycle_creation";
    protected override string Exchange => "Rent";
    protected override string RoutingKey => "create.motorcycle";

    protected override string HostName => rabbitOptions.Value.HostName;
    protected override string UserName => rabbitOptions.Value.UserName;
    protected override string Password => rabbitOptions.Value.Password;

    protected override async Task OnExecuting(IServiceProvider provider, CreateMotorcycleEvent model, CancellationToken cancellationToken)
    {
        logger.LogInformation("ExecuteAsync started");

        using var scope = provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WorkerDbContext>();

        var motorcycle = new Motorcycle
        {
            Model = model.Model,
            Plate = model.Plate,
            Year = model.Year
        };

        dbContext.Motorcycles.Add(motorcycle);

         await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);

    }
}
