# ADR-009: Decisão Sobre o Uso do Azure API Management (APIM)

## Contexto
O sistema **Fluxo de Caixa Diário** precisa garantir **segurança, escalabilidade e padronização** na exposição de suas APIs. Atualmente, os serviços estão sendo consumidos diretamente pelos clientes e integrações, o que gera desafios como:

1. **Falta de um ponto central de gerenciamento** para autenticação e autorização.
2. **Dificuldade de aplicação de rate limits e caching** para melhorar a performance.
3. **Exposição direta dos endpoints internos** sem um gateway intermediário.
4. **Ausência de logging e monitoramento centralizados** sobre o consumo das APIs.

Para resolver esses problemas, decidimos adotar o **Azure API Management (APIM)** como um API Gateway.

---

## Decisão
O **Azure API Management (APIM)** será utilizado como API Gateway para todas as requisições externas ao sistema.

1. Todas as chamadas para **API de Controle de Lançamentos** e **API de Relatórios** passarão pelo APIM.
2. O APIM fornecerá **autenticação centralizada** usando **Azure AD e JWT**.
3. Implementação de **Rate Limiting** para evitar sobrecarga nas APIs.
4. **Caching de respostas** para otimizar chamadas frequentes aos relatórios.
5. **Log centralizado e monitoramento** via **Azure Monitor e Application Insights**.

O **Worker Consolidado** e o **RabbitMQ** continuarão funcionando internamente sem passar pelo APIM, pois são serviços de backend sem exposição pública.

---

## Justificativa
A adoção do **Azure API Management (APIM)** traz os seguintes benefícios:

✅ **Segurança Aprimorada**: Camada de proteção antes das APIs, controlando acessos e permissões.
✅ **Gestão Centralizada**: Controle unificado de endpoints, versões de API e políticas de acesso.
✅ **Escalabilidade**: Capacidade de gerenciar alto volume de chamadas de forma eficiente.
✅ **Observabilidade e Monitoramento**: Logs detalhados de consumo de APIs.
✅ **Rate Limiting**: Controle de acessos abusivos e proteção contra DDoS.
✅ **Caching para Relatórios**: Redução de carga no banco de dados analítico.

---

## Alternativas Consideradas

1. **Uso do Ocelot como API Gateway**
   - 🚫 **Desvantagens**: Manutenção manual, menos integração com Azure Monitor e escalabilidade limitada.

2. **Exposição direta das APIs sem um gateway**
   - 🚫 **Desvantagens**: Falta de controle central, maior risco de segurança e dificuldade de monitoramento.

---

## Consequências da Decisão

### Positivas:
- ✅ **Melhoria na segurança e governança das APIs**.
- ✅ **Facilidade na escalabilidade** das requisições com caching e rate limiting.
- ✅ **Maior monitoramento e logging** através do Azure Monitor.

### Negativas:
- ❌ **Custo Adicional** do APIM conforme o volume de chamadas.
- ❌ **Possível latência extra** devido ao processamento intermediário pelo APIM.

---

## Decisão Final
O **Azure API Management (APIM)** foi escolhido para gerenciar as APIs, garantindo **segurança, escalabilidade e monitoramento**. A solução atenderá os requisitos de arquitetura e facilitará a gestão dos serviços expostos.

📄 **Leia mais sobre a Infraestrutura e Configuração do APIM em** [Terraform - APIM](../devops/Terraform.md)

