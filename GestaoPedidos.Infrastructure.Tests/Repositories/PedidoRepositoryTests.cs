using Microsoft.EntityFrameworkCore;
using GestaoPedidos.Infrastructure;
using GestaoPedidos.Infrastructure.Data.Repositories;
using GestaoPedidos.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using GestaoPedidos.Application.Dtos;
using System.Collections.Generic;

namespace GestaoPedidos.Infrastructure.Tests.Repositories
{
    public class PedidoRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly PedidoRepository _repository;
        private readonly Mock<ILogger<PedidoRepository>> _loggerMock;

        public PedidoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _loggerMock = new Mock<ILogger<PedidoRepository>>();
            _repository = new PedidoRepository(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task ProcessarEAdicionarPedidoAsync_DeveAdicionarPedidoAoBanco()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                CodigoPedido = 1,
                ClienteId = 1,
                PrecoTotal = 100,
                Itens = new List<ItemPedidoDto> { new ItemPedidoDto { Produto = "Teste", Quantidade = 1, PrecoUnitario = 100 } }
            };

            // Act
            await _repository.ProcessarEAdicionarPedidoAsync(pedidoDto);
            var result = await _context.Pedidos.FindAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetValorTotalPedidoAsync_DeveRetornarValorCorreto()
        {
            // Arrange
            var pedido = new Pedido { Id = 1, ClienteId = 1, PrecoTotal = 150.50m };
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetValorTotalPedidoAsync(1);

            // Assert
            result.Should().Be(150.50m);
        }

        [Fact]
        public async Task GetQuantidadePedidosPorClienteAsync_DeveRetornarQuantidadeCorreta()
        {
            // Arrange
            _context.Pedidos.AddRange(
                new Pedido { ClienteId = 1, PrecoTotal = 100 },
                new Pedido { ClienteId = 1, PrecoTotal = 200 },
                new Pedido { ClienteId = 2, PrecoTotal = 300 }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetQuantidadePedidosPorClienteAsync(1);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public async Task GetPedidosPorClienteAsync_DeveRetornarPedidosCorretos()
        {
            // Arrange
            _context.Pedidos.AddRange(
                new Pedido { ClienteId = 1, PrecoTotal = 100 },
                new Pedido { ClienteId = 1, PrecoTotal = 200 },
                new Pedido { ClienteId = 2, PrecoTotal = 300 }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPedidosPorClienteAsync(1);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosPedidos()
        {
            // Arrange
            _context.Clientes.AddRange(
                new Cliente { Id = 1, Nome = "Cliente 1" },
                new Cliente { Id = 2, Nome = "Cliente 2" }
            );
            _context.Pedidos.AddRange(
                new Pedido { ClienteId = 1, PrecoTotal = 100 },
                new Pedido { ClienteId = 2, PrecoTotal = 200 }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
        }
    }
}
