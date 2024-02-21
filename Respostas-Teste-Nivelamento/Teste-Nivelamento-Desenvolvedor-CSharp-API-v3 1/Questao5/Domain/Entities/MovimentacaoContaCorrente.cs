namespace Questao5.Domain.Entities
{
    public class MovimentacaoContaCorrente
    {        
        public string? IdentificacaoRequisicao { get; set; }
        public string? IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}
