using MediatR;
using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPedidos.Application.Pedidos.Queries.GetAllPedidos
{
    public class GetAllPedidosQueryHandler : IRequestHandler<GetAllPedidosQuery, IEnumerable<PedidoDetalhadoDto>>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetAllPedidosQueryHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<IEnumerable<PedidoDetalhadoDto>> Handle(GetAllPedidosQuery request, CancellationToken cancellationToken)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();

            return pedidos.Select(p => new PedidoDetalhadoDto
            {
                CodigoPedido = p.Id,
                QuantidadeItens = p.Itens.Sum(i => i.Quantidade),
                ValorTotal = p.PrecoTotal,
                NomeCliente = p.Cliente?.Nome ?? "Cliente não informado"
            });
        }
    }
}
