using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoPedidos.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public DateTime DataCadastro { get; set; }
    public ICollection<Pedido>? Pedidos { get; set; }
}
