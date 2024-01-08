using System.Text;
using System.Text.Json;
using BlogHub.Domain.UserEvents;
using RabbitMQ.Client;

namespace BlogHub.Identity.Services;

public class UserEventService : IUserEventService, IDisposable
{
    private readonly string _createdKey = "created";
    private readonly string _deletedKey = "deleted";
    private readonly string _eventCreatedKey = "user-created";
    private readonly string _eventDeletedKey = "user-deleted";
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchange;

    public UserEventService(string hostName, string userName, string password, string exchange)
    {
        _exchange = exchange;

        var factory = new ConnectionFactory();
        factory.HostName = hostName;
        factory.UserName = userName;
        factory.Password = password;

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_exchange, ExchangeType.Direct);

        _channel.QueueDeclare(
                queue: _eventCreatedKey,
                durable: false,  
                exclusive: false, 
                autoDelete: false,
                arguments: null);

        _channel.QueueDeclare(
                queue: _eventDeletedKey,
                durable: false,  
                exclusive: false, 
                autoDelete: false,
                arguments: null);

        _channel.QueueBind(_eventCreatedKey, _exchange, _createdKey, null);
        _channel.QueueBind(_eventDeletedKey, _exchange, _deletedKey, null);
    }

    public void Publish(UserCreatedEvent userCreatedEvent)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userCreatedEvent)); 
        Publish(body, _createdKey);
    }

    public void Publish(UserDeletedEvent userDeletedEvent)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userDeletedEvent)); 
        Publish(body, _deletedKey);
    }

    public void Dispose()
    {
        _channel.Close();
        _channel.Dispose();
        _connection.Close();
        _connection.Dispose();
    }

    private void Publish(byte[] body, string key)
    {
        _channel.BasicPublish(
            exchange: _exchange,
            routingKey: key,
            basicProperties: null,
            body: body);
    }
}