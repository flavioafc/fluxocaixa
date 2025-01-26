using ApiControleLancamentos.Application.DTOs;
using ApiControleLancamentos.Domain.Entities;

namespace ApiControleLancamentos.Infra.Persistence.Repositories
{
    public interface ILancamentoRepository
    {
        Task<IEnumerable<Lancamento>> GetAllAsync();
        Task<Lancamento?> GetByIdAsync(int id);
        Task AddAsync(Lancamento lancamento);
        Task UpdateAsync(Lancamento lancamento);
        Task DeleteAsync(Lancamento lancamento);
    }

}
