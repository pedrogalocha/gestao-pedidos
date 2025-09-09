using FluentAssertions;
using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Mappers;
using GestaoPedidos.Domain.Entities;
using System.Collections.Generic;
using Xunit;

namespace GestaoPedidos.Application.Tests.Mappers
{
    public class PedidoMapperTests
    {
        [Fact]
        public void ToDto_DeveMapearPedidoParaPedidoDto()
        {
            // Arrange
            var pedido = new Pedido
            {
                ClienteId = 1,
                Itens = new List<ItemPedido> { new ItemPedido { Id = 1, Produto = "Produto A", Quantidade = 10, PrecoUnitario = 15.50m } },
                PrecoTotal = 155.0m,
                Status = "Em Processamento",
                DataCriacao = System.DateTime.Now
            };

            // Act
            var pedidoDto = PedidoMapper.ToDto(pedido);

            // Assert
            pedidoDto.Should().NotBeNull();
            pedidoDto.Should().BeOfType<PedidoDto>();
            pedidoDto.ClienteId.Should().Be(pedido.ClienteId);
            pedidoDto.PrecoTotal.Should().Be(pedido.PrecoTotal);
            pedidoDto.Status.Should().Be(pedido.Status);
            pedidoDto.DataCriacao.Should().Be(pedido.DataCriacao);
            pedidoDto.Itens.Should().HaveCount(1);
            pedidoDto.Itens[0].Produto.Should().Be("Produto A");
        }

        [Fact]
        public void ToEntity_DeveMapearPedidoDtoParaPedido()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                ClienteId = 1,
                Itens = new List<ItemPedidoDto> { new ItemPedidoDto { Id = 1, Produto = "Produto A", Quantidade = 10, PrecoUnitario = 15.50m } },
                PrecoTotal = 155.0m,
                Status = "Em Processamento",
                DataCriacao = System.DateTime.Now
            };

            // Act
            var pedido = PedidoMapper.ToEntity(pedidoDto);

            // Assert
            pedido.Should().NotBeNull();
            pedido.Should().BeOfType<Pedido>();
            pedido.ClienteId.Should().Be(pedidoDto.ClienteId);
            pedido.PrecoTotal.Should().Be(pedidoDto.PrecoTotal);
            pedido.Status.Should().Be(pedidoDto.Status);
            pedido.DataCriacao.Should().Be(pedidoDto.DataCriacao);
            pedido.Itens.Should().HaveCount(1);
            pedido.Itens[0].Produto.Should().Be("Produto A");
        }
    }
}
