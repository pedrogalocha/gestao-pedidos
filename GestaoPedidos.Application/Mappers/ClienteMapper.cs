using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GestaoPedidos.Application.Mappers
{
    public static class ClienteMapper
    {
        public static ClienteDto? ToDto(this Cliente cliente)
        {
            if (cliente == null)
            {
                return null;
            }

            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome ?? string.Empty,
                DataCadastro = cliente.DataCadastro
            };
        }

        public static IEnumerable<ClienteDto> ToDto(this IEnumerable<Cliente> clientes)
        {
            return clientes.Select(c => c.ToDto()!);
        }
    }
}
