using System;
using System.Collections.Generic;

namespace GestaoPedidos.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public decimal PrecoTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ItemPedido
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
