# 📊 Estimativa de Custos com Infraestrutura e Licenças

## 1️⃣ Introdução

Este documento apresenta a **estimativa de custos** da infraestrutura utilizada na solução **Fluxo de Caixa Diário**, considerando serviços de **hospedagem, banco de dados, mensageria, cache, segurança, monitoramento e licenciamento**.

Os custos são calculados com base em **instâncias específicas** da Azure, prevendo diferentes **níveis de carga** e **cenários de pico**.

---

## 2️⃣ Componentes da Solução e Custos Estimados

### 🏗 **Infraestrutura Aplicacional**
| Componente                  | Descrição | Hospedagem | Tipo/Instância | Custo Estimado (mês) |
|-----------------------------|-----------|------------|----------------|----------------------|
| **API de Controle de Lançamentos** | Serviço em .NET 8 que registra lançamentos e publica eventos. | Azure App Service | P1V3 (2 instâncias) | **US$ 200 - US$ 400** |
| **Worker Consolidado** | Serviço em .NET 8 que processa eventos e calcula saldos. | Azure Kubernetes Service (AKS) | 2vCPU / 4GB RAM (2 pods) | **US$ 150 - US$ 300** |
| **Serviço de Relatórios** | API em .NET 8 que fornece dados sintéticos e analíticos. | Azure App Service | P1V3 (1 instância) | **US$ 100 - US$ 200** |
| **Azure API Management (APIM)** | Gateway de APIs para segurança, rate limiting e caching. | APIM Standard | S1 | **US$ 600 - US$ 900** |

---

### 🗄 **Armazenamento e Banco de Dados**
| Componente                  | Descrição | Tipo | Instância | Custo Estimado (mês) |
|-----------------------------|-----------|------|-----------|----------------------|
| **Banco Transacional (Lançamentos)** | Armazena lançamentos financeiros detalhados. | Azure SQL | **S4 (200 DTUs, 250GB)** | **US$ 500 - US$ 700** |
| **Banco Analítico (Saldos Consolidados)** | Armazena dados agregados e otimizados para consulta. | Azure SQL | **S3 (100 DTUs, 250GB)** | **US$ 300 - US$ 500** |

---

### 📩 **Mensageria e Comunicação**
| Componente | Descrição | Tipo | Instância | Custo Estimado (mês) |
|------------|-----------|------|-----------|----------------------|
| **RabbitMQ** | Comunicação assíncrona entre serviços. | AKS / Container | 2vCPU / 4GB RAM | **US$ 50 - US$ 100** |

---

### ⚡ **Cache e Desempenho**
| Componente | Descrição | Tipo | Instância | Custo Estimado (mês) |
|------------|-----------|------|-----------|----------------------|
| **Azure Cache for Redis** | Melhoria de desempenho para consultas frequentes. | Cache Premium | P1 (6GB) | **US$ 200 - US$ 300** |

---

### 🔐 **Segurança e Gerenciamento de Segredos**
| Componente | Descrição | Tipo | Instância | Custo Estimado (mês) |
|------------|-----------|------|-----------|----------------------|
| **Azure Key Vault** | Armazena credenciais, certificados e segredos. | Cofre de Segredos | Standard | **US$ 10 - US$ 20** |
| **Azure Entra ID (Antigo Azure AD)** | Gerenciamento de identidade e autenticação. | Standard | P1 | **US$ 60 - US$ 100** |

---

### 📈 **Monitoramento e Observabilidade**
| Componente | Descrição | Tipo | Instância | Custo Estimado (mês) |
|------------|-----------|------|-----------|----------------------|
| **Azure Monitor + Log Analytics** | Coleta logs, métricas e análise de desempenho. | Monitoramento | Standard | **US$ 100 - US$ 200** |
| **Prometheus + Grafana OSS (Open Source)** | Visualização de métricas | AKS / Container | 2vCPU / 4GB RAM | **US$ 50 - US$ 100** |

---

### 💻 **Licenciamento**
| Componente | Descrição | Tipo | Custo Estimado (mês) |
|------------|-----------|------|----------------------|
| **Windows Server (se aplicável)** | Licença para servidores Windows. | Licenciamento | **US$ 20 - US$ 50 por VM** |
| **SQL Server Enterprise (se aplicável)** | Licença para SQL Server Enterprise. | Licenciamento | **US$ 200 - US$ 700** |
| **Grafana Enterprise (Opcional)** | Caso a versão OSS não seja suficiente, pode ser necessário licenciamento. | Licenciamento | **US$ 50 - US$ 300** |
| **Linux (se aplicável)** | Caso seja necessário suporte para instâncias pagas no Azure. | Licenciamento | **US$ 10 - US$ 30 por VM** |

---

## 3️⃣ **Cenários de Carga e Estimativas de Custos**

A seguir, apresentamos **diferentes cenários** para a solução, considerando **tráfego normal, aumento gradual e picos de alta demanda**.

### 📌 **Cenário 1: Operação Regular**
- **Requisições**: 500.000/mês (~17.000/dia)
- **Uso normal de banco de dados e cache**
- **RabbitMQ com carga moderada**
- **APIM processando requisições sem necessidade de escalabilidade**
- **Estimativa de Custo**: **US$ 2.500 - US$ 4.000/mês**

### 📌 **Cenário 2: Escala Média**
- **Requisições**: 2.000.000/mês (~67.000/dia)
- **Escalonamento horizontal (2-3 instâncias de API e Workers)**
- **Banco transacional precisa de mais DTUs**
- **RabbitMQ processando maior volume de mensagens**
- **APIM lidando com maior carga, podendo exigir escalonamento**
- **Estimativa de Custo**: **US$ 5.000 - US$ 7.000/mês**

### 📌 **Cenário 3: Pico de Tráfego**
- **Requisições**: 10.000.000/mês (~333.000/dia)
- **Escalonamento máximo (5-7 instâncias de API e Workers)**
- **RabbitMQ com filas de alta prioridade**
- **Azure SQL precisa ser escalado para instâncias superiores (P1, P2)**
- **APIM precisará de uma camada adicional para balanceamento**
- **Estimativa de Custo**: **US$ 10.000 - US$ 15.000/mês**

---

## 4️⃣ **Considerações Finais e Otimizações**

✅ **Escalabilidade Horizontal**: APIs e Workers podem escalar conforme demanda.  
✅ **Uso de Caching**: Reduz chamadas ao banco de dados, otimizando custos.  
✅ **Adoção de Mensageria**: RabbitMQ reduz o acoplamento e melhora resiliência.  
✅ **Infraestrutura como Código (IaC)**: Deploy automatizado e redução de custos.  
✅ **APIM substitui a necessidade de Ocelot** para segurança, rate limit e roteamento.

📌 **Possíveis Reduções de Custo**:
- Utilizar **planos menores do Azure SQL em horários de menor demanda**.
- Configurar **APIM para autoescalar somente quando necessário**.
- Avaliar **substituir Redis Cache Premium por uma opção mais barata, dependendo da carga real**.
