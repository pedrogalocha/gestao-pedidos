using MediatR;
using GestaoPedidos.Application.Dtos;
using System.Collections.Generic;

namespace GestaoPedidos.Application.Pedidos.Queries.GetAllPedidos
{
    public class GetAllPedidosQuery : IRequest<IEnumerable<PedidoDetalhadoDto>>
    {
    }
}
