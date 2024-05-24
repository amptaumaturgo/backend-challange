using Backend.Communication.Base.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Backend.Communication.Base;

internal class RabbitMqPublisher(IRabbitMqConnectionManager connectionManager) : IMessagePublisher
{
    public Task Publish<T>(string exchange, string routingKey, T message) where T : class
    {
        using var channel = connectionManager.GetChannel();

        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);


        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);

        return Task.CompletedTask;
    }
}