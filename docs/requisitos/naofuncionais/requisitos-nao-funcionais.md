# 📌 Requisitos Não-Funcionais

## 1️⃣ Introdução
Os **Requisitos Não-Funcionais (RNFs)** definem os critérios que a solução deve atender **além da funcionalidade**, garantindo **performance, segurança, escalabilidade, resiliência e observabilidade**.

Abaixo estão os **principais requisitos não-funcionais** da solução.

---

## 2️⃣ Requisitos Não-Funcionais

### 🚀 **RNF-1: Escalabilidade**
- A solução deve suportar um aumento de carga sem degradação significativa de desempenho.
- O sistema deve ser capaz de lidar com pelo menos **50 requisições por segundo** no pico de utilização.
- **Escalabilidade horizontal**: Serviços podem rodar em múltiplas instâncias sem estado.
- **RabbitMQ** pode distribuir mensagens entre múltiplos Workers.

📄 **Leia mais:** [Arquitetura e Infraestrutura](../arquitetura/arquitetura-geral.md)  

---

### 🔄 **RNF-2: Resiliência**
- O sistema deve se recuperar automaticamente de falhas de componentes individuais.
- **Mensageria (RabbitMQ)** garantirá a **entrega garantida** dos eventos.
- **Retentativas** e **fallbacks** serão implementados para evitar falhas inesperadas.
- A base de dados deve ser configurada para **backup automático** e **failover**.

📄 **Leia mais:** [Arquitetura de Resiliência](../arquitetura/arquitetura-geral.md)  

---

### 🔐 **RNF-3: Segurança**
- **Autenticação** via **OAuth 2.0 e OpenID Connect** (ex.: Azure AD, Keycloak, Auth0).
- **Autorização** baseada em **JWT**, com políticas de acesso granular.
- **TLS/SSL** obrigatório para APIs, RabbitMQ e comunicação com banco de dados.
- **Criptografia** de dados sensíveis no banco via **TDE (Transparent Data Encryption)**.
- **Gerenciamento de segredos** no **Azure Key Vault** ou outro cofre seguro.

📄 **Leia mais:** [Documento de Segurança](../requisitos/naofuncionais/seguranca.md)  

---

### ⚡ **RNF-4: Performance**
- A resposta da API deve ter um tempo médio de **latência abaixo de 200ms**.
- Consultas de relatórios frequentes serão **cacheadas no Redis** para melhor tempo de resposta.
- As operações mais pesadas (ex.: consolidação de fluxo de caixa) serão **processadas em background (RabbitMQ + Worker)**.

📄 **Leia mais:** [ADR-003: Cache para Relatórios](../adr/ADR-0003-Cache.md)  

---

### 📊 **RNF-5: Observabilidade**
- Todos os serviços devem gerar **logs estruturados** para rastreamento de eventos.
- **Métricas de monitoramento** devem ser coletadas, como:
  - Quantidade de mensagens processadas por minuto.
  - Tempo médio de resposta da API.
  - Erros e falhas em requisições.
- Alertas configurados para detectar **padrões anômalos** (ex.: falhas recorrentes de login).

📄 **Leia mais:** [Observabilidade e Logs](../requisitos/Observabilidade.md)  

---

### 🏗 **RNF-6: Manutenibilidade**
- O código deve seguir os princípios de **Clean Architecture** e ser modular.
- A API e os Workers devem ser **fáceis de atualizar** sem downtime significativo.
- Configuração de **CI/CD** para **automação de testes e deploys**.

📄 **Leia mais:** [DevOps e Deploy](../requisitos/DevOpsEDeploy.md)  

---

### 🎯 **RNF-7: Testabilidade**
- O sistema deve ter **testes unitários e de integração** cobrindo pelo menos **80% do código**.
- Simulação de carga para validar a **resistência** do sistema sob alto tráfego.
- Testes end-to-end para garantir o **funcionamento da solução**.

📄 **Leia mais:** [Setup de Testes](../setup/setup-testes.md)  

---

### ☁️ **RNF-8: Deploy e Infraestrutura**
- O sistema será **containerizado (Docker)** para facilitar a replicação do ambiente.
- Deploy automatizado via **Azure DevOps / GitHub Actions / Jenkins**.
- Configuração **Infrastructure as Code (IaC)** usando Terraform ou Bicep (opcional).
- Banco de dados provisionado via **Azure SQL** ou equivalente.

📄 **Leia mais:** [Setup de Deploy](../setup/setup-deploy.md)  

---

## 3️⃣ Conclusão

Os **Requisitos Não-Funcionais (RNFs)** garantem que a solução não apenas funcione corretamente, mas seja **segura, escalável e resiliente**.  

Eles cobrem aspectos fundamentais como **performance, segurança, escalabilidade, monitoramento e testes**, permitindo que a solução seja utilizada **de forma confiável em produção**.

📄 **Documentação Complementar**:
- [Documento de Segurança](../requisitos/DocumentoDeSeguranca.md)  
- [Arquitetura da Solução](../arquitetura/arquitetura-geral.md)  
- [Observabilidade e Logs](../requisitos/Observabilidade.md)  
- [DevOps e Deploy](../requisitos/DevOpsEDeploy.md)  

---
