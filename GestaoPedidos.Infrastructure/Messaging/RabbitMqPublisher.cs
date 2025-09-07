using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GestaoPedidos.Application.Interfaces.Messaging;
using RabbitMQ.Client;

namespace GestaoPedidos.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IConnection _connection;

    public RabbitMqPublisher(IConnection connection)
    {
        _connection = connection;
    }
    public async Task Publish(object message, string queueName)
    {

        using (var channel = await _connection.CreateChannelAsync())
        {

            await channel.QueueDeclareAsync(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var jsonString = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonString);
            
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, mandatory: true, basicProperties: new BasicProperties { Persistent = true }, body: body);
        }
    }

}
