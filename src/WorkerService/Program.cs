using MongoDB.Driver;
using WorkerService.Repositories;
using WorkerService;
using WorkerService.Models;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var serviceBusSettings = builder.Configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();

builder.Services.AddSingleton(serviceBusSettings);

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("ProductsLogsDb");
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();

var host = builder.Build();
host.Run();
