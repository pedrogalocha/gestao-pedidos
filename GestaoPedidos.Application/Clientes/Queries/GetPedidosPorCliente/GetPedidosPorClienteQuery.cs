using GestaoPedidos.Application.Dtos;
using MediatR;

namespace GestaoPedidos.Application.Clientes.Queries.GetPedidosPorCliente;

public record GetPedidosPorClienteQuery(int ClienteId) : IRequest<List<PedidoDto>>;