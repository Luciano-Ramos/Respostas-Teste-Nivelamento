using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class ConsultarSaldoContaCorrenteCommand : IRequest<Result<SaldoContaCorrenteResponse>>
    {
        public string? IdContaCorrente { get; set; }
    }
}
