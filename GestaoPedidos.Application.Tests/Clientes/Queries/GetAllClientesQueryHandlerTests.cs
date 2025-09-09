using Moq;
using FluentAssertions;
using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Application.Clientes.Queries.GetAllClientes;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Application.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestaoPedidos.Application.Tests.Clientes.Queries
{
    public class GetAllClientesQueryHandlerTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly GetAllClientesQueryHandler _handler;

        public GetAllClientesQueryHandlerTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _handler = new GetAllClientesQueryHandler(_clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaDeClientes()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Pedro" },
                new Cliente { Id = 2, Nome = "Joao" }
            };
            _clienteRepositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(clientes);
            var query = new GetAllClientesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            _clienteRepositoryMock.Verify(repo => repo.ObterTodosAsync(), Times.Once);
        }
    }
}
