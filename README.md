# 📌 Fluxo de Caixa Diário - Solução Arquitetural

## 🚨 Aviso

**Este projeto é uma Prova de Conceito (POC).**  
Ele foi desenvolvido para aprendizado e experimentação de conceitos arquiteturais, **não sendo solicitado ou pertencente ao trabalho de nenhuma empresa**.

---

## 📖 Índice
1. [Visão Geral](#1-visão-geral)
2. [Arquitetura da Solução](#2-arquitetura-da-solução)
3. [Documentação Completa](#3-documentação-completa)
4. [Como Executar](#4-como-executar)
5. [Observações Finais](#5-observações-finais)

---

## 1️⃣ **Visão Geral**

### 🎯 **Objetivo**
A solução permite gerenciar o **fluxo de caixa** de um pequeno comércio, oferecendo **registro de lançamentos**, **consolidação diária** e **relatórios financeiros**.

### 🏗 **Componentes Principais**
1. **API de Controle de Lançamentos**  
   - Gerencia os lançamentos financeiros (créditos e débitos).  
   - Persiste dados no banco transacional e publica eventos no RabbitMQ.  
2. **Worker de Consolidação**  
   - Consome mensagens de eventos e processa o saldo diário consolidado.  
   - Persiste dados no banco analítico.  
3. **API de Relatórios**  
   - Exibe dados consolidados via API e permite exportação de relatórios.

### 🔄 **Diagrama de Fluxo**
![Fluxo de Negócio](./docs/images/fluxodenegocio.png)

---

## 2️⃣ **Arquitetura da Solução**

A solução adota **Clean Architecture**, **CQRS**, e **mensageria assíncrona**.

### 🏛 **Desenho da Arquitetura**
![Arquitetura Geral](./docs/images/diagramasolucao.png)

### 🔹 **Principais Decisões Arquiteturais**
- Uso de **RabbitMQ** para comunicação assíncrona entre serviços.
- Separação entre **banco transacional** (lançamentos detalhados) e **banco analítico** (dados agregados).
- Uso de **Redis Cache** para otimizar consultas de relatórios frequentes.
- **Autenticação OAuth 2.0 + JWT**, integrado ao **Azure AD**.
- Segurança reforçada com **Key Vault para segredos** e **TLS obrigatório**.

📄 **Documentação detalhada:**  
- [Documento de Arquitetura](./docs/arquitetura/arquitetura-geral.md)  
- [ADR: Adoção de Clean Architecture](./docs/adr/ADR-0001-CleanArchitecture.md)

---

## 3️⃣ **Documentação Completa**

📌 **Requisitos de Negócio e Técnicos**:
- [Documento de Requisitos](./docs/requisitos/documentorequisitos.md)  
- [Requisitos Não-Funcionais](./docs/requisitos/naofuncionais/requisitos-nao-funcionais.md)  

🔐 **Segurança**:
- [Documento de Segurança](./docs/requisitos/DocumentoDeSeguranca.md)  
- [Arquitetura de Segurança](./docs/arquitetura/arquitetura-seguranca.md)  

🚀 **DevOps e Infraestrutura**:
- [DevOps e Deploy](./docs/requisitos/DevOpsEDeploy.md)  
- [Setup Local](./docs/setup/setup-local.md)  
- [Setup de Produção](./docs/setup/setup-deploy.md)  

📊 **Monitoramento e Observabilidade**:
- [Observabilidade e Logs](./docs/requisitos/Observabilidade.md)  

---

## 4️⃣ **Como Executar**

### ✅ **Pré-requisitos**
- **SDK do .NET 8** instalado.
- **Docker** para executar serviços dependentes (RabbitMQ, SQL, Redis).
- **Conta no Azure** (caso queira testar recursos em nuvem).
- **Git** para clonar o repositório.

### 🛠 **Passo a Passo**

1️⃣ **Clonar o Repositório**
```bash
git clone https://github.com/seu-usuario/fluxo-caixa-diario.git
cd fluxo-caixa-diario
