using MediatR;
using Questao5.Application.Commands.Requests;

namespace Questao5.Application.Handlers
{
    public interface IMovimentacaoContaCorrenteCommandHandler
    {
        Task<Unit> Handle(MovimentacaoContaCorrenteCommand request, CancellationToken cancellationToken);
    }
}