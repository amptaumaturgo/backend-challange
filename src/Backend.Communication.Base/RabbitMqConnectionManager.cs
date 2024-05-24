using Backend.Communication.Base.Interfaces;
using RabbitMQ.Client;

namespace Backend.Communication.Base;

public class RabbitMqConnectionManager : IRabbitMqConnectionManager
{
    private readonly IConnection _connection;

    public RabbitMqConnectionManager(string hostname, string username, string password)
    {
        var factory = new ConnectionFactory() { HostName = hostname, UserName = username, Password = password };
        _connection = factory.CreateConnection();
    }

    public IModel GetChannel()
    {
        return _connection.CreateModel();
    } 
}