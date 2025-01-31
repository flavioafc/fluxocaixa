# ADR-0006: Decisão Sobre o Uso do RabbitMQ para Comunicação Assíncrona

## 1️⃣ Contexto

Inicialmente, consideramos utilizar o **Azure Service Bus** como solução de comunicação assíncrona entre os serviços do **Fluxo de Caixa Diário**, conforme documentado no **ADR-004**. No entanto, após revisão da implementação e necessidades do projeto, decidimos seguir com **RabbitMQ** como solução definitiva.

## 2️⃣ Problema

A migração para o Azure Service Bus traria os seguintes desafios:

- **Custo Operacional**: O Azure Service Bus introduz custos que não eram necessários com o RabbitMQ, já que a infraestrutura atual já suporta mensageria sem encargos adicionais.
- **Complexidade na Integração**: A adaptação exigiria mudanças no código e nas dependências de mensageria já desenvolvidas com RabbitMQ.
- **Independência Tecnológica**: O RabbitMQ permite que o projeto continue independente de um provedor de nuvem, evitando lock-in no Azure.
- **Controle sobre a Infraestrutura**: Como o RabbitMQ pode ser hospedado on-premises ou em qualquer provedor, temos maior flexibilidade para ajustes e otimizações.

## 3️⃣ Decisão

Manter **RabbitMQ** como solução de mensageria do sistema, garantindo:

1. **Serviço de Controle de Lançamentos**:
   - Publica mensagens no RabbitMQ sempre que um lançamento é registrado.

2. **Serviço de Consolidação Diário**:
   - Consome as mensagens do RabbitMQ para processar os lançamentos e calcular os saldos consolidados.

## 4️⃣ Justificativa

1. **Desempenho e Baixa Latência**:
   - RabbitMQ permite comunicação eficiente e entrega rápida de mensagens.

2. **Escalabilidade e Controle**:
   - Podemos configurar estratégias como **"fanout", "direct" e "topic" exchanges** para distribuir mensagens entre consumidores.

3. **Persistência de Mensagens**:
   - O RabbitMQ permite persistência de mensagens com alta confiabilidade.

4. **Independência de Nuvem**:
   - Mantemos a flexibilidade de rodar a solução em **qualquer infraestrutura**.

5. **Experiência da Equipe**:
   - O time já tem conhecimento e familiaridade com **RabbitMQ**, reduzindo curva de aprendizado.

## 5️⃣ Consequências

### ✅ **Positivas**
- **Continuidade da arquitetura atual** sem necessidade de refatoração.
- **Custo zero adicional** comparado ao Azure Service Bus.
- **Maior controle sobre configuração e performance**.
- **Evita vendor lock-in** no Azure.

### ❌ **Negativas**
- **Necessidade de gerenciar a infraestrutura do RabbitMQ** (manutenção e monitoramento).
- **Não possui integração nativa com os serviços do Azure** como o Service Bus teria.

## 6️⃣ Alternativas Consideradas

| Alternativa            | Vantagens | Desvantagens |
|------------------------|-----------|--------------|
| **Azure Service Bus**  | Integração nativa com Azure, monitoramento avançado | Custo adicional, dependência da nuvem |
| **RabbitMQ (Escolhido)** | Controle total, independente da nuvem, baixo custo | Requer gestão da infraestrutura |
| **Apache Kafka**       | Alta escalabilidade para eventos distribuídos | Complexidade e overhead desnecessário |

## 7️⃣ Decisão Final

Manter **RabbitMQ** como a solução de mensageria do projeto, garantindo **baixo custo, controle sobre a infraestrutura e flexibilidade**.

📄 **Veja também:**  
- 📄 [ADR-004: Consideração do Azure Service Bus](./ADR-004-Decisao-Sobre-Azure-Service-Bus.md)  

