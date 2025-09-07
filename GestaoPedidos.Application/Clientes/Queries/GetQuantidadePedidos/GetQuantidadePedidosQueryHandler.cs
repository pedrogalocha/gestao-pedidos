using GestaoPedidos.Application.Interfaces.Repositories;
using MediatR;

namespace GestaoPedidos.Application.Clientes.Queries.GetQuantidadePedidos;

public class GetQuantidadePedidosQueryHandler : IRequestHandler<GetQuantidadePedidosQuery, int>
{
    private readonly IPedidoRepository _pedidoRepository;

    public GetQuantidadePedidosQueryHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<int> Handle(GetQuantidadePedidosQuery request, CancellationToken cancellationToken)
    {

        return await _pedidoRepository.GetQuantidadePedidosPorClienteAsync(request.ClienteId);
    }
}
