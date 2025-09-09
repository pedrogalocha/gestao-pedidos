using FluentAssertions;
using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Mappers;
using GestaoPedidos.Domain.Entities;
using Xunit;

namespace GestaoPedidos.Application.Tests.Mappers
{
    public class ClienteMapperTests
    {
        [Fact]
        public void ToDto_DeveMapearClienteParaClienteDto()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Pedro", DataCadastro = System.DateTime.Now };

            // Act
            var clienteDto = ClienteMapper.ToDto(cliente);

            // Assert
            clienteDto.Should().NotBeNull();
            clienteDto.Should().BeOfType<ClienteDto>();
            clienteDto.Id.Should().Be(cliente.Id);
            clienteDto.Nome.Should().Be(cliente.Nome);
            clienteDto.DataCadastro.Should().Be(cliente.DataCadastro);
        }
    }
}
