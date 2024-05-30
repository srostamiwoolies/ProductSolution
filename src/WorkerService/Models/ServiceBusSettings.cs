namespace WorkerService.Models;

public class ServiceBusSettings
{
    public string ConnectionString { get; set; }
    public string Topic { get; set; }
    public string Subscription { get; set; }
}
