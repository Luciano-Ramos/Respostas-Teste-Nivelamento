using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/movimentacao")]
    public class MovimentacaoContaCorrenteController : ControllerBase
    {

        private readonly IMediator _mediator;

        public MovimentacaoContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarMovimento([FromBody] MovimentacaoContaCorrenteCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(new { IdMovimento = result.Data });
            }

            return BadRequest(new { ErrorMessage = result.ErrorMessage, ErrorType = result.ErrorType.ToString() });

        }

    }
}
