using MediatR;
using GestaoPedidos.Application.Dtos;
using System.Collections.Generic;

namespace GestaoPedidos.Application.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesQuery : IRequest<IEnumerable<ClienteDto>>
    {
    }
}
