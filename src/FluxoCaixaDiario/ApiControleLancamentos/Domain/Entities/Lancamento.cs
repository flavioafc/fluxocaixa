using ApiControleLancamentos.Domain.Enums;
using MassTransit.Caching;

namespace ApiControleLancamentos.Domain.Entities;


public class Lancamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; }
    public StatusLancamento Status { get; set; } = StatusLancamento.Ativo;
    public DateTime Data { get; set; }
    
    public Lancamento() { }
        
    public Lancamento(decimal valor, TipoLancamento tipo, string descricao, DateTime data)
    {
        Valor = valor;
        Tipo = tipo;
        Descricao = descricao;
        Data = data;
    }

    public void AtualizarDados(string descricao, decimal valor, TipoLancamento tipo, DateTime data)
    {
        Descricao = descricao;
        Valor = valor;
        Tipo = tipo;
        Data = data;
    }

    public void Cancelar()
    {
        Status = StatusLancamento.Cancelado;
    }
}
