using ApiControleLancamentos.Models;
using ApiControleLancamentos.Repositories;

namespace ApiControleLancamentos.Services;

public class LancamentoService
{
    private readonly ILancamentoRepository _repository;

    public LancamentoService(ILancamentoRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Lancamento>> GetAll() => _repository.GetAll();

    public Task<Lancamento> GetById(int id) => _repository.GetById(id);

    public Task Add(Lancamento lancamento) => _repository.Add(lancamento);
}
