using ApiControleLancamentos.Application.Interfaces;
using ApiControleLancamentos.Domain.Entities;
using ApiControleLancamentos.Domain.Events;
using ApiControleLancamentos.Infra.Persistence.Repositories;
using ApiControleLancamentos.Infra.Utils;
using Azure.Core.Pipeline;

namespace ApiControleLancamentos.Application.UseCases.RegistrarLancamento
{
    public class RegistrarLancamentoHandler
    {
        private readonly ILancamentoRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarLancamentoHandler(
            ILancamentoRepository repository,
            IEventPublisher eventPublisher,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(RegistrarLancamentoCommand command)
        {
            // Cria o lançamento
            var lancamento = new Lancamento(command.Valor, command.Tipo, command.Descricao, DateTime.UtcNow);

            // Adiciona o lançamento no repositório
            await _repository.AddAsync(lancamento);

            // Criar o evento para mandar pro rabbit
            var evento = new LancamentoCriadoEvent(
                lancamento.Id, lancamento.Valor, lancamento.Tipo.ToString(), lancamento.Data
            );

            try
            {
                // Publica o evento e tenta novamente em caso de falhas transitórias
                await PollyPolicies.RetryPolicy.ExecuteAsync(() => _eventPublisher.PublishAsync(evento));

                // Confirma as mudanças no banco apenas após publicação bem-sucedida
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // Loga a falha e pode lançar para o consumidor final, caso necessário
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("Falha ao publicar o evento no RabbitMQ", ex);
            }

            return lancamento.Id;
        }
    }
}
