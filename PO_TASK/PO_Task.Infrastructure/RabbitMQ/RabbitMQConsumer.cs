using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PO_Task.Domain.BuildingBlocks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SHO_Task.Application.IntegrationEvents;
using System.Text;
using System.Text.Json;

namespace PO_Task.Infrastructure.RabbitMQ;
public class RabbitMQConsumer
{
    private readonly string _queueName = "PurchaseOrderQueue";
    
    private readonly ILogger<RabbitMQConsumer> _logger;

    public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger)
    {

        _logger = logger;
    }

    public async void StartListening(RabbitMQConfig _rabbitMQConfig)
    {
        var _factory = new ConnectionFactory { HostName = "host.docker.internal", UserName = _rabbitMQConfig.Username, Password = _rabbitMQConfig.Password, Port = _rabbitMQConfig.Port };
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

       await channel.QueueDeclareAsync(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<ShippingOrderCreatedIntegrationEvent>(jsonString);

                if (@event != null)
                {

                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Listening to queue {_queueName} Error : \n{ ex.Message }...");
                // handle error, possibly send to DLQ
            }
            finally
            {
                // 6. Ack
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
        };

        await channel.BasicConsumeAsync(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);

        _logger.LogInformation($"Listening to queue {_queueName}...");
    }
}

