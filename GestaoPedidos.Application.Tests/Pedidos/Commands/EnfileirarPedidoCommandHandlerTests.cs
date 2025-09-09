using Moq;
using FluentAssertions;
using GestaoPedidos.Application.Pedidos.Commands.EnfileirarPedido;
using GestaoPedidos.Application.Interfaces.Messaging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestaoPedidos.Application.Dtos;
using Xunit;
using Microsoft.Extensions.Logging;

namespace GestaoPedidos.Application.Tests.Pedidos.Commands
{
    public class EnfileirarPedidoCommandHandlerTests
    {
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly Mock<ILogger<EnfileirarPedidoCommandHandler>> _loggerMock;
        private readonly EnfileirarPedidoCommandHandler _handler;

        public EnfileirarPedidoCommandHandlerTests()
        {
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _loggerMock = new Mock<ILogger<EnfileirarPedidoCommandHandler>>();
            _handler = new EnfileirarPedidoCommandHandler(_messagePublisherMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_DevePublicarMensagem_QuandoComandoValido()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                ClienteId = 1,
                Itens = new List<ItemPedidoDto> { new ItemPedidoDto { Produto = "Produto Teste", Quantidade = 1, PrecoUnitario = 10 } },
                PrecoTotal = 10
            };
            var command = new EnfileirarPedidoCommand(pedidoDto);

            _messagePublisherMock.Setup(m => m.Publish(It.IsAny<object>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _messagePublisherMock.Verify(m => m.Publish(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }
    }
}
