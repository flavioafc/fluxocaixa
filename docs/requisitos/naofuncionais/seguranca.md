# 📌 Documento de Segurança

## 1. Introdução

A segurança da solução de **Controle de Fluxo de Caixa** é fundamental para garantir a **integridade**, **confidencialidade** e **disponibilidade** das informações financeiras. Este documento define as diretrizes de **Autenticação**, **Autorização**, **Proteção de Dados** e **Segurança na Comunicação** que serão implementadas.

---

## 2. Autenticação e Autorização

### 2.1. Autenticação

A aplicação utilizará **OAuth 2.0 e OpenID Connect (OIDC)** como protocolo padrão de autenticação.  
O sistema poderá ser integrado com um **Identity Provider (IdP)**, como:

- **Azure Active Directory (AAD)**
- **Auth0**
- **Keycloak**
- **Okta**
- **IdentityServer**
- Ou outro provedor compatível.

Os **usuários autenticados** receberão um **Access Token** no formato **JWT (JSON Web Token)**, que será utilizado nas chamadas à API.

### 2.2. Autorização

A autorização será baseada em **Claims e Roles**, definidas no JWT.  
Perfis de acesso:

| Perfil  | Permissões |
|---------|------------|
| **Admin** | Registrar, atualizar, cancelar lançamentos e acessar todos os relatórios. |
| **Usuário Comum** | Registrar lançamentos e visualizar seus próprios dados. |

Os endpoints protegidos validarão **escopos e permissões** do JWT antes de conceder acesso.

---

## 3. Segurança na Comunicação

### 3.1. Protocolos Seguros
- Todo o tráfego HTTP será protegido com **TLS 1.2 ou superior**.
- As APIs só aceitarão **requisições HTTPS** (HTTP será bloqueado).
- O **RabbitMQ** será configurado para aceitar apenas **conexões seguras via TLS**.

### 3.2. Proteção contra Ataques
Para mitigar ameaças comuns como **SQL Injection, Cross-Site Scripting (XSS) e Cross-Site Request Forgery (CSRF)**, serão implementadas as seguintes práticas:

✅ **SQL Injection**: Uso de **ORM (EF Core, Dapper) com parâmetros seguros**.  
✅ **XSS**: Sanitização de entradas nos endpoints.  
✅ **CSRF**: Proteção de chamadas sensíveis com **CSRF Tokens**.  
✅ **Rate Limiting**: Limitação de requisições por IP com **AspNetCore Rate Limit**.  
✅ **CORS**: Configuração de regras de **origens confiáveis** para chamadas da API.

---

## 4. Proteção de Dados e Segredos

### 4.1. Armazenamento de Senhas
- O sistema **não armazenará senhas diretamente**. Apenas **tokens de autenticação criptografados** ou referências a usuários autenticados via IdP.
- Caso um login local seja necessário, senhas serão armazenadas usando **PBKDF2**, **Argon2**, ou **BCrypt**.

### 4.2. Gerenciamento de Segredos
Os segredos sensíveis, como:
- **Strings de conexão**
- **Chaves de criptografia**
- **Credenciais de terceiros**
- **Chaves JWT**
- **Credenciais do RabbitMQ**

Serão armazenados em um **cofre seguro**, como:
- **Azure Key Vault**
- **AWS Secrets Manager**
- **Google Secret Manager**
- **HashiCorp Vault**

### 4.3. Criptografia de Dados
- O banco de dados utilizará **Transparent Data Encryption (TDE)** para proteger os dados em repouso.
- Dados sensíveis (ex.: CPF, número de cartão) serão armazenados de forma **criptografada no banco**.

---

## 5. Monitoramento e Auditoria

### 5.1. Logs e Rastreamento de Eventos
- Logs estruturados serão coletados via **Serilog**, **Application Insights**, ou **Elastic Stack (ELK)**.
- Logs críticos incluirão tentativas de login, falhas de autenticação e ações administrativas.

### 5.2. Auditoria de Segurança
- Qualquer alteração crítica (ex.: criação, atualização, exclusão de lançamentos) será **auditada** e armazenada para fins de rastreabilidade.

### 5.3. Alertas e Notificações
- Alertas serão configurados para detectar **padrões anômalos** como múltiplas tentativas de login falhas.
- Integração com **Prometheus + Grafana**, **Azure Monitor**, ou **AWS CloudWatch** para geração de métricas de segurança.

---

## 6. Segurança no Deploy e Infraestrutura

- **Docker Security**: As imagens Docker serão **assinadas** para evitar uso de versões comprometidas.
- **CI/CD Seguro**: Credenciais de produção nunca ficarão em repositórios; serão acessadas via **cofre seguro**.
- **Permissões Mínimas**: Serviços só terão acesso ao que for estritamente necessário (princípio do **Least Privilege**).

---

## 7. Conclusão

Este documento define os principais controles de segurança que garantem:

✅ **Autenticação segura com OAuth 2.0/OpenID e JWT**.  
✅ **Proteção dos dados** via criptografia e gerenciamento de segredos.  
✅ **Resistência a ataques** como SQL Injection e XSS.  
✅ **Monitoramento e auditoria** para rastrear ações suspeitas.  
✅ **Segurança no deploy e na infraestrutura** com Docker e CI/CD seguro.

