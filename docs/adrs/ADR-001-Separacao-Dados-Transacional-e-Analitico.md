# ADR 001: Separação de Dados Transacional e Analítico usando CQRS

## Contexto
O sistema de Fluxo de Caixa Diário precisa atender aos seguintes requisitos:
- Registrar e armazenar lançamentos detalhados para fins transacionais (créditos e débitos).
- Gerar relatórios de saldos diários consolidados para leitura e análise.
- Suportar alta carga de consultas (50 requisições por segundo) sem impactar as gravações.

Problema:
- Consultas analíticas pesadas podem degradar o desempenho de gravações transacionais, resultando em impacto na experiência do usuário.
- Relatórios precisam ser rápidos e otimizados, mas não exigem os mesmos requisitos ACID das gravações.

## Decisão
Adotar o padrão **CQRS (Command Query Responsibility Segregation)**, separando a persistência de dados em:
1. **Banco Transacional**:
   - Responsável por gravações e armazenamento dos lançamentos detalhados.
   - Exemplo: Azure SQL Database.
2. **Banco Analítico**:
   - Responsável por consultas de saldos consolidados.
   - Exemplo: Azure SQL Database (ou Azure Table Storage para redução de custos).

## Justificativa
1. **Desempenho**:
   - Consultas de leitura intensiva não impactam as operações transacionais.
   - Bancos analíticos podem ser otimizados para leitura (ex.: índices e partições específicas).

2. **Escalabilidade**:
   - O banco transacional pode ser dimensionado para gravações (ex.: auto-scale em Azure SQL).
   - O banco analítico pode ser escalado independentemente para consultas pesadas.

3. **Resiliência**:
   - O banco analítico pode ser reconstruído a partir do banco transacional e dos eventos do **Service Bus**.

4. **Custos**:
   - Inicialmente, dois bancos SQL Server são usados. Alternativamente, o **Banco Analítico** pode ser substituído por Azure Table Storage ou Cosmos DB, reduzindo custos.

5. **Auditabilidade**:
   - O banco transacional mantém um histórico completo e detalhado de todos os lançamentos.

## Consequências
### Positivas:
- Melhor desempenho para gravações e consultas.
- Isolamento entre operações de leitura e gravação, reduzindo conflitos.
- Infraestrutura escalável para cenários de alta carga.

### Negativas:
- Custos iniciais mais altos devido ao uso de dois bancos de dados SQL.
- A complexidade da arquitetura aumenta com a necessidade de sincronização entre bancos.

## Alternativas Consideradas
1. **Manter um único banco SQL**:
   - Simples e com menor custo.
   - Desvantagem: Consultas analíticas podem impactar gravações, e escalabilidade seria limitada.

2. **Usar Table Storage para o Banco Analítico**:
   - Reduz custos.
   - Desvantagem: Pode não atender bem a consultas complexas sem ajustes no modelo de dados.

3. **Cosmos DB para Banco Analítico**:
   - Altamente escalável e otimizado para leitura global.
   - Desvantagem: Maior custo operacional comparado ao Table Storage.

## Decisão Final
Adotar dois bancos separados para implementar CQRS:
- **Banco Transacional**: Azure SQL Database.
- **Banco Analítico**: Inicialmente Azure SQL, com possibilidade de migração para Azure Table Storage.

