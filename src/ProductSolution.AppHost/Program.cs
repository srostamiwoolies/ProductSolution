var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("Redis");

var isPublished = builder.ExecutionContext.IsPublishMode;

var serviceBus = isPublished
    ? builder.AddAzureServiceBus("Messaging")
    : builder.AddConnectionString("Messaging");

var mongo = builder.AddMongoDB("mongo");
var mongodb = mongo.AddDatabase("mongodb");

builder.AddProject<Projects.Service1>("service1")
    .WithReference(mongodb)
    .WithReference(redis);

builder.AddProject<Projects.Service2>("service2");

builder.AddProject<Projects.Service3>("service3")
    .WithReference(serviceBus);

builder.AddProject<Projects.WorkerService>("workerservice");

builder.Build().Run();
