using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.Repositories.Movimentacao;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoContaCorrenteCommandHandler : IRequestHandler<MovimentacaoContaCorrenteCommand, Result<string>>
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public MovimentacaoContaCorrenteCommandHandler(IMovimentacaoRepository movimentacaoRepository)
        {
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<Result<string>> Handle(MovimentacaoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isProcess = await _movimentacaoRepository.ValidaIdempotencia(request.MovimentacaoDto);

                Result<string> idMovimento = null;

                if (string.IsNullOrEmpty(isProcess))
                    idMovimento = await _movimentacaoRepository.AdicionarMovimentacaoAsync(request.MovimentacaoDto);



                if (idMovimento == null && isProcess != null)
                {
                    return Result<string>.Failure("Essa transação já foi realizada: " + isProcess, TipoFalha.ERROR_ADDING_MOVEMENT);
                }

                if (!string.IsNullOrEmpty(idMovimento.Data))
                {
                    _movimentacaoRepository.AdicionarIdempotencia(request.MovimentacaoDto, idMovimento.Data);
                    return Result<string>.Success(idMovimento.Data);
                }
                else
                {
                    return Result<string>.Failure(idMovimento.ErrorMessage, (TipoFalha)idMovimento.ErrorType);
                }

            }
            catch (InvalidOperationException ex)
            {
                return Result<string>.Failure(ex.Message, TipoFalha.ERROR_ADDING_MOVEMENT);
            }
        }
    }
}
