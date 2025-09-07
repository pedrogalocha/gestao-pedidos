using System.Reflection;
using GestaoPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Cliente> Clientes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
         
        base.OnModelCreating(modelBuilder);
    }
}