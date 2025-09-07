using GestaoPedidos.Application.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GestaoPedidos.Application.Interfaces.Repositories
{
    public interface IPedidoRepository
    {
        Task ProcessarEAdicionarPedidoAsync(PedidoDto pedidoDto);
        Task<decimal?> GetValorTotalPedidoAsync(int pedidoId);
        Task<int> GetQuantidadePedidosPorClienteAsync(int clienteId);
        Task<IEnumerable<GestaoPedidos.Domain.Entities.Pedido>> GetPedidosPorClienteAsync(int clienteId);
    }
}
