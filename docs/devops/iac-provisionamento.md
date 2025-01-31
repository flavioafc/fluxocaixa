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
4. **Aplicação** → APIs e Worker Services rodando em Azure Kubernetes Service (AKS).  
5. **Monitoramento** → Azure Application Insights e Log Analytics.  

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

