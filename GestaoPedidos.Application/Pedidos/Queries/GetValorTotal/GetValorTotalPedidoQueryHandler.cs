using GestaoPedidos.Application.Interfaces.Repositories;
using MediatR;

namespace GestaoPedidos.Application.Pedidos.Queries.GetValorTotal;

    public class GetValorTotalPedidoQueryHandler : IRequestHandler<GetValorTotalPedidoQuery, decimal?>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetValorTotalPedidoQueryHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<decimal?> Handle(GetValorTotalPedidoQuery request, CancellationToken cancellationToken)
        {
            return await _pedidoRepository.GetValorTotalPedidoAsync(request.PedidoId);
        }
    }