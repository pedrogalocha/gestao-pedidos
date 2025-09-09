using System.Collections.Generic;

namespace GestaoPedidos.Application.Dtos
{
    public class PedidoDetalhadoDto
    {
        public int CodigoPedido { get; set; }
        public int QuantidadeItens { get; set; }
        public decimal ValorTotal { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
    }
}
