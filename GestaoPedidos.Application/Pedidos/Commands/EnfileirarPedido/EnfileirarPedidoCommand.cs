using GestaoPedidos.Application.Dtos;
using MediatR;

namespace GestaoPedidos.Application.Pedidos.Commands.EnfileirarPedido;

/// <summary>
/// Representa o comando para enfileirar um novo pedido para processamento.
/// </summary>
/// <param name="Pedido">O DTO do pedido recebido pela API.</param>
public record EnfileirarPedidoCommand(PedidoDto Pedido) : IRequest;