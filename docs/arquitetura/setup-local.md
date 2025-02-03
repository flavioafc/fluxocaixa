# 🛠 Configuração do Ambiente Local

## 1️⃣ Introdução
Este documento fornece um guia passo a passo para configurar e executar o ambiente localmente para desenvolvimento e testes do projeto **Fluxo de Caixa Diário**.

A execução local é feita utilizando **Docker Compose** para os serviços dependentes e **.NET 8** para as APIs e o Worker.

---

## 2️⃣ Pré-requisitos
Antes de iniciar, certifique-se de ter os seguintes componentes instalados:

### 🔹 Softwares Necessários
✅ **.NET 8 SDK** → [Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
✅ **Docker Desktop** → [Download](https://www.docker.com/products/docker-desktop)  
✅ **Git** → [Download](https://git-scm.com/downloads)  
✅ **Visual Studio Code ou Visual Studio** para desenvolvimento

### 🔹 Configuração do Docker
Para garantir que o Docker funcione corretamente:
1. Abra o **Docker Desktop** e certifique-se de que está rodando.
2. No **Settings**, verifique se a opção **Use the WSL 2 based engine** está habilitada (se estiver no Windows).
3. O limite de memória alocado para contêineres deve ser **pelo menos 4GB**.

---

## 3️⃣ Clonar o Repositório
Abra um terminal e execute:
```bash
# Clonar o repositório
git clone https://github.com/flavioafc/fluxocaixa.git

# Entrar no diretório do projeto
cd fluxocaixa
```

---

## 4️⃣ Subir os Serviços Dependentes
Os serviços como **SQL Server, RabbitMQ, Redis, Prometheus e Grafana** são gerenciados via **Docker Compose**. Para iniciar:
```bash
# Subir os containers com os serviços necessários
docker-compose up -d
```
📌 **Serviços disponíveis:**
- **SQL Server** → `localhost:1433`
- **RabbitMQ** → `localhost:5672` (Painel: `http://localhost:15672` - `guest/guest`)
- **Redis Cache** → `localhost:6379`
- **Prometheus** → `http://localhost:9090`
- **Grafana** → `http://localhost:3000` (Login: `admin/admin`)

Para verificar os logs e conferir se os containers estão rodando corretamente:
```bash
docker ps
```
Se precisar parar os containers:
```bash
docker-compose down
```

---

## 5️⃣ Executar as Aplicações

### 📌 API de Controle de Lançamentos
```bash
cd src/ApiControleLancamentos

# Rodar a API localmente
 dotnet run
```
Acesse o Swagger para testar os endpoints:  
➡️ `http://localhost:5116/swagger`

### 📌 Worker de Consolidação
```bash
cd src/WorkerConsolidado

# Rodar o Worker
 dotnet run
```

### 📌 API de Relatórios
```bash
cd src/ApiRelatorios

# Rodar a API
 dotnet run
```

Acesse o Swagger da API de Relatórios:  
➡️ `http://localhost:5117/swagger`

---

## 6️⃣ Testar a Observabilidade

### 📊 **Verificar Métricas com Prometheus**
- Acesse **Prometheus**: `http://localhost:9090`
- Teste a query:
```text
http_requests_received_total
```

### 📊 **Visualizar Métricas no Grafana**
1. Acesse **Grafana**: `http://localhost:3000`
2. Login: `admin` / `admin`
3. Vá em **Configuration > Data Sources** e adicione **Prometheus** (`http://prometheus:9090`)
4. Importe os dashboards existentes.

---

## 7️⃣ Debug e Desenvolvimento
Se estiver utilizando o **Visual Studio**:
1. **Abra a solução** `FluxoCaixaDiario.sln`
2. **Defina múltiplos projetos de inicialização**:
   - `ApiControleLancamentos`
   - `WorkerConsolidado`
   - `ApiRelatorios`
3. **Pressione F5** para iniciar o debug.

Para depuração no **VS Code**:
1. **Instale a extensão C#**.
2. Configure os **launch.json** para cada aplicação.
3. Use o Debug (`F5`) para iniciar.

---

## 8️⃣ Finalizando
Agora o ambiente local está pronto para testes e desenvolvimento. Se encontrar problemas:
- Verifique os **logs do Docker** (`docker logs <container_id>`).
- Certifique-se de que as **APIs estão rodando** (`http://localhost:5116/swagger`).
- Confira o status do **RabbitMQ** (`http://localhost:15672`).

Qualquer erro ou comportamento inesperado, verifique os logs e reinicie os serviços.

🚀 **Agora você está pronto para desenvolver e testar o Fluxo de Caixa Diário!**

