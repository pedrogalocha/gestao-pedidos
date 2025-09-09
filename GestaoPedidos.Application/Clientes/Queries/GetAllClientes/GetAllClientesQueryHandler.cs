using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Application.Mappers;
using GestaoPedidos.Application.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPedidos.Application.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesQueryHandler : IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteDto>>
    {
        private readonly IClienteRepository _clienteRepository;

        public GetAllClientesQueryHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteDto>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _clienteRepository.ObterTodosAsync();
            return clientes.ToDto();
        }
    }
}
