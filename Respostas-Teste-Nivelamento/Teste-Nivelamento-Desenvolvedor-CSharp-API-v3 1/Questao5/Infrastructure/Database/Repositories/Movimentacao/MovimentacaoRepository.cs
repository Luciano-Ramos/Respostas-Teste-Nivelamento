using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.Repositories.Movimentacao
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {

        private readonly IDatabaseBootstrap _databaseBootstrap;

        public MovimentacaoRepository(IDatabaseBootstrap databaseBootstrap)
        {
            _databaseBootstrap = databaseBootstrap;
        }
        public async Task<Result<string>> AdicionarMovimentacaoAsync(MovimentacaoContaCorrente request)
        {
            try
            {
                using IDbConnection connection = _databaseBootstrap.GetConnection();
                

                bool existeNoEnum = false;
                if (request.TipoMovimento == "C" || request.TipoMovimento == "D")
                    existeNoEnum = Enum.IsDefined(typeof(TipoMovimentoEnum), request.TipoMovimento == "C" ? 0 : 1);

                string TipoMovimento = string.Empty;
                if (existeNoEnum)
                    TipoMovimento = request.TipoMovimento == "C" ? "C" : "D";



                string query = @"
                    SELECT idcontacorrente AS IdContaCorrente,
                           numero,
                           nome,
                           ativo
                    FROM ContaCorrente
                    WHERE idcontacorrente = @IdContaCorrente";

                ContaCorrente objContaCorrente = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { request.IdContaCorrente });




                if (objContaCorrente == null || string.IsNullOrEmpty(objContaCorrente.IdContaCorrente))
                    return Result<string>.Failure("Apenas contas correntes cadastradas podem receber movimentação", TipoFalha.INVALID_ACCOUNT);

                if (objContaCorrente.Ativo == false)
                    return Result<string>.Failure("Apenas contas correntes ativas podem receber movimentação", TipoFalha.INACTIVE_ACCOUNT);

                if (request.Valor <= 0)
                    return Result<string>.Failure("Apenas valores positivos podem ser recebidos", TipoFalha.INVALID_VALUE);

                if (TipoMovimento != "C" && TipoMovimento != "D")
                    return Result<string>.Failure("Apenas os tipos 'débito' ou 'crédito' podem ser aceitos", TipoFalha.INVALID_TYPE);


                var IdMovimentoNovo = Guid.NewGuid();

                var idMovimento = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO MOVIMENTO (idmovimento,idcontacorrente, datamovimento, tipomovimento, valor) " +
                    "VALUES (@IdMovimento,@IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor); SELECT last_insert_rowid();",
                    new
                    {
                        IdMovimento = IdMovimentoNovo,
                        IdContaCorrente = request.IdContaCorrente,
                        DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                        TipoMovimento = TipoMovimento,
                        Valor = request.Valor
                    });

                
                string query2 = @"
                    SELECT *
                    FROM MOVIMENTO 
                    WHERE idMovimento = @IdMovimento AND idcontacorrente = @IdContaCorrente AND tipomovimento = @TipoMovimento AND valor = @Valor ";
                dynamic objMovimento = await connection.QueryFirstOrDefaultAsync<dynamic>(query2,
                    new
                    {
                        IdMovimento = IdMovimentoNovo,
                        IdContaCorrente = request.IdContaCorrente,
                        DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                        TipoMovimento = TipoMovimento,
                        Valor = request.Valor
                    });


                return Result<string>.Success(objMovimento.idmovimento);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message, TipoFalha.ERROR_ADDING_MOVEMENT);
            }
        }


        public async Task<string> ValidaIdempotencia(MovimentacaoContaCorrente requisicao)
        {


            using IDbConnection connection = _databaseBootstrap.GetConnection();

            string query = @"
                    SELECT *
                    FROM idempotencia  
                    WHERE requisicao  = @requisicao";

            IEnumerable<dynamic> result = await connection.QueryAsync<dynamic>(query,
                new
                {
                    requisicao = requisicao.IdentificacaoRequisicao
                });



            if (result != null && result.Any())
            {

                
                string query2 = @"
                    SELECT *
                    FROM MOVIMENTO 
                    WHERE idmovimento = @chave_idempotencia   AND idcontacorrente = @IdContaCorrente AND tipomovimento = @TipoMovimento AND valor = @Valor ";

                dynamic objMovimento = await connection.QueryFirstOrDefaultAsync<dynamic>(query2, new { result.FirstOrDefault().chave_idempotencia, requisicao.IdContaCorrente, requisicao.TipoMovimento, requisicao.Valor });

                if (objMovimento != null)
                {
                    return objMovimento.idmovimento;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        public async void AdicionarIdempotencia(MovimentacaoContaCorrente requisicao, string requestIdMovimento)
        {
            using IDbConnection connection = _databaseBootstrap.GetConnection();

            var idMovimento = await connection.ExecuteScalarAsync<int>(
               "INSERT INTO idempotencia (chave_idempotencia,requisicao, resultado) " +
               "VALUES (@Chave_idempotencia,@Requisicao, @Resultado); SELECT last_insert_rowid();",
               new
               {
                   Chave_idempotencia = requestIdMovimento,
                   Requisicao = requisicao.IdentificacaoRequisicao,
                   Resultado = "Cadastrada"
               });

        }

    }
}
