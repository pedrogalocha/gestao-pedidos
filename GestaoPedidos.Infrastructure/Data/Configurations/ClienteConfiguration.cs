using GestaoPedidos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Infrastructure.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
       public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
       {
              builder.ToTable("Clientes");

              builder.HasKey(c => c.Id);

              builder.Property(c => c.Nome)
                     .HasColumnName("Nome")
                     .HasMaxLength(100)
                     .IsRequired(false);

              builder.Property(c => c.DataCadastro)
                     .HasColumnName("DataCadastro")
                     .IsRequired()
                     .HasDefaultValueSql("NOW()");

              builder.HasMany(c => c.Pedidos)
                     .WithOne(p => p.Cliente)
                     .HasForeignKey(p => p.ClienteId);
       }

}
