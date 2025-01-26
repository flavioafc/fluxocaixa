using ApiControleLancamentos.Domain.Enums;

namespace ApiControleLancamentos.Application.DTOs
{
    public class LancamentoDto
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public TipoLancamento Tipo { get; set; }
        public string Descricao { get; set; }
        public StatusLancamento Status { get; set; }
        public DateTime Data { get; set; }
    }


}
