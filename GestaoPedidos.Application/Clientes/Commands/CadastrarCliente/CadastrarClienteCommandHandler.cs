using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPedidos.Application.Clientes.Commands.CadastrarCliente
{
    public class CadastrarClienteCommandHandler : IRequestHandler<CadastrarClienteCommand, int>
    {
        private readonly IClienteRepository _clienteRepository;

        public CadastrarClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<int> Handle(CadastrarClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = new Cliente
            {
                Nome = request.Nome,
                DataCadastro = request.DataCadastro
            };

            await _clienteRepository.AdicionarAsync(cliente);
            return cliente.Id;
        }
    }
}
