using MediatR;

namespace GestaoPedidos.Application.Clientes.Queries.GetQuantidadePedidos;


/// <summary>
/// Representa a consulta para obter a quantidade de pedidos de um cliente.
/// </summary>
/// <param name="ClienteId">O ID do cliente a ser consultado.</param>
/// <returns>Retorna um inteiro (int) com a contagem de pedidos.</returns>
public record GetQuantidadePedidosQuery(int ClienteId) : IRequest<int>;