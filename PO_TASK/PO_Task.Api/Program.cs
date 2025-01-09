using Microsoft.Extensions.Configuration;
using PO_Task.Api.Extensions;
using PO_Task.Application;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Infrastructure;
using PO_Task.Infrastructure.RabbitMQ;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (
        context,
        loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();
app.UseCustomExceptionHandler();

var rabbitMQConfig = app.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

var rabbitMQConsumer = app.Services.GetRequiredService<RabbitMQConsumer>();
await Task.Run(() => rabbitMQConsumer.StartListening(rabbitMQConfig));

app.UseAuthorization();

app.MapControllers();

app.Run();
