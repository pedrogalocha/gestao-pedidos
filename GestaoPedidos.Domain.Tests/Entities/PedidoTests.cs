using FluentAssertions;
using GestaoPedidos.Domain.Entities;
using System.Collections.Generic;

namespace GestaoPedidos.Domain.Tests.Entities
{
    public class PedidoTests
    {
        [Fact]
        public void Construtor_DeveCriarPedido_QuandoDadosValidos()
        {
            // Arrange
            var clienteId = 1;
            var itens = new List<ItemPedido>
            {
                new ItemPedido { Produto = "Produto Teste", Quantidade = 1, PrecoUnitario = 10.0m }
            };
            var precoTotal = 10.0m;
            var status = "Novo";

            // Act
            var pedido = new Pedido
            {
                ClienteId = clienteId,
                Itens = itens,
                PrecoTotal = precoTotal,
                Status = status,
                DataCriacao = System.DateTime.Now
            };

            // Assert
            pedido.Should().NotBeNull();
            pedido.ClienteId.Should().Be(clienteId);
            pedido.Itens.Should().BeEquivalentTo(itens);
            pedido.PrecoTotal.Should().Be(precoTotal);
            pedido.Status.Should().Be(status);
        }
    }
}
