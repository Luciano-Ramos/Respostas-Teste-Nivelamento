using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.Repositories.Movimentacao
{
    public interface IMovimentacaoRepository
    {
        Task<Result<string>> AdicionarMovimentacaoAsync(MovimentacaoContaCorrente request);
        Task<string> ValidaIdempotencia(MovimentacaoContaCorrente request);
        void AdicionarIdempotencia(MovimentacaoContaCorrente requisicao,string idMovimento);
    }
}
