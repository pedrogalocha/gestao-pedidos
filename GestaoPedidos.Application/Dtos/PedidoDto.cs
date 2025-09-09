namespace GestaoPedidos.Application.Dtos;

public class PedidoDto
{
    public int ClienteId { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = new();
    public decimal PrecoTotal { get; set; }
    public string? Status { get; set; }
    public DateTime DataCriacao { get; set; }
    public int CodigoPedido { get; set; }
}

public class ItemPedidoDto
{
    public int Id { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
