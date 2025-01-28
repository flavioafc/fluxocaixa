namespace WorkerConsolidado.Models
{
    public class MovimentoFinanceiro
    {
        public string Tipo { get; set; } // "ENTRADA" ou "SAIDA"
        public decimal Valor { get; set; }
        public DateTime DataMovimento { get; set; }
        public string Descricao { get; set; }
    }
}
