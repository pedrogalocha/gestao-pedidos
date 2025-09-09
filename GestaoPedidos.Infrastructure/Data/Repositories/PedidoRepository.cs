using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Application.Mappers;
using GestaoPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GestaoPedidos.Infrastructure.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PedidoRepository> _logger;

        public PedidoRepository(AppDbContext context, ILogger<PedidoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessarEAdicionarPedidoAsync(PedidoDto pedidoDto)
        {
            try
            {
                _logger.LogInformation("Processando e salvando pedido {PedidoId}", pedidoDto.CodigoPedido);

                var pedido = pedidoDto.ToEntity();

                if (pedido != null)
                {
                    _context.Pedidos.Add(pedido);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Pedido {PedidoId} salvo com sucesso no banco de dados.", pedido.Id);
                }
                else
                {
                    _logger.LogWarning("Mapeamento do PedidoDto para a entidade Pedido resultou em nulo.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar e salvar o pedido {PedidoId}", pedidoDto.CodigoPedido);
                throw;
            }
        }

        public async Task<decimal?> GetValorTotalPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            return pedido?.PrecoTotal;
        }

        public async Task<int> GetQuantidadePedidosPorClienteAsync(int clienteId)
        {
            return await _context.Pedidos.CountAsync(p => p.ClienteId == clienteId);
        }

        public async Task<IEnumerable<Pedido>> GetPedidosPorClienteAsync(int clienteId)
        {
            return await _context.Pedidos
                                 .Where(p => p.ClienteId == clienteId)
                                 .Include(p => p.Itens)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                                 .Include(p => p.Itens)
                                 .Include(p => p.Cliente)
                                 .ToListAsync();
        }
    }
}
