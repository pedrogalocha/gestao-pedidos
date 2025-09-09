using FluentAssertions;
using GestaoPedidos.Domain.Entities;

namespace GestaoPedidos.Domain.Tests.Entities
{
    public class ClienteTests
    {
        [Fact]
        public void Construtor_DeveCriarCliente_QuandoDadosValidos()
        {
            // Arrange
            var nome = "Pedro";

            // Act
            var cliente = new Cliente{ Nome = nome };

            // Assert
            cliente.Should().NotBeNull();
            cliente.Nome.Should().Be(nome);
        }
    }
}
