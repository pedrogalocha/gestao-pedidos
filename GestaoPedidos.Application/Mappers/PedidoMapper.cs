using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Domain.Entities;
using System.Linq;

namespace GestaoPedidos.Application.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido ToEntity(this PedidoDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new Pedido
            {
                Id = dto.CodigoPedido,
                ClienteId = dto.ClienteId,
                PrecoTotal = dto.PrecoTotal,
                Status = dto.Status ?? string.Empty,
                DataCriacao = dto.DataCriacao,
                Itens = dto.Itens?.Select(itemDto => itemDto.ToEntity()).Where(i => i != null).ToList() as List<ItemPedido> ?? new List<ItemPedido>()
            };
        }

        public static ItemPedido? ToEntity(this ItemPedidoDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new ItemPedido
            {
                Id = dto.Id,
                Produto = dto.Produto,
                Quantidade = dto.Quantidade,
                PrecoUnitario = dto.PrecoUnitario
            };
        }

        public static PedidoDto? ToDto(this Pedido? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new PedidoDto
            {
                CodigoPedido = entity.Id,
                ClienteId = entity.ClienteId,
                PrecoTotal = entity.PrecoTotal,
                Status = entity.Status,
                DataCriacao = entity.DataCriacao,
                Itens = entity.Itens?.Select(item => item.ToDto()).Where(i => i != null).ToList() as List<ItemPedidoDto> ?? new List<ItemPedidoDto>()
            };
        }

        public static ItemPedidoDto? ToDto(this ItemPedido? entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ItemPedidoDto
            {
                Id = entity.Id,
                Produto = entity.Produto,
                Quantidade = entity.Quantidade,
                PrecoUnitario = entity.PrecoUnitario
            };
        }
    }
}
