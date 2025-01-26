namespace ApiControleLancamentos.Application.UseCases.CancelarLancamento
{
    using ApiControleLancamentos.Domain.Enums;
    using ApiControleLancamentos.Infra.Persistence.Repositories;

    public class CancelarLancamentoHandler
    {
        private readonly ILancamentoRepository _repository;

        public CancelarLancamentoHandler(ILancamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CancelarLancamentoCommand command)
        {
            var lancamento = await _repository.GetByIdAsync(command.Id);

            if (lancamento == null)
            {
                return false; // Lançamento não encontrado
            }

            // Atualiza o status para Cancelado
            lancamento.Cancelar();

            await _repository.UpdateAsync(lancamento);
            return true;
        }
    }

}
