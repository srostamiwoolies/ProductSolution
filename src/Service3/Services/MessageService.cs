using Azure.Messaging.ServiceBus;
using Service3.Models;

namespace Service3.Services;

public class MessageService : IMessageService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ServiceBusSettings serviceBusSettings, ILogger<MessageService> logger)
    {
        _client = new ServiceBusClient(serviceBusSettings.ConnectionString);
        _sender = _client.CreateSender(serviceBusSettings.Topic);
        _logger = logger;
    }

    public async Task Send(string message)
    {
        using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();
        messageBatch.TryAddMessage(new ServiceBusMessage(message));

        try
        {
            await _sender.SendMessagesAsync(messageBatch);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while sending a message to service bus. {0}", ex.Message);
        }
    }
}