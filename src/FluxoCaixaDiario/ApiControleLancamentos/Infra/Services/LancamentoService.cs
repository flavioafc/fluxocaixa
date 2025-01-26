using ApiControleLancamentos.Domain.Entities;
using ApiControleLancamentos.Infra.Persistence.Repositories;

namespace ApiControleLancamentos.Infra.Services;

public class LancamentoService
{
    private readonly ILancamentoRepository _repository;

    public LancamentoService(ILancamentoRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Lancamento>> GetAll() => _repository.GetAllAsync();

    public Task<Lancamento> GetById(int id) => _repository.GetByIdAsync(id);

    public Task Add(Lancamento lancamento) => _repository.AddAsync(lancamento);
}
