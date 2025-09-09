using GestaoPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Infrastructure.Data.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PrecoTotal)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Recebido");

        builder.Property(p => p.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.HasOne(p => p.Cliente)
               .WithMany(c => c.Pedidos)
               .HasForeignKey(p => p.ClienteId)
               .IsRequired();


        builder.HasMany(p => p.Itens)
               .WithOne()
               .HasForeignKey("PedidoId");
    }

}
