namespace Backend.Communication.Base.Interfaces;

public interface IMessageConsumer
{
    Task ConsumeAsync<T>(string exchange, string exchangeType, string queueName, string routingKey,  Func<T, Task> handleMessage);
}