# ADR-007: Garantia de Processamento de 50 Requisições por Segundo no Worker Consolidado

## 1️⃣ Contexto

O **Worker Consolidado** é responsável por processar os lançamentos financeiros e calcular os saldos diários. De acordo com os **requisitos não-funcionais**, ele deve ser capaz de **processar 50 requisições por segundo**, garantindo **resiliência, escalabilidade e eficiência no processamento**.

Sem uma estratégia adequada, o Worker pode:
- Ficar sobrecarregado, causando **atrasos** no processamento.
- Não processar a quantidade mínima de **50 mensagens/segundo**.
- Gerar **perda de mensagens** caso a carga exceda sua capacidade.

---

## 2️⃣ Decisão

Para garantir que o **Worker Consolidado** atinja **50 requisições por segundo**, adotaremos a seguinte estratégia:

1. **Paralelismo e Concurrency**  
   - Ajuste de **Prefetch Count** e **Concurrent Message Limit** no RabbitMQ.
   - Utilização de **múltiplos threads** dentro do Worker.

2. **Escalabilidade Horizontal**  
   - Múltiplas **réplicas do Worker** rodando simultaneamente.
   - **AutoScaling no Kubernetes** para aumentar instâncias dinamicamente.

3. **Configuração do RabbitMQ para Alta Performance**  
   - Definir **PrefetchCount** e **Parallel Consumers**.
   - Uso de **Dead Letter Queue (DLQ)** para mensagens não processadas.

4. **Monitoramento e Ajustes Automáticos**  
   - **Métricas no Prometheus + Grafana**.
   - **AutoScaling baseado na fila do RabbitMQ**.

5. **Testes de Performance**  
   - Simulação de carga usando **Locust** para validar a arquitetura.

---

## 3️⃣ Justificativa

| Estratégia | Benefícios |
|------------|-----------|
| **Paralelismo** | Permite processar múltiplas mensagens ao mesmo tempo. |
| **Escalabilidade Horizontal** | Se a carga aumentar, novas instâncias serão criadas automaticamente. |
| **RabbitMQ Otimizado** | Melhora o throughput e evita gargalos. |
| **Monitoramento Ativo** | Permite ajustes em tempo real com AutoScaling. |
| **Testes de Carga** | Garante que o Worker atende ao requisito de 50 req/s. |

---

## 4️⃣ Implementação

### 🔹 **1. Configuração do Worker Consolidado**
No **Worker Consolidado**, devemos configurar o processamento paralelo das mensagens:

```csharp
public class WorkerConsolidado : BackgroundService
{
    private readonly IBusControl _bus;
    private readonly ILogger<WorkerConsolidado> _logger;

    public WorkerConsolidado(IBusControl bus, ILogger<WorkerConsolidado> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker Consolidado iniciado...");

        await _bus.ConnectReceiveEndpoint("fila-consolidado", e =>
        {
            e.PrefetchCount = 20; // Permite processar 20 mensagens simultaneamente
            e.ConcurrentMessageLimit = 10; // Limita a 10 mensagens concorrentes

            e.Handler<LancamentoCriadoEvent>(async context =>
            {
                await ProcessarLancamento(context.Message);
            });
        });

        await Task.Delay(-1, stoppingToken);
    }

    private async Task ProcessarLancamento(LancamentoCriadoEvent evento)
    {
        _logger.LogInformation($"Processando lançamento {evento.Id}");

        // Simulação de processamento
        await Task.Delay(50); // Ajuste conforme necessário para performance

        _logger.LogInformation($"Lançamento {evento.Id} processado!");
    }
}

```
### 🔹 **2. Configuração do RabbitMQ**
No MassTransit, configuramos a concorrência para maximizar a performance:

```csharp
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("fila-consolidado", e =>
        {
            e.PrefetchCount = 20;
            e.ConcurrentMessageLimit = 10;
        });
    });
});
```

### 🔹 **3. Configuração de AutoScaling no Kubernetes**
No Kubernetes, ativamos o Horizontal Pod Autoscaler (HPA) para escalar os Workers conforme a fila do RabbitMQ.

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: worker-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: worker-consolidado
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: External
    external:
      metric:
        name: rabbitmq_queue_length
      target:
        type: AverageValue
        averageValue: 50
```
📌 Isso significa que sempre que houver mais de 50 mensagens na fila, o Kubernetes automaticamente criará mais Workers para processá-las.

### 🔹 **4. Teste de Carga com Locust**
Para validar que conseguimos atingir 50 mensagens por segundo, usamos Locust:

```python
from locust import HttpUser, task, between

class LancamentoTest(HttpUser):
    wait_time = between(1, 2)

    @task
    def enviar_lancamento(self):
        self.client.post("/api/lancamentos", json={
            "valor": 100.0,
            "tipo": "Credito",
            "descricao": "Teste de carga"
        })
```

Rodamos o teste com:

```bash
locust -f locustfile.py --host=http://localhost:5000
```
📌 Isso nos permite validar que a arquitetura suporta 50 mensagens por segundo.


## 5️⃣ Consequências

✅ Positivas

O Worker suportará 50 mensagens/segundo sem perder eventos.
Escalabilidade automática conforme a necessidade.
Monitoramento ativo para ajustar performance dinamicamente.
O sistema permanece resiliente mesmo em alta carga.

❌ Negativas

Aumento de custo com múltiplas instâncias no Kubernetes.
Complexidade adicional na configuração de AutoScaling e RabbitMQ.

## 6️⃣ Decisão Final
Com base na análise, adotamos a abordagem de escalabilidade horizontal, concorrência e auto-scaling para garantir que o Worker Consolidado processe 50 mensagens por segundo.

Essa decisão assegura que o sistema seja resiliente, performático e escalável, atendendo aos requisitos não-funcionais da solução.



