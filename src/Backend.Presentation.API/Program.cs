using Backend.Communication.Base;
using Backend.Presentation.API.Configurations;
using Backend.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

var configuration = configurationBuilder.Build();

var rabbitMqSection = configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqSettings>(rabbitMqSection);

var rabbitMqSettings = rabbitMqSection.Get<RabbitMqSettings>() ?? throw new InvalidOperationException(builder.Environment.EnvironmentName); 

builder.Services.AddApiConfigurations();
builder.Services.AddIdentityConfiguration(configuration);
builder.Services.AddSwaggerConfigurations();
builder.Services.AddDependencyInjections();
builder.Services.AddDatabaseConfiguration(configuration);

builder.Services.AddCommunicationSettings(rabbitMqSettings.HostName, rabbitMqSettings.UserName, rabbitMqSettings.Password);

var app = builder.Build();
 
app.ConfigureApiConfigurations(app.Environment);
app.UseSwaggerConfiguration(app.Environment);

app.Run();
