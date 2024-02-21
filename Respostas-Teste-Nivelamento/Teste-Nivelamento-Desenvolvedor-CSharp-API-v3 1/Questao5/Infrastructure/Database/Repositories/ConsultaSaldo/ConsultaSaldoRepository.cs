using Dapper;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.Repositories.ConsultaSaldo
{
    public class ConsultaSaldoRepository : IConsultaSaldoRepository
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;

        public ConsultaSaldoRepository(IDatabaseBootstrap databaseBootstrap)
        {
            _databaseBootstrap = databaseBootstrap;
        }

        public async Task<Result<SaldoContaCorrenteResponse>> ConsultarSaldoContaCorrenteAsync(string IdContaCorrente)
        {
            using IDbConnection connection = _databaseBootstrap.GetConnection();

            string query = @"
                    SELECT CC.idcontacorrente AS IdContaCorrente,                       
                        CC.numero,
                        CC.nome,
                        CC.ativo,
                        SUM(CASE WHEN MOV.tipomovimento = 'D' THEN MOV.valor ELSE 0 END) AS totalDebitos,
                        SUM(CASE WHEN MOV.tipomovimento = 'C' THEN MOV.valor ELSE 0 END) AS totalCreditos
                    FROM ContaCorrente CC
                        INNER JOIN MOVIMENTO MOV ON CC.idcontacorrente = MOV.IdContaCorrente
                    WHERE CC.idcontacorrente = @IdContaCorrente";

            var objContaCorrente = await connection.QueryFirstOrDefaultAsync<dynamic>(query, new { IdContaCorrente = IdContaCorrente });

            decimal saldo = 0;
            if (objContaCorrente.IdContaCorrente != null)
            {
                decimal totalDebitos = objContaCorrente.totalDebitos != null ? (decimal)objContaCorrente.totalDebitos : 0m;
                decimal totalCreditos = objContaCorrente.totalCreditos != null ? (decimal)objContaCorrente.totalCreditos : 0m;

                // --> SALDO = SOMA_DOS_CREDITOS – SOMA_DOS_DEBITOS
                saldo = totalCreditos - totalDebitos;
            }
            else
            {
                string querySemMovimentos = @"
                    SELECT 
                        CC.idcontacorrente AS IdContaCorrente,                       
                        CC.numero,
                        CC.nome,
                        CC.ativo                        
                    FROM ContaCorrente CC                        
                    WHERE CC.idcontacorrente = @IdContaCorrente";

                objContaCorrente = await connection.QueryFirstOrDefaultAsync<dynamic>(querySemMovimentos, new { IdContaCorrente = IdContaCorrente });
            }

            if (objContaCorrente == null)
            {
                return Result<SaldoContaCorrenteResponse>.Failure("Apenas contas correntes cadastradas podem consultar o saldo", TipoFalha.INVALID_ACCOUNT);                
               
            }

            if (objContaCorrente.ativo == 0)
            { 
                return Result<SaldoContaCorrenteResponse>.Failure("Apenas contas correntes ativas podem consultar o saldo", TipoFalha.INACTIVE_ACCOUNT);                
            }
           

            return Result<SaldoContaCorrenteResponse>.Success(new SaldoContaCorrenteResponse
            {
                NumeroContaCorrente = objContaCorrente.numero.ToString(),
                TitularContaCorrente = objContaCorrente.nome,
                DataHoraResposta = DateTime.Now,
                SaldoAtual = Convert.ToDecimal(saldo)
            });
        }
    }
}
