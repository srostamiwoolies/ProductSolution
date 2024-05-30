using MongoDB.Driver;
using Service1.Models;
using Service1.Repositories;
using Service1.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("ProductsDb");
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

var redisConfig = builder.Configuration.GetSection(nameof(RedisConfig)).Get<RedisConfig>();

var redisConnection = ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);

builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();
builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
