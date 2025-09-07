using GestaoPedidos.Application.Interfaces.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoPedidos.Application.Pedidos.Commands.EnfileirarPedido;

public class EnfileirarPedidoCommandHandler : IRequestHandler<EnfileirarPedidoCommand>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<EnfileirarPedidoCommandHandler> _logger;

    public EnfileirarPedidoCommandHandler(IMessagePublisher messagePublisher, ILogger<EnfileirarPedidoCommandHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public Task Handle(EnfileirarPedidoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handler recebendo o comando para enfileirar o pedido {CodigoPedido}.", request.Pedido.CodigoPedido);

            _messagePublisher.Publish(request.Pedido, "pedidos");

            _logger.LogInformation("Pedido {CodigoPedido} passado para o publisher com sucesso.", request.Pedido.CodigoPedido);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao tentar publicar o pedido {CodigoPedido} na fila.", request.Pedido.CodigoPedido);
            throw;
        }
    }
}
