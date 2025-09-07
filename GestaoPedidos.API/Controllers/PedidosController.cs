using GestaoPedidos.Application.Clientes.Queries.GetPedidosPorCliente;
using GestaoPedidos.Application.Clientes.Queries.GetQuantidadePedidos;
using GestaoPedidos.Application.Dtos;
using GestaoPedidos.Application.Interfaces.Messaging;
using GestaoPedidos.Application.Interfaces.Repositories;
using GestaoPedidos.Application.Mappers;
using GestaoPedidos.Application.Pedidos.Commands.EnfileirarPedido;
using GestaoPedidos.Application.Pedidos.Queries.GetValorTotal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoPedidos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly IMediator _mediator;

        public PedidosController(ILogger<PedidosController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpPost]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostNovoPedido([FromBody] PedidoDto pedidoDto)
        {
            if (pedidoDto == null || !pedidoDto.Itens.Any())
            {
                return BadRequest("O pedido não pode ser nulo ou não conter itens.");
            }

            try
            {
                var command = new EnfileirarPedidoCommand(pedidoDto);
                await _mediator.Send(command);

                _logger.LogInformation("Comando para enfileirar pedido {CodigoPedido} enviado com sucesso.", pedidoDto.CodigoPedido);

                return Accepted(new { message = "Pedido recebido e enfileirado para processamento." }); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na API ao tentar enfileirar o pedido {CodigoPedido}.", pedidoDto.CodigoPedido);
                return StatusCode(500, "Erro ao enfileirar o pedido para processamento.");
            }
        }

        [HttpGet("{pedidoId}/valor-total")]
        public async Task<IActionResult> GetValorTotalPedido(int pedidoId)
        {
            _logger.LogInformation("Buscando valor total para o pedido ID: {PedidoId}", pedidoId);

            var query = new GetValorTotalPedidoQuery(pedidoId);
            var valorTotal = await _mediator.Send(query);

            if (valorTotal == null)
            {
                _logger.LogWarning("Pedido ID: {PedidoId} não encontrado.", pedidoId);
                return NotFound($"Pedido com ID {pedidoId} não encontrado.");
            }

            return Ok(new { pedidoId, valorTotal });
        }

        [HttpGet("cliente/{clienteId}/quantidade")]
        public async Task<IActionResult> GetQuantidadePedidosPorCliente(int clienteId)
        {
            _logger.LogInformation("Buscando quantidade de pedidos para o cliente ID: {ClienteId}", clienteId);
            var query = new GetQuantidadePedidosQuery(clienteId);

            var result = await _mediator.Send(query);

            return Ok(new { clienteId = clienteId, quantidadePedidos = result });
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> GetPedidosPorCliente(int clienteId)
        {
            _logger.LogInformation("Buscando pedidos para o cliente ID: {ClienteId}", clienteId);

            var query = new GetPedidosPorClienteQuery(clienteId);
            var pedidosDto = await _mediator.Send(query);

            if (pedidosDto == null || !pedidosDto.Any())
            {
                return NotFound($"Nenhum pedido encontrado para o cliente com ID {clienteId}.");
            }

            return Ok(pedidosDto);
        }
    }
}
