using GestaoPedidos.Domain.Entities;
using MediatR;
using System;

namespace GestaoPedidos.Application.Clientes.Commands.CadastrarCliente
{
    public class CadastrarClienteCommand : IRequest<int>
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
