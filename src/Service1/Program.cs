using MongoDB.Driver;
using Service1.Repositories;
using Service1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
//var mongoClient = new MongoClient(mongoConnectionString);
//var mongoDatabase = mongoClient.GetDatabase("ProductsDb");
//builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

builder.AddMongoDBClient("MongoDB");

builder.AddRedisClient("Redis");

builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();
builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
