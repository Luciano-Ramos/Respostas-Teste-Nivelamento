using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Repositories.ConsultaSaldo;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoContaCorrenteHandler : IRequestHandler<ConsultarSaldoContaCorrenteCommand, Result<SaldoContaCorrenteResponse>>
    {

        private readonly IConsultaSaldoRepository _contaCorrenteRepository;

        public ConsultarSaldoContaCorrenteHandler(IConsultaSaldoRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }
        public async Task<Result<SaldoContaCorrenteResponse>> Handle(ConsultarSaldoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Result<SaldoContaCorrenteResponse> saldoResponse = await _contaCorrenteRepository.ConsultarSaldoContaCorrenteAsync(request.IdContaCorrente);

                if (saldoResponse.Data != null)
                {
                    return Result<SaldoContaCorrenteResponse>.Success(saldoResponse.Data);
                }
                else
                {
                    return Result<SaldoContaCorrenteResponse>.Failure(saldoResponse.ErrorMessage, (TipoFalha)saldoResponse.ErrorType);
                }                
            }
            catch (InvalidOperationException ex)
            {                
                return Result<SaldoContaCorrenteResponse>.Failure(ex.Message, TipoFalha.INVALID_OPERATION);
            }
        }

      
    }
}
