using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GestaoPedidos.API.Controllers;
using GestaoPedidos.Application.Clientes.Queries.GetAllClientes;
using GestaoPedidos.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GestaoPedidos.API.Tests.Controllers
{
    public class ClientesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ClientesController _controller;

        public ClientesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ClientesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOkComListaDeClientes()
        {
            // Arrange
            var clientesDto = new List<ClienteDto>
            {
                new ClienteDto { Id = 1, Nome = "Cliente Teste 1" },
                new ClienteDto { Id = 2, Nome = "Cliente Teste 2" }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllClientesQuery>(), default)).ReturnsAsync(clientesDto);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedClientes = okResult.Value.Should().BeAssignableTo<IEnumerable<ClienteDto>>().Subject;
            returnedClientes.Should().HaveCount(2);
        }
    }
}
