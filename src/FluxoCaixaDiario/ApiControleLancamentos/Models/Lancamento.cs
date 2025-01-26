namespace ApiControleLancamentos.Models;

public enum TipoLancamento
{
    Credito,
    Debito,
    Estorno
}

public enum StatusLancamento
{
    Ativo,
    Cancelado
}

public class Lancamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; }
    public StatusLancamento Status { get; set; } = StatusLancamento.Ativo;
    public DateTime Data { get; set; } = DateTime.UtcNow;
}
