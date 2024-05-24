namespace Backend.Communication.Base.Interfaces;

public interface IMessagePublisher
{
    Task Publish<T>(string exchange, string routingKey, T message) where T : class;
}