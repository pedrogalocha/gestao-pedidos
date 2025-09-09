using Microsoft.EntityFrameworkCore;
using GestaoPedidos.Infrastructure;
using GestaoPedidos.Infrastructure.Data.Repositories;
using GestaoPedidos.Domain.Entities;
using FluentAssertions;

namespace GestaoPedidos.Infrastructure.Tests.Repositories
{
    public class ClienteRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ClienteRepository _repository;

        public ClienteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repository = new ClienteRepository(_context);
        }

        [Fact]
        public async Task Obter_DeveRetornarCliente_QuandoClienteExiste()
        {
            // Arrange
            var cliente = new Cliente { Nome = "Cliente Teste" };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ObterPorIdAsync(cliente.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(cliente.Id);
        }

        [Fact]
        public async Task Cadastrar_DeveAdicionarClienteAoBanco()
        {
            // Arrange
            var cliente = new Cliente { Nome = "Novo Cliente" };

            // Act
            await _repository.AdicionarAsync(cliente);
            var result = await _context.Clientes.FindAsync(cliente.Id);

            // Assert
            result.Should().NotBeNull();
            result.Nome.Should().Be("Novo Cliente");
        }
    }
}
