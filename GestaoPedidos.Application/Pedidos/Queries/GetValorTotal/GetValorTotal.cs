using MediatR;

namespace GestaoPedidos.Application.Pedidos.Queries.GetValorTotal;

public record GetValorTotalPedidoQuery(int PedidoId) : IRequest<decimal?>;