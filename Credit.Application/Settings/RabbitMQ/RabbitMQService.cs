using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Credit.Application.Settings.RabbitMQ;

public class RabbitMQService : IRabbitMQService, IDisposable
{
    private readonly RabbitMQSettings _rabbitMQSettings;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQService(IOptions<RabbitMQSettings> options)
    {
        _rabbitMQSettings = options.Value;
        CreateConnection();
        CreateChannel();
    }

    private void CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMQSettings.HostName,
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password,
            Port = _rabbitMQSettings.Port
        };
        _connection = factory.CreateConnection();
    }

    private void CreateChannel()
    {
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: _rabbitMQSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    private static readonly JsonSerializerSettings SerializerSettings =
        new() { TypeNameHandling = TypeNameHandling.Objects };

    public void BasicPublish(object message)
    {
        var json = JsonConvert.SerializeObject(message, SerializerSettings);

        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "",
            routingKey: _rabbitMQSettings.QueueName,
            basicProperties: null,
            body: body);
    }

    public IEnumerable<T> BasicGet<T>()
    {
        var messages = new List<T>();

        //get first 10 messages from the queue
        var count = 1;

        BasicGetResult result;
        do
        {
            // get message from the queue
            result = _channel.BasicGet(queue: _rabbitMQSettings.QueueName, autoAck: true);

            if (result == null) continue;
            var body = result.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            // Deserialize the JSON
            var message = JsonConvert.DeserializeObject<T>(json);

            if (message != null)
            {
                messages.Add(message);
                count++;
            }
        }
        while (result != null && count <= 10);

        return messages;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}