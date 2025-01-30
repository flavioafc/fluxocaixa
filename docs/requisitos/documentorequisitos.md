# Documento de Requisitos

## 1. Introdução

### 1.1 Objetivo
Este documento descreve os requisitos de negócio e técnicos para um sistema de **controle de fluxo de caixa**. O principal objetivo é permitir que um pequeno comércio registre lançamentos financeiros (créditos, débitos, estornos), consolide diariamente esses valores e disponibilize relatórios de forma confiável.

### 1.2 Escopo
A solução é composta por **três aplicações** principais:

1. **ApiControleLancamentos**: Responsável por registrar, atualizar e cancelar lançamentos, além de publicar eventos no RabbitMQ.  
2. **WorkerConsolidado**: Serviço do tipo “Worker” que consome eventos do RabbitMQ e processa o fluxo de caixa diário, armazenando dados consolidados.  
3. **ApiRelatorios**: Exposição de endpoints que consultam o consolidado gerado e fornecem relatórios (por dia, intervalo, etc.).

### 1.3 Definições e Abreviações
- **Lançamento**: Operação financeira de crédito, débito ou estorno.  
- **Fluxo de Caixa**: Conjunto de entradas e saídas num determinado período.  
- **Consolidação**: Processo de calcular o saldo diário (soma de créditos e débitos) e armazenar em tabelas de resumo.  
- **Exchange/Fila (RabbitMQ)**: Mecanismo de mensageria para envio/consumo de eventos de forma assíncrona.

---

## 2. Visão Geral da Solução

A arquitetura geral é baseada em uma API para gerenciar lançamentos (**ApiControleLancamentos**), que se comunica com o **RabbitMQ**. Um **Worker** consome esses eventos para consolidar o fluxo de caixa. A **ApiRelatorios** consulta o banco de dados de consolidados para oferecer relatórios.

1. **Cliente** faz requisições HTTP à ApiControleLancamentos (registrando ou manipulando lançamentos).  
2. A API registra os dados em um banco (ex.: SQL Server) e publica um evento no RabbitMQ.  
3. O WorkerConsolidado recebe as mensagens e atualiza o **saldo diário** ou as tabelas de **consolidado**.  
4. A ApiRelatorios lê esses dados consolidados e expõe endpoints para consultas e relatórios.

---

## 3. Requisitos Funcionais

### 3.1. ApiControleLancamentos
1. **RF-1**: **Registrar Lançamento**  
   - Receber lançamento com campos (valor, tipo, descrição, data, etc.).  
   - Validar campos obrigatórios e coerência (ex.: valor > 0 para Crédito, etc.).  
   - Em caso de sucesso, gravar no BD (tabela `Lancamentos`) e publicar evento (`LancamentoCriadoEvent`) no RabbitMQ.

2. **RF-2**: **Atualizar Lançamento**  
   - Permitir alterar informações (valor, tipo, data, descrição) de lançamentos que ainda estejam “Ativos”.  
   - Publicar evento (`LancamentoAtualizadoEvent`) no RabbitMQ após sucesso.

3. **RF-3**: **Cancelar Lançamento**  
   - Alterar status para “Cancelado”, desde que ainda esteja “Ativo”.  
   - Publicar evento (`LancamentoCanceladoEvent`) no RabbitMQ.

4. **RF-4**: **Listar Lançamentos**  
   - Fornecer endpoint com possibilidade de filtros (por data, tipo, status).  
   - Retornar dados paginados, se necessário.

5. **RF-5**: **Regras de Domínio**  
   - Valor não pode ser negativo (exceto se for Estorno, caso a regra exija).  
   - Data não pode ser futura.  
   - Tipo de Lançamento (`Credito`, `Debito`, `Estorno`) define validações específicas.

### 3.2. WorkerConsolidado
6. **RF-6**: **Consumir Mensagens** do RabbitMQ  
   - Conectar-se à fila (ex.: “lancamento-criado-queue”) e consumir eventos de criação/atualização/cancelamento.

7. **RF-7**: **Calcular Fluxo de Caixa Diário**  
   - A cada mensagem, atualizar o total de créditos/débitos do dia correspondente.  
   - Armazenar em uma tabela de consolidação (ex. `FluxoCaixaDiario`), contendo `Data`, `TotalCredito`, `TotalDebito`, `Saldo`.

8. **RF-8**: **Persistir Consolidados**  
   - Garantir que cada dia tenha apenas um registro (ou poucos) que são atualizados incrementally.  
   - Em caso de estorno ou cancelamento, reverter valores.

### 3.3. ApiRelatorios
9. **RF-9**: **Relatório Diário**  
   - Endpoint (`GET /relatorios/diario?data=YYYY-MM-DD`) retornando saldo, créditos e débitos do dia.

10. **RF-10**: **Relatório por Período**  
    - (`GET /relatorios?dataInicial=...&dataFinal=...`) listando cada dia do intervalo com totais de crédito, débito e saldo acumulado.

11. **RF-11**: **Exportação**  
    - **Opcional**: fornecer CSV ou PDF, dependendo do escopo do projeto.

---

## 4. Requisitos Não Funcionais

1. **RNF-1**: **Persistência**  
   - Banco relacional (ex.: SQL Server) com mapeamento via **EF Core** ou Dapper.  
   - Migrations versionando o schema.

2. **RNF-2**: **Mensageria**  
   - **RabbitMQ** com filas duráveis e `autoAck = false` para confiabilidade.  
   - Política de DLX (Dead Letter Exchange) em caso de falhas críticas.

3. **RNF-3**: **Resiliência**  
   - Retentativas em caso de falha no Worker.  
   - Logs de erro e fallback (se necessário).

4. **RNF-4**: **Escalabilidade**  
   - Possibilidade de rodar múltiplas instâncias da ApiControleLancamentos e do WorkerConsolidado.  
   - Configurar prefetch no RabbitMQ para evitar sobrecarga de um único worker.

5. **RNF-5**: **Segurança**  
   - Uso de TLS/SSL no RabbitMQ e no SQL em produção.  
   - Autenticação/Autorização via JWT ou outra abordagem na ApiControleLancamentos e ApiRelatorios.

6. **RNF-6**: **Performance**  
   - Manter latência aceitável (média < 200ms em cenários normais).  
   - Poder lidar com picos de até 50 requisições/segundo.

7. **RNF-7**: **Observabilidade**  
   - Logs estruturados (ex.: Serilog) para cada serviço.  
   - Métricas de consumo de fila, throughput, e uso de CPU/memória.

8. **RNF-8**: **Hospedagem e Deploy**  
   - Docker + Docker Compose (ou Kubernetes) para orquestrar local e produção.  
   - CI/CD definindo build, teste, e entrega automatizada.

---

## 5. Fluxo Principal (Resumido)

1. **Registrar Lançamento**  
   - Cliente chama `POST /lancamentos` na ApiControleLancamentos → A API valida e grava no BD → Publica evento “LancamentoCriadoEvent” em RabbitMQ → Retorna status 201.

2. **WorkerConsolidado** recebe evento  
   - Lê a fila de RabbitMQ → Converte JSON para objeto de domínio → Atualiza/incrementa tabela `FluxoCaixaDiario` → Dá `ACK` na mensagem.

3. **ApiRelatorios** exibe consolidado  
   - Cliente chama `GET /relatorios/diario?data=2025-01-01` → A API busca no BD (tabela `FluxoCaixaDiario`) e retorna JSON com saldo do dia.

---

## 6. Critérios de Aceite

1. **CA-1**: Lançamento com valor negativo em `Credito` ou `Debito` deve ser rejeitado pela API.  
2. **CA-2**: Não pode atualizar/cancelar se já estiver Cancelado.  
3. **CA-3**: Worker deve processar os eventos e manter `Saldo` correto no final de cada dia.  
4. **CA-4**: ApiRelatorios retorna status `200` com dados consolidados corretos, incluindo saldo diário e total de crédito/débito.  
5. **CA-5**: Em picos de 50 req/s, o sistema deve continuar funcionando sem erros críticos (com logs e monitoramento ativados).

---

