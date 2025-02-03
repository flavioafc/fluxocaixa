# 🏛 Requisitos Arquiteturais - Azure API Management (APIM)

## 1️⃣ Introdução  

Este documento define os requisitos arquiteturais para o uso do **Azure API Management (APIM)** na solução **Fluxo de Caixa Diário**. O APIM será utilizado para **centralizar, proteger e gerenciar as APIs**, garantindo **segurança, escalabilidade e observabilidade**.

---

## 2️⃣ Objetivo  

O **APIM** será a **porta de entrada** para todas as chamadas externas às APIs da solução, fornecendo:
✅ **Segurança** → Controle de acesso, autenticação e autorização.  
✅ **Governança** → Padronização do tráfego e monitoramento das requisições.  
✅ **Desempenho** → Caching, rate limiting e balanceamento de carga.  
✅ **Observabilidade** → Monitoramento de logs, métricas e rastreamento distribuído.  

---

## 3️⃣ Requisitos Arquiteturais  

### 📌 **3.1. Segurança e Autenticação**
✔ **Autenticação baseada em OAuth 2.0** com **Azure Entra ID (antigo Azure AD)**.  
✔ Uso de **JWT Bearer Tokens** para garantir acesso seguro às APIs.  
✔ Configuração de **IP Whitelisting** para acesso restrito às APIs.  
✔ **Proteção contra ataques DDoS** por meio de Rate Limiting e Throttling.  

### 📌 **3.2. Governança e Padronização**  
✔ Todas as APIs devem ser **registradas no APIM** e acessadas através dele.  
✔ Definição de **um único endpoint externo** (`https://api.fluxocaixa.com`).  
✔ Exposição das APIs usando **versões e revisões** para facilitar manutenção.  
✔ Suporte a **políticas customizadas** para controle de acesso e manipulação de resposta.  

### 📌 **3.3. Desempenho e Escalabilidade**  
✔ **Rate Limiting** → Limitação de requisições por minuto para evitar sobrecarga.  
✔ **Caching** → Ativar cache para reduzir chamadas redundantes ao backend.  
✔ **Timeout configurável** para evitar requisições longas e não responsivas.  
✔ **Balanceamento de carga** interno para distribuir requisições entre instâncias das APIs.  

### 📌 **3.4. Monitoramento e Observabilidade**  
✔ **Logging centralizado** → Todas as requisições devem ser monitoradas no **Azure Monitor**.  
✔ **Métricas de uso** → Tempo de resposta, taxa de erro, throughput de requisições.  
✔ **Integração com OpenTelemetry** para rastreamento distribuído.  
✔ **Alertas** → Configuração de notificações para falhas e picos de tráfego.  

---

## 4️⃣ Configuração no APIM  

### 📌 **4.1. Definição de Roteamento**  
Todas as chamadas às APIs serão feitas através do APIM, conforme o seguinte modelo:

| API | Rota no APIM | Serviço Backend |
|----|--------------|----------------|
| API Controle de Lançamentos | `/api/lancamentos` | `http://api-controle-lancamentos` |
| API de Relatórios | `/api/relatorios` | `http://api-relatorios` |


### 📌 **4.2. Configuração de Políticas no APIM**  
As **políticas do APIM** serão configuradas para garantir segurança e eficiência:

✔ **Autenticação obrigatória** via JWT Bearer Token  
✔ **Throttling** (limitação de 100 requisições/minuto por usuário)  
✔ **Cache de respostas** para reduzir tráfego desnecessário ao backend  
✔ **Redirecionamento automático de versões depreciadas**  
✔ **Monitoramento e logging de todas as requisições**  

---
