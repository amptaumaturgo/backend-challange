using Backend.MessageProcessor;
using Backend.MessageProcessor.Data;
using Backend.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

Thread.Sleep(20000);

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

var configuration = configurationBuilder.Build();

var rabbitMqSection = configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqSettings>(rabbitMqSection);

bool IsRabbitMqAvailable(string hostName, string userName, string password, int port, int maxRetries, TimeSpan delay)
{
    var factory = new ConnectionFactory
    {
        HostName = hostName,
        UserName = userName,
        Password = password,
        Port = port
    };

    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            Console.WriteLine("Connected to RabbitMQ successfully.");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Attempt {i + 1} to connect to RabbitMQ failed. Exception: {e.Message}");

            if (i == maxRetries - 1)
            {
                Console.WriteLine("Maximum retry attempts reached. Exiting application.");
                throw;
            }

            Thread.Sleep(delay);
        }
    }

    return false;
}

var maxRetries = 3;
var delay = TimeSpan.FromSeconds(15);
var rabbitMqSettings = rabbitMqSection.Get<RabbitMqSettings>() ?? throw new InvalidOperationException(builder.Environment.EnvironmentName);

if (!IsRabbitMqAvailable(rabbitMqSettings.HostName, rabbitMqSettings.UserName, rabbitMqSettings.Password, 5672, maxRetries, delay))
{
    throw new Exception("Could not connect to RabbitMQ after multiple attempts.");
}


builder.Services.AddHostedService<Worker>();
 
builder.Services.AddDbContext<WorkerDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information));

var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Starting host");
await host.RunAsync();
logger.LogInformation("Host started");