# ADR-001: Decisão de Adotar Microserviços

## Contexto

O sistema de Fluxo de Caixa Diário precisa atender aos seguintes requisitos:

- Processar lançamentos de créditos e débitos de maneira independente e consistente.
- Gerar relatórios diários consolidados sem impactar as operações transacionais.
- Escalar de forma eficiente para atender picos de carga de até 50 requisições por segundo.
- Prover alta disponibilidade e resiliência.

## Problema

Uma arquitetura monolítica poderia atender aos requisitos funcionais iniciais, mas apresenta as seguintes limitações:

1. **Baixa escalabilidade**: Todos os componentes estariam acoplados, dificultando o escalonamento individual de partes do sistema.
2. **Manutenção complexa**: Alterar ou evoluir funcionalidades impactaria outras partes do sistema, aumentando o risco de regressões.
3. **Conflitos de carga**: Consultas pesadas (relatórios) competiriam por recursos com operações transacionais (lançamentos), resultando em possível degradação de desempenho.

## Decisão

Adotar uma arquitetura baseada em **microserviços**, separando as responsabilidades em serviços especializados:

1. **Serviço de Controle de Lançamentos**:
   - Responsável por registrar lançamentos no banco transacional.
   - Publica eventos no Azure Service Bus para notificar outros serviços sobre os lançamentos registrados.

2. **Serviço de Consolidado Diário**:
   - Consome eventos de lançamentos do Azure Service Bus.
   - Calcula os saldos diários consolidados e os armazena em um banco analítico.

3. **Serviço de Relatórios**:
   - Fornece relatórios consolidados e consultas detalhadas.
   - Usa cache distribuído (Redis) para melhorar a performance de consultas frequentes.

## Justificativa

1. **Escalabilidade**:
   - Cada serviço pode ser escalado individualmente conforme a demanda.
   - O processamento de relatórios pesados não impacta o registro de lançamentos.

2. **Manutenção**:
   - Cada serviço é isolado, permitindo que equipes trabalhem independentemente em funcionalidades específicas.

3. **Resiliência**:
   - Falhas em um serviço (ex.: relatórios) não afetam os outros (ex.: registro de lançamentos).

4. **Evolução**:
   - Novas funcionalidades podem ser adicionadas como novos serviços, sem impacto direto nos existentes.

## Consequências

### Positivas:

- Maior escalabilidade e resiliência do sistema.
- Isolamento de responsabilidades, facilitando manutenção e testes.
- Melhor alocação de recursos de infraestrutura.

### Negativas:

- Maior complexidade operacional, com necessidade de orquestrar serviços e gerenciar comunicação assíncrona.
- Custos adicionais com infraestrutura e monitoramento de múltiplos serviços.

## Alternativas Consideradas

1. **Arquitetura Monolítica**:
   - Simples de implementar e com menor custo inicial.
   - Desvantagem: Não atende aos requisitos de escalabilidade e resiliência.

2. **Arquitetura Modular**:
   - Componentes modularizados dentro de um monolito.
   - Desvantagem: A carga ainda seria centralizada, limitando a escalabilidade.
