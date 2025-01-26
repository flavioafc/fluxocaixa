using ApiControleLancamentos.Domain.Enums;

namespace ApiControleLancamentos.Application.UseCases.RegistrarLancamento
{
    public class RegistrarLancamentoCommand
    {
        public decimal Valor { get; set; }
        public TipoLancamento Tipo { get; set; }
        public string Descricao { get; set; }
    }


}
