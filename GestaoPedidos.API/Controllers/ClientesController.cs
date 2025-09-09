using GestaoPedidos.Application.Clientes.Commands.CadastrarCliente;
using GestaoPedidos.Application.Clientes.Queries.GetAllClientes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestaoPedidos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllClientesQuery();
            var clientes = await _mediator.Send(query);
            return Ok(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CadastrarClienteCommand command)
        {
            var clienteId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = clienteId }, command);
        }
    }
}
