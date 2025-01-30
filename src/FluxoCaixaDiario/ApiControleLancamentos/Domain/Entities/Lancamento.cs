using ApiControleLancamentos.Domain.Enums;
using ApiControleLancamentos.Domain.Exceptions;

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
        DefinirTipoEValor(tipo, valor);
        Descricao = descricao;
        DefinirData(data);
        Status = StatusLancamento.Ativo;
    }

    public void Cancelar()
    {
        if (Status == StatusLancamento.Cancelado)
            throw new DomainException("Lançamento já está cancelado.");

        Status = StatusLancamento.Cancelado;
    }


    public void AtualizarDados(string descricao, decimal valor, TipoLancamento tipo, DateTime data)
    {
        DefinirTipoEValor(tipo, valor);
        Descricao = descricao;
        DefinirData(data);
    }

    private void DefinirTipoEValor(TipoLancamento tipo, decimal valor)
    {
        // Exemplo de regras (adapte conforme sua realidade):
        switch (tipo)
        {
            case TipoLancamento.Credito:
                if (valor <= 0)
                    throw new DomainException("Para crédito, o valor deve ser maior que zero.");
                break;

            case TipoLancamento.Debito:
                if (valor <= 0)
                    throw new DomainException("Para débito, o valor deve ser maior que zero.");
                break;

            case TipoLancamento.Estorno:
                // Exemplo: exigir valor negativo
                if (valor >= 0)
                    throw new DomainException("Para estorno, o valor deve ser negativo (reversão).");
                break;

            default:
                throw new DomainException("Tipo de lançamento inválido.");
        }

        Tipo = tipo;
        Valor = valor;
    }


    public void DefinirData(DateTime data)
    {
        if (data > DateTime.Now)
            throw new DomainException("Data de lançamento não pode ser futura.");

        Data = data;
    }


}
