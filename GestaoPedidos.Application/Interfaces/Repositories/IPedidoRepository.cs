using GestaoPedidos.Application.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using GestaoPedidos.Domain.Entities;

namespace GestaoPedidos.Application.Interfaces.Repositories
{
    public interface IPedidoRepository
    {
        Task ProcessarEAdicionarPedidoAsync(PedidoDto pedidoDto);
        Task<decimal?> GetValorTotalPedidoAsync(int pedidoId);
        Task<int> GetQuantidadePedidosPorClienteAsync(int clienteId);
        Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(int clienteId);
        Task<IEnumerable<Pedido>> GetAllAsync();
    }
}
