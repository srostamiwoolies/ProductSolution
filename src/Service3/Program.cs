using Service3.Models;
using Service3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

var serviceBusSettings = builder.Configuration.GetSection("ServiceBusSettings").Get<ServiceBusSettings>();

builder.Services.AddSingleton(serviceBusSettings);
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddHttpClient("Service2", client =>
{
    var url = builder.Configuration["Service2Settings:Url"];
    client.BaseAddress = new Uri(url);
}).AddStandardResilienceHandler(options => {

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
