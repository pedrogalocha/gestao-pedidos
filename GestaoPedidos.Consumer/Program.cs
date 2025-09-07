using GestaoPedidos.Application.Interfaces;
using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Consumer.Services;
using GestaoPedidos.Infrastructure;
using GestaoPedidos.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();
host.Run();