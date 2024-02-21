using MediatR;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoContaCorrenteCommand : IRequest<Result<string>>
    {
        public MovimentacaoContaCorrente MovimentacaoDto { get; }

        public MovimentacaoContaCorrenteCommand(MovimentacaoContaCorrente movimentacaoDto)
        {
            MovimentacaoDto = movimentacaoDto;
        }        
    }
}
