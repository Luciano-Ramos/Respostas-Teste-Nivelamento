using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Handlers
{
    public interface IConsultarSaldoContaCorrenteHandler
    {
        public Task<Result<SaldoContaCorrenteResponse>> Handle(ConsultarSaldoContaCorrenteCommand request, CancellationToken cancellationToken);
    }
}
