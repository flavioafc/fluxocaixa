using System;
using WorkerConsolidado.Models;
using Microsoft.Extensions.Logging;

namespace WorkerConsolidado.Services
{
    public class FluxoDeCaixaService
    {
        private readonly ILogger<FluxoDeCaixaService> _logger;

        // Aqui poderiamos ter, por exemplo,
        // um repositório/DAO para gravar em banco de dados,
        // ou outra forma de persistência.
        // private readonly IFluxoDeCaixaRepository _repository;

        // Nessa implementação simples, apenas logamos o valor e o tipo, mas, na prática, 
        // Nós chamariamos um repositório / banco de dados para registrar cada MovimentoFinanceiro, 
        // atualizar tabelas de consolidado diário ou gerar um arquivo de histórico conforme sua necessidade.

        public FluxoDeCaixaService(ILogger<FluxoDeCaixaService> logger)
        {
            _logger = logger;
        }

        public void ProcessarMovimento(MovimentoFinanceiro movimento)
        {
            // Simples exemplo de regra de negócio:
            if (movimento.Tipo == "ENTRADA")
            {
                // Adiciona ao fluxo de caixa
                _logger.LogInformation($"Processando ENTRADA de {movimento.Valor} - {movimento.Descricao}");
                // Lógica adicional...
            }
            else if (movimento.Tipo == "SAIDA")
            {
                // Deduz do fluxo de caixa
                _logger.LogInformation($"Processando SAÍDA de {movimento.Valor} - {movimento.Descricao}");
                // Lógica adicional...
            }
            else
            {
                // Caso não seja um tipo válido
                _logger.LogWarning($"Tipo desconhecido: {movimento.Tipo}");
            }

            // Exemplo: salvar no banco ou atualizar relatórios:
            // _repository.Save(movimento);

            // Caso queiramos processar com base na data, controle diário, etc.,
            // Podemos agrupar tudo por data e, no final do dia, fazer um
            // fechamento. Isso vai depender do requisito de negócio.

        }
    }
}
