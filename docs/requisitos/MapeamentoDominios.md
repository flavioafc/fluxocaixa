# 🏛 Mapeamento de Domínios e Capacidades de Negócio

## 1️⃣ Introdução
A solução segue os princípios do **Domain-Driven Design (DDD)** para organizar a estrutura do sistema.  
O sistema é dividido no **Domínio de Gestão Financeira**, que contém **Bounded Contexts** bem definidos.  

Cada **Bounded Context** possui responsabilidades próprias e interage com os demais **através de eventos assíncronos**.

---

## 2️⃣ Diagrama de Contextos

O diagrama abaixo representa a separação dos **Bounded Contexts** dentro do **Domínio de Gestão Financeira**.

![Bounded Contexts - Gestão Financeira](./images/dominio-bounded-contexts.png)

---

## 3️⃣ Domínio de Gestão Financeira

O **Domínio de Gestão Financeira** abrange todos os processos relacionados a lançamentos financeiros, consolidação de saldos e relatórios.

Ele contém os seguintes **Bounded Contexts**:

### **🔹 Bounded Context de Fluxo de Caixa**
📌 **Responsabilidade**: Gerencia **lançamentos financeiros** e **cálculo do saldo diário consolidado**.  
📌 **Componentes**:
- **API de Lançamentos** → Permite registrar **créditos e débitos** e publica eventos.  
- **Worker de Consolidação** → Processa eventos do RabbitMQ e **calcula o saldo diário**.  
- **Banco Transacional** → Armazena **lançamentos detalhados**.  

📌 **Eventos Publicados**:
- `LancamentoCriadoEvent` → Quando um novo lançamento é registrado.  
- `SaldoDiarioCalculadoEvent` → Quando um saldo diário consolidado é gerado.  

📌 **Capacidades de Negócio**:
✅ Registro de lançamentos financeiros.  
✅ Cálculo automático do saldo diário consolidado.  
✅ Comunicação assíncrona via eventos no RabbitMQ.  

---

### **🔹 Bounded Context de Relatórios Financeiros**
📌 **Responsabilidade**: Gerencia **consultas e exportação de relatórios consolidados**.  
📌 **Componentes**:
- **API de Relatórios** → Exibe **dados agregados e relatórios financeiros**.  
- **Banco Analítico** → Otimizado para **consultas rápidas** de saldos consolidados.  
- **Redis Cache** → Melhora a performance de consultas frequentes.  

📌 **Eventos Consumidos**:
- `SaldoDiarioCalculadoEvent` → Atualiza relatórios quando um novo saldo é consolidado.  

📌 **Capacidades de Negócio**:
✅ Consulta eficiente de relatórios financeiros.  
✅ Otimização de consultas usando cache.  
✅ Exportação de relatórios para análise financeira.  

---

### **🔹 Bounded Context de Conciliação Bancária**
📌 **Responsabilidade**: Valida **lançamentos internos** comparando com **extratos bancários externos**.  
📌 **Componentes**:
- **API de Conciliação** → Obtém **extratos bancários** e verifica inconsistências.  
- **Serviço de Integração Bancária** → Conecta-se a **bancos externos** via APIs.  
- **Banco Analítico** → Armazena os **dados reconciliados**.  

📌 **Eventos Consumidos**:
- `LancamentoCriadoEvent` → Valida se o lançamento está conciliado com os extratos.  
- `ExtratoRecebidoEvent` → Indica que um novo extrato bancário foi importado.  

📌 **Capacidades de Negócio**:
✅ Integração com bancos para obtenção de extratos.  
✅ Validação automática de lançamentos contra extratos bancários.  
✅ Identificação de discrepâncias financeiras.  

---

## 4️⃣ Infraestrutura e Comunicação

Além dos **contextos de negócio**, a solução conta com uma **camada de infraestrutura** para suportar comunicação e segurança.

📌 **Componentes de Infraestrutura**:
- **RabbitMQ** → Garante **comunicação assíncrona** entre os serviços.  
- **Azure Key Vault** → Gerencia **credenciais e chaves de segurança**.  
- **Monitoramento (Logs e Alertas)** → Captura métricas e eventos para observabilidade.  

📌 **Eventos Importantes**:
- `LancamentoCriadoEvent` → Publicado pelo Bounded Context de Fluxo de Caixa.  
- `SaldoDiarioCalculadoEvent` → Publicado pelo Worker de Consolidação.  
- `ExtratoRecebidoEvent` → Publicado pelo serviço de integração bancária.  

---

## 5️⃣ Relação entre os Contextos

### **🔄 Fluxo de Processamento de Lançamentos**
1️⃣ **O usuário registra um lançamento** na **API de Lançamentos**.  
2️⃣ O evento **`LancamentoCriadoEvent`** é publicado no **RabbitMQ**.  
3️⃣ O **Worker de Consolidação** processa o evento e **calcula o saldo diário**.  
4️⃣ O evento **`SaldoDiarioCalculadoEvent`** é publicado para outros contextos.  

### **📊 Fluxo de Consulta de Relatórios**
1️⃣ O usuário consulta relatórios na **API de Relatórios**.  
2️⃣ O sistema verifica se os dados estão no **Redis Cache**.  
3️⃣ Se necessário, os dados são obtidos do **Banco Analítico**.  

### **🔎 Fluxo de Conciliação Bancária**
1️⃣ O sistema obtém um **extrato bancário externo** via **Serviço de Integração Bancária**.  
2️⃣ O evento **`ExtratoRecebidoEvent`** é publicado no **RabbitMQ**.  
3️⃣ A **API de Conciliação** verifica se os lançamentos batem com os extratos.  

---

📄 **Documentação Relacionada**:
- [Arquitetura Geral](../arquitetura/arquitetura-geral.md)  
- [Requisitos Não-Funcionais](../requisitos/RequisitosNaoFuncionais.md)  
- [Segurança e Autenticação](../arquitetura/arquitetura-seguranca.md)  
