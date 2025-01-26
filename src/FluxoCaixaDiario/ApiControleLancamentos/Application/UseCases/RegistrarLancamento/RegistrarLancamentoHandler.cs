using ApiControleLancamentos.Application.Interfaces;
using ApiControleLancamentos.Domain.Entities;
using ApiControleLancamentos.Domain.Enums;
using ApiControleLancamentos.Domain.Events;
using ApiControleLancamentos.Infra.Persistence.Repositories;

namespace ApiControleLancamentos.Application.UseCases.RegistrarLancamento
{
    public class RegistrarLancamentoHandler(ILancamentoRepository repository, IEventPublisher eventPublisher)
    {
        private readonly ILancamentoRepository _repository = repository;
        private readonly IEventPublisher _eventPublisher = eventPublisher;

        public async Task<int> Handle(RegistrarLancamentoCommand command)
        {
            var lancamento = new Lancamento(command.Valor, command.Tipo, command.Descricao, DateTime.UtcNow);

            await _repository.AddAsync(lancamento);
            await _eventPublisher.PublishAsync(new LancamentoCriadoEvent(lancamento.Id, lancamento.Valor, lancamento.Tipo.ToString(), lancamento.Data));
            return lancamento.Id;
        }
    }
   

}
