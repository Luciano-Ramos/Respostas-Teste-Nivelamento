namespace Questao5.Application.Commands.Responses
{
    public class SaldoContaCorrenteResponse
    {
        public string NumeroContaCorrente { get; set; }
        public string TitularContaCorrente { get; set; }
        public DateTime DataHoraResposta { get; set; }
        public decimal SaldoAtual { get; set; }
    }
}
