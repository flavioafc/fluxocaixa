namespace ApiControleLancamentos.Application.UseCases.ListarLancamentos
{
    using ApiControleLancamentos.Application.DTOs;
    using ApiControleLancamentos.Infra.Persistence.Repositories;

    public class ListarLancamentosHandler
    {
        private readonly ILancamentoRepository _repository;

        public ListarLancamentosHandler(ILancamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LancamentoDto>> Handle()
        {
            var lancamentos = await _repository.GetAllAsync();
            return lancamentos.Select(l => new LancamentoDto
            {
                Id = l.Id,
                Data = l.Data,
                Descricao = l.Descricao,
                Status = l.Status,
                Tipo = l.Tipo,
                Valor = l.Valor
            });
        }

        public async Task<LancamentoDto?> HandleById(int id)
        {
            var lancamento = await _repository.GetByIdAsync(id);
            if (lancamento == null)
            {
                return null;
            }
            return new LancamentoDto
            {
                Id = lancamento.Id,
                Data = lancamento.Data,
                Descricao = lancamento.Descricao,
                Status = lancamento.Status,
                Tipo = lancamento.Tipo,
                Valor = lancamento.Valor

            };
        }
    }

}
