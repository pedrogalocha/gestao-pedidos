using System;

namespace GestaoPedidos.Application.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}
