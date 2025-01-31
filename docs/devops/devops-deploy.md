# 🚀 DevOps e Deploy

## 1️⃣ Introdução

Este documento descreve a estratégia de **CI/CD (Continuous Integration / Continuous Deployment)** para a solução **Fluxo de Caixa Diário**, garantindo um fluxo de deploy automatizado e seguro.

A infraestrutura segue os princípios de **Infraestrutura como Código (IaC)**, garantindo **reprodutibilidade, escalabilidade e versionamento**.

---

## 2️⃣ Estratégia de Versionamento e Branches

📌 **Fluxo de Branches**  
- **`main`** → Contém a versão **estável** em produção.  
- **`develop`** → Integração de novas features antes de irem para produção.  
- **`feature/*`** → Desenvolvimento de novas funcionalidades.  
- **`hotfix/*`** → Correções emergenciais na produção.  

---

## 3️⃣ Pipelines de CI/CD

A automação de deploy é gerenciada via **GitHub Actions / Azure DevOps**, garantindo **builds automatizados, testes e implantação**.

### 🔹 **Pipeline de CI (Continuous Integration)**
📌 Disparado em **pull requests** e **push para develop/main**.  
✔️ **Passos do pipeline**:
1. 🔹 Restaurar pacotes do .NET  
2. 🔹 Rodar testes unitários  
3. 🔹 Análise estática de código (SonarQube)  
4. 🔹 Construção da aplicação  
5. 🔹 Geração de imagens Docker  
6. 🔹 Publicação do artefato  

### 🔹 **Pipeline de CD (Continuous Deployment)**
📌 Disparado após **merge na `main` ou `develop`**.  
✔️ **Passos do pipeline**:
1. 🔹 Implantação automática no ambiente de **Staging**.  
2. 🔹 Execução de testes de integração.  
3. 🔹 Aprovação manual para deploy em **Produção**.  
4. 🔹 Rollback automático em caso de falha.  

📄 **Veja também**:  
- [Infraestrutura como Código (IaC)](./iac-provisionamento.md)  
- [Setup do ambiente local](../setup/setup-local.md)  
