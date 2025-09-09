using GestaoPedidos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoPedidos.Application.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        Task AdicionarAsync(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<IEnumerable<Cliente>> ObterTodosAsync();
    }
}
