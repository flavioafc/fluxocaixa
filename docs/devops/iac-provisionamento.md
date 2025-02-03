# 📦 Infraestrutura como Código (IaC) - Provisionamento de Ambientes

## 1️⃣ Introdução

Este documento detalha como provisionar os ambientes usando **Infraestrutura como Código (IaC)**. O uso de IaC garante que toda a configuração da infraestrutura seja **reprodutível, versionada e escalável**.

A solução suporta provisionamento para **Azure e Docker**, utilizando **Terraform** e **Docker Compose**.

---

## 2️⃣ Estrutura da Infraestrutura

### 🔹 **Recursos provisionados**
1. **Banco de Dados** → Azure SQL Database.  
2. **Mensageria** → RabbitMQ gerenciado via container.  
3. **Cache** → Redis Cache para otimizar consultas de relatórios.  
4. **API Gateway** → Azure API Management (APIM) para gerenciar e proteger APIs.  
5. **Aplicação** → APIs e Worker Services rodando em Azure Kubernetes Service (AKS).  
6. **Monitoramento e Observabilidade**:
   - **Prometheus + Grafana** → Coleta e exibe métricas detalhadas da aplicação.  
   - **Azure Monitor + Log Analytics** → Monitoramento de logs e análise de desempenho.  
   - **OpenTelemetry** → Rastreia chamadas distribuídas entre os serviços.  

📂 **Código de provisionamento:**  
- `infra/terraform/` → Provisionamento da infraestrutura na **Azure**.  
- `infra/docker/` → Configuração local usando **Docker Compose**.  

---

## 3️⃣ Provisionamento no Azure com Terraform

### 🔹 **Requisitos**
1. **Terraform instalado** → [Download](https://www.terraform.io/downloads.html)  
2. **CLI do Azure instalada** → [Download](https://aka.ms/InstallAzureCli)  

### 🔹 **Passos para provisionar o ambiente**
```bash
# 1️⃣ Autenticar no Azure
az login

# 2️⃣ Inicializar o Terraform
terraform init

# 3️⃣ Planejar a infraestrutura
terraform plan -out=tfplan

# 4️⃣ Aplicar mudanças
terraform apply tfplan

```
---

🔹 Provisionamento do Azure API Management (APIM)
O APIM é responsável por expor e proteger as APIs da aplicação. Ele inclui funcionalidades como rate limiting, autenticação via Azure AD e roteamento.

📌 O provisionamento do APIM é feito pelo Terraform.


## 4️⃣ Provisionamento Local com Docker Compose
Para rodar a solução localmente com RabbitMQ, Redis e Banco de Dados, usamos Docker Compose.


🔹 Subir os serviços localmente
```bash
docker-compose up -d
```
🔹 Parar os serviços
```bash
docker-compose down
```

