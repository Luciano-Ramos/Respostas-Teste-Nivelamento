using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.Repositories.ConsultaSaldo
{
    public interface IConsultaSaldoRepository
    {
        Task<Result<SaldoContaCorrenteResponse>> ConsultarSaldoContaCorrenteAsync(string idContaCorrente);
    }
}
