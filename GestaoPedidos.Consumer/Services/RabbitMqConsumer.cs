using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPedidos.Consumer.Services
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly string _queueName = "pedidos";

        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var rabbitMqUri = _configuration.GetValue<string>("RabbitMQ:Uri");
            if (string.IsNullOrEmpty(rabbitMqUri))
            {
                _logger.LogError("--> URI do RabbitMQ não encontrada na configuração. O serviço não será iniciado.");
                return;
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri(rabbitMqUri),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            try
            {
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _channel = await _connection.CreateChannelAsync();
                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                _logger.LogInformation("--> QoS (Prefetch Count) definido para 1.");

                await _channel.QueueDeclareAsync(queue: _queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                _logger.LogInformation("--> Conexão com RabbitMQ estabelecida e canal criado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "--> Erro ao inicializar RabbitMQ. O serviço não será iniciado.");
                return;
            }

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel == null)
            {
                _logger.LogError("Canal RabbitMQ não está disponível. O worker não pode iniciar.");
                return;
            }

            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("--> Nova mensagem recebida: {Message}", message);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var pedidoDto = JsonSerializer.Deserialize<PedidoDto>(message, options);

                    if (pedidoDto != null)
                    {
                        pedidoDto.DataCriacao = DateTime.UtcNow;
                        pedidoDto.PrecoTotal = pedidoDto.Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
                        pedidoDto.Status = "Pendente";
                        await repository.ProcessarEAdicionarPedidoAsync(pedidoDto);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                        _logger.LogInformation("--> Mensagem processada e confirmada (ACK).");
                    }
                    else
                    {
                        _logger.LogWarning("--> Mensagem deserializada para nulo. Descartando (NACK).");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                    }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "--> Erro de deserialização. Mensagem descartada (NACK): {Message}", message);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "--> Erro inesperado ao processar mensagem. Reenfileirando (NACK)...");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is { IsOpen: true })
            {
                await _channel.CloseAsync();
            }
            if (_connection is { IsOpen: true })
            {
                await _connection.CloseAsync();
            }
            await base.StopAsync(cancellationToken);
        }
    }
}
