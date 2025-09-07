using GestaoPedidos.Application.Interfaces.Messaging;
using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Infrastructure;
using GestaoPedidos.Infrastructure.Data.Repositories;
using GestaoPedidos.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do DbContext com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetValue<string>("RabbitMQ:Uri"))
    };
    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(GestaoPedidos.Application.AssemblyReference).Assembly));

builder.Services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
