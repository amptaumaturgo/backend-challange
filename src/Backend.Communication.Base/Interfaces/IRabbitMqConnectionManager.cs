using RabbitMQ.Client;

namespace Backend.Communication.Base.Interfaces;

public interface IRabbitMqConnectionManager
{
    IModel GetChannel();
}