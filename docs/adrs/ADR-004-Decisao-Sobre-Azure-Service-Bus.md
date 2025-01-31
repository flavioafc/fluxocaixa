# ADR-004: Decisão Sobre o Uso do Azure Service Bus para Comunicação Assíncrona [DEPRECATED]

## Contexto

O sistema de Fluxo de Caixa Diário precisa processar lançamentos de créditos e débitos e consolidar saldos diários, garantindo alta escalabilidade, resiliência e desacoplamento entre os serviços. A comunicação entre o **Serviço de Controle de Lançamentos** e o **Serviço de Consolidado Diário** exige um mecanismo assíncrono para:

1. Garantir que os lançamentos sejam processados de forma confiável, mesmo em cenários de alta carga.
2. Permitir que os serviços sejam escalados independentemente.
3. Prover uma arquitetura resiliente a falhas temporárias em um dos serviços.

## Problema

Sem um mecanismo de comunicação assíncrona:
- Os serviços ficariam fortemente acoplados, dificultando o escalonamento e aumentando a complexidade.
- Em caso de falha no **Serviço de Consolidado Diário**, os lançamentos não seriam processados, comprometendo a confiabilidade do sistema.
- Não haveria suporte para processamento eventual, caso a carga do sistema aumentasse além da capacidade dos serviços.

## Decisão

Adotar o **Azure Service Bus** como solução de comunicação assíncrona entre os serviços:

1. **Serviço de Controle de Lançamentos**:
   - Publica mensagens no Azure Service Bus sempre que um lançamento é registrado.

2. **Serviço de Consolidado Diário**:
   - Consome as mensagens do Azure Service Bus para processar lançamentos e calcular os saldos consolidados.

## Justificativa

1. **Desacoplamento**:
   - O uso de filas garante que os serviços possam operar de forma independente, permitindo escalabilidade e resiliência.

2. **Resiliência**:
   - O Azure Service Bus oferece garantias de entrega confiável, incluindo **at-least-once delivery**, permitindo que mensagens sejam processadas mesmo em caso de falhas temporárias.

3. **Escalabilidade**:
   - O Azure Service Bus suporta alta taxa de mensagens por segundo e permite escalonamento automático de serviços consumidores.

4. **Flexibilidade**:
   - Oferece suporte a padrões de mensagens como **filas** (FIFO) e **tópicos** (pub/sub), permitindo expansão futura para novos consumidores.

5. **Monitoramento**:
   - O Azure Service Bus oferece métricas e logs integrados ao Azure Monitor, facilitando a análise e solução de problemas.

6. **Simplicidade na Integração**:
   - Suporte nativo para .NET com bibliotecas como **Azure.Messaging.ServiceBus**, facilitando a implementação.

## Consequências

### Positivas:

- **Desempenho**: Permite processamento assíncrono, eliminando dependências de tempo real entre os serviços.
- **Escalabilidade**: Serviços podem ser escalados horizontalmente de forma independente.
- **Resiliência**: Falhas temporárias em um dos serviços não comprometem o sistema como um todo.

### Negativas:

- **Complexidade Adicional**: Introduz a necessidade de gerenciar filas e mensagens.
- **Custo**: O uso do Azure Service Bus implica custos operacionais, que crescem com o volume de mensagens.

## Alternativas Consideradas

1. **Comunicação Sincronizada (HTTP)**:
   - Simples e fácil de implementar.
   - Desvantagem: Fortemente acoplado, menos resiliente e não escalável para altas cargas.

2. **Armazenamento Temporário em Banco de Dados**:
   - Usar tabelas temporárias no banco para simular filas.
   - Desvantagem: Baixo desempenho e alta latência, além de não ser uma solução projetada para comunicação assíncrona.

3. **Outros Sistemas de Mensageria (ex.: RabbitMQ, Kafka)**:
   - RabbitMQ: Ótima solução, mas exigiria gerência adicional da infraestrutura.
   - Kafka: Mais adequado para processamento em tempo real de grandes volumes de dados, mas complexo demais para o escopo atual.

## Decisão Final

Adotar o **Azure Service Bus** como mecanismo de comunicação assíncrona entre os serviços. Ele atende aos requisitos de escalabilidade, resiliência e simplicidade de integração, garantindo alta confiabilidade no processamento de mensagens.
