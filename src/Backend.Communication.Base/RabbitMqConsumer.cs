using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Backend.Communication.Base;

public abstract class SubscriberRabbit<TModel> : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    protected abstract string Queue { get; }
    protected abstract string Exchange { get; }
    protected abstract string RoutingKey { get; }
    protected virtual string SubscribeExchangeType => ExchangeType.Direct;
    protected virtual ushort PrefetchCount => 1;

    protected abstract string HostName { get; }
    protected abstract string UserName { get; }
    protected abstract string Password { get; }

    protected SubscriberRabbit(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = (serviceProvider.GetService<ILoggerFactory>() ?? throw new InvalidOperationException()).CreateLogger(GetType());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting rabbitmq process");
        _logger.LogInformation("HostName " + HostName);
        _logger.LogInformation("PassWord " + Password);
        _logger.LogInformation("UserName " + UserName);
        var factory = new ConnectionFactory
        {
            HostName = HostName,
            UserName = UserName,
            Password = Password,
            Port = 5672,
            VirtualHost = "/",
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            RequestedHeartbeat = TimeSpan.FromSeconds(30), 
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(Exchange, SubscribeExchangeType, true);
        channel.QueueDeclare(queue: Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(Queue, Exchange, RoutingKey);
        channel.BasicQos(0, PrefetchCount, false);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation(message);

                var deserializedMessage = JsonSerializer.Deserialize<TModel>(message);

                try
                {
                    await OnExecuting(scope.ServiceProvider, deserializedMessage!, stoppingToken);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(Queue, false, consumer);


            await Task.Delay(500, stoppingToken);

        }
    }

    protected abstract Task OnExecuting(IServiceProvider provider, TModel model, CancellationToken cancellationToken);
}