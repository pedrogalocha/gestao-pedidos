using GestaoPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Infrastructure.Data.Configurations;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItensPedido");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        builder.Property(i => i.Produto)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.Quantidade)
            .IsRequired();

        builder.Property(i => i.PrecoUnitario)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.HasOne<Pedido>()
               .WithMany(p => p.Itens)
               .HasForeignKey("PedidoId")
               .IsRequired();
    }
}
