using Azure.Messaging.ServiceBus;
using WorkerService.Repositories;
using System.Text.Json;
using WorkerService.Models;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ServiceBusSettings _serviceBusSettings;
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly IProductRepository _productRepository;

    public Worker(ILogger<Worker> logger, ServiceBusSettings serviceBusSettings, IProductRepository productRepository)
    {
        _logger = logger;
        _serviceBusSettings = serviceBusSettings;
        _productRepository = productRepository;
        _client = new ServiceBusClient(serviceBusSettings.ConnectionString);
        _processor = _client.CreateProcessor(serviceBusSettings.Topic, serviceBusSettings.Subscription, new ServiceBusProcessorOptions());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await HandleMessages();
        while (!stoppingToken.IsCancellationRequested)
        {
        }
    }

    private async Task HandleMessages()
    {
        try
        {
            _processor.ProcessMessageAsync += MessageHandler;

            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync();
        }
        catch { }
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();

        var product = JsonSerializer.Deserialize<Product>(body);

        await _productRepository.CreateAsync(product);

        Console.WriteLine($"Message handled: {body}");

        await args.CompleteMessageAsync(args.Message);
    }

    // handle any errors when receiving messages
    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}
