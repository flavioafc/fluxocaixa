namespace ApiControleLancamentos.Application.UseCases.AtualizarLancamento
{
    using ApiControleLancamentos.Domain.Entities;
    using ApiControleLancamentos.Infra.Persistence.Repositories;

    public class AtualizarLancamentoHandler
    {
        private readonly ILancamentoRepository _repository;

        public AtualizarLancamentoHandler(ILancamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(AtualizarLancamentoCommand command)
        {
            // Recupera a entidade de domínio
            var lancamentoDomain = await _repository.GetByIdAsync(command.Id);

            if (lancamentoDomain == null)
            {
                return false; // Lançamento não encontrado
            }

            // Atualiza os campos da entidade de domínio com os dados do comando
            lancamentoDomain.AtualizarDados(
                command.Descricao,
                command.Valor,
                command.Tipo,
                command.Data
            );

            // Persiste a entidade atualizada
            await _repository.UpdateAsync(lancamentoDomain);
            return true;
        }
    }


}
