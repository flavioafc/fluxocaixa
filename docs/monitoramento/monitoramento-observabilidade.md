# 🔍 Monitoramento e Observabilidade

## 1️⃣ Introdução

Este documento define as diretrizes de **monitoramento e observabilidade** da aplicação **Fluxo de Caixa Diário**, garantindo **detecção rápida de falhas**, **rastreamento de requisições** e **análise de métricas** para otimização do sistema.

Observabilidade não é apenas monitorar logs, mas compreender **como e por que** um sistema se comporta de determinada maneira.

---

## 2️⃣ Monitoramento vs Observabilidade

| Conceito         | Definição |
|-----------------|------------------------------------------------------------|
| **Monitoramento** | Acompanhamento do status e métricas do sistema (CPU, memória, requisições). |
| **Observabilidade** | Capacidade do sistema de fornecer informações detalhadas sobre seu estado interno através de logs, métricas e traces. |

---

## 3️⃣ Componentes da Observabilidade

A aplicação é monitorada através de **três pilares**:

### 📊 **1. Métricas**
✅ Medição de uso de CPU, memória, requests por segundo.  
✅ Coletadas via **Application Insights, Prometheus, Azure Monitor**.  
✅ Configuração de **alertas para detectar anomalias**.  

### 📄 **2. Logs Estruturados**
✅ **Formato JSON** para facilitar consulta e análise.  
✅ Centralizados via **ELK Stack (Elasticsearch, Logstash, Kibana) ou Azure Monitor**.  
✅ Exemplo de log estruturado:
```json
{
  "timestamp": "2025-01-30T12:45:00Z",
  "level": "INFO",
  "message": "Novo lançamento financeiro registrado.",
  "service": "ApiControleLancamentos",
  "correlation_id": "123456789",
  "usuario": "admin",
  "valor": 200.00
}
```

### 🔍 **3. Rastreamento Distribuído (Distributed Tracing)**
✅ Permite **seguir o fluxo das requisições** entre microserviços.  
✅ Implementado com **OpenTelemetry + Application Insights**.  
✅ Exemplo de rastreamento:
```yaml
Trace ID: abc123xyz
Serviço: ApiControleLancamentos
Endpoint: POST /lancamentos
Duração: 120ms
Dependência: SQL Server (tempo: 45ms)
```

---

## 4️⃣ Ferramentas Utilizadas

### ✅ **Monitoramento de Aplicação**
| Ferramenta             | Finalidade |
|------------------------|-------------------------------------------|
| **Azure Application Insights** | Telemetria de aplicações (tempo de resposta, erros, desempenho). |
| **Prometheus + Grafana** | Coleta de métricas e visualização gráfica. |
| **Azure Monitor + Log Analytics** | Centralização de logs e geração de alertas. |

### ✅ **Logs e Análises**
| Ferramenta             | Finalidade |
|------------------------|-------------------------------------------|
| **Serilog + Elasticsearch** | Armazena e consulta logs estruturados. |
| **Kibana (ELK Stack)** | Interface para visualizar e consultar logs. |
| **Azure Log Analytics** | Visualização e correlação de logs de aplicações no Azure. |

### ✅ **Rastreamento Distribuído**
| Ferramenta             | Finalidade |
|------------------------|-------------------------------------------|
| **OpenTelemetry** | Coleta traces distribuídos para análise do fluxo de requisições. |
| **Jaeger/Zipkin** | Ferramentas para visualizar rastreamento de requisições. |

---

## 5️⃣ Configuração e Boas Práticas

### 📌 **1. Logging Estruturado**
✅ Todos os logs devem ser estruturados em **JSON**.  
✅ Evitar logs sensíveis como senhas e tokens.  
✅ Níveis de log recomendados:
   - `INFO` → Operações normais  
   - `WARNING` → Erros recuperáveis  
   - `ERROR` → Erros que impactam usuários  
   - `DEBUG` → Logs internos para debug  

### 📌 **2. Configuração do OpenTelemetry**
Adicionar a seguinte configuração no `appsettings.json` para habilitar o OpenTelemetry:
```json
{
  "OpenTelemetry": {
    "Enabled": true,
    "Exporter": "Jaeger",
    "Endpoint": "http://jaeger:14268/api/traces"
  }
}
```

### 📌 **3. Configuração do Prometheus**
Adicionar a seguinte configuração no `docker-compose.yml`:
```yaml
prometheus:
  image: prom/prometheus
  ports:
    - "9090:9090"
  volumes:
    - ./prometheus.yml:/etc/prometheus/prometheus.yml
```

### 📌 **4. Configuração do Serilog para Logs Estruturados**
Adicionar no `Program.cs`:
```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true
    })
    .CreateLogger();
```

---

## 6️⃣ Alertas e Monitoramento

📌 **Alertas podem ser configurados no Azure Monitor para:**  
✅ Erros 500 em excesso em um curto período.  
✅ Requisições com tempo de resposta elevado.  
✅ Falha de conexão com banco de dados ou RabbitMQ.  

📌 **Exemplo de alerta no Azure Monitor:**
- **Condição:** Mais de 10 erros HTTP 500 em 1 minuto.  
- **Ação:** Enviar notificação no Microsoft Teams.  

---

## 7️⃣ Conclusão

A adoção de **monitoramento e observabilidade** garante que qualquer problema seja detectado rapidamente.  

📌 **Próximos passos:**  
✅ Implementar métricas no código usando **Prometheus e Application Insights**.  
✅ Configurar o **OpenTelemetry** para rastreamento distribuído.  
✅ Configurar alertas no **Azure Monitor** para detecção de falhas.  


