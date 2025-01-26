using ApiControleLancamentos.Models;

namespace ApiControleLancamentos.Repositories;

public interface ILancamentoRepository
{
    Task<IEnumerable<Lancamento>> GetAll();
    Task<Lancamento> GetById(int id);
    Task Add(Lancamento lancamento);
}
