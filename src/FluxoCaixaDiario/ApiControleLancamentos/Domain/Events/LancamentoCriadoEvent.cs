namespace ApiControleLancamentos.Domain.Events
{
    public class LancamentoCriadoEvent(int id, decimal valor, string tipo, DateTime data)
    {
        public int Id { get; } = id;
        public decimal Valor { get; } = valor;
        public string Tipo { get; } = tipo;
        public DateTime Data { get; } = data;
    }
}
