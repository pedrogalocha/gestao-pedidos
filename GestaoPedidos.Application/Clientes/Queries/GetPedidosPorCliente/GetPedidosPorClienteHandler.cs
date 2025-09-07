using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Application.Mappers;
using MediatR;

namespace GestaoPedidos.Application.Clientes.Queries.GetPedidosPorCliente;

public class GetPedidosPorClienteHandler : IRequestHandler<GetPedidosPorClienteQuery, List<PedidoDto>>
{
    private readonly IPedidoRepository _pedidoRepository;

    public GetPedidosPorClienteHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<List<PedidoDto>> Handle(GetPedidosPorClienteQuery request, CancellationToken cancellationToken)
    {
        var pedidos = await _pedidoRepository.GetPedidosPorClienteAsync(request.ClienteId);

        if (pedidos == null || !pedidos.Any())
        {
            return new List<PedidoDto>();
        }

        return pedidos.Select(p => PedidoMapper.ToDto(p)).ToList();
    }
}