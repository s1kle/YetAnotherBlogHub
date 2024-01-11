using System.Text;
using System.Text.Json;
using BlogHub.Data.Users.Commands.Create;
using BlogHub.Data.Users.Commands.Delete;
using BlogHub.Domain.UserEvents;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace BlogHub.Api.Services;

public class EventConsumerService : BackgroundService
{
	private readonly string _createdKey = "created";
    private readonly string _deletedKey = "deleted";
    private readonly string _eventCreatedKey = "user-created";
    private readonly string _eventDeletedKey = "user-deleted";
    private IConnection _connection;
	private IModel _channel;
	private IServiceProvider _provider;

	public EventConsumerService(
		string hostName,
		string userName, 
		string password, 
		string exchange,
		IServiceProvider provider)
	{
		_provider = provider;

		var factory = new ConnectionFactory { HostName = hostName, UserName = userName, Password = password };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();

		_channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);
        
        _channel.QueueDeclare(_eventCreatedKey, false, false, false, null);
        _channel.QueueBind(_eventCreatedKey, exchange, _createdKey, null);
        
        _channel.QueueDeclare(_eventDeletedKey, false, false, false, null);
        _channel.QueueBind(_eventDeletedKey, exchange, _deletedKey, null); 
	}
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

		var consumer = new EventingBasicConsumer(_channel);
		consumer.Received += (ch, ea) =>
		{
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
			var key = ea.RoutingKey;

			Log.Information("Message reсeived: {key} - {message}", key, message);

			if (key.Equals(_createdKey))
				HandleUserCreated(message);
			if(key.Equals(_deletedKey))
				HandleUserDeleted(message);
			
			var ack = ea.DeliveryTag;

			_channel.BasicAck(ack, false);
		};

		_channel.BasicConsume(_eventCreatedKey, false, consumer);
		_channel.BasicConsume(_eventDeletedKey, false, consumer);

		return Task.CompletedTask;
	}

	public async void HandleUserCreated(string message)
	{
		using (var scope = _provider.CreateScope())
		{
			var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
			var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

			if (userEvent is null) return;

			var command = new CreateUserCommand()
			{
				Id = userEvent.Id,
				Name = userEvent.Name
			};

			_ = await mediator.Send(command);
		}
	}
	public async void HandleUserDeleted(string message)
	{
		using (var scope = _provider.CreateScope())
		{
			var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
			var userEvent = JsonSerializer.Deserialize<UserDeletedEvent>(message);

			if (userEvent is null) return;

			var command = new DeleteUserCommand()
			{
				Id = userEvent.Id,
			};

			_ = await mediator.Send(command);
		}
	}
	
	public override void Dispose()
	{
		_channel.Close();
		_channel.Dispose();
		_connection.Close();
		_connection.Dispose();
		base.Dispose();
	}
}