using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/saldocontacorrente")]
    public class SaldoContaCorrenteController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public SaldoContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{idContaCorrente}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ObterSaldoContaCorrente(string idContaCorrente)
        {
            var request = new ConsultarSaldoContaCorrenteCommand { IdContaCorrente = idContaCorrente };
            var response = await _mediator.Send(request);

            
            if (response != null && response.Data != null && response.IsSuccess == true)
            {
                var responseData = new
                {
                    NumeroConta = response.Data.NumeroContaCorrente,
                    Titular = response.Data.TitularContaCorrente,
                    DataHoraResposta = response.Data.DataHoraResposta,
                    Saldo = response.Data.SaldoAtual
                };
                return Ok(new { responseData });
            }
            else
            {
                return BadRequest(new { ErrorMessage = response.ErrorMessage, ErrorType = response.ErrorType.ToString() });
            }
        }
    }
}
