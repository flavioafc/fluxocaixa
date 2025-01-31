
# 🔐 Arquitetura de Segurança

## 1️⃣ Introdução
A **segurança** é um dos pilares fundamentais desta solução. O sistema adota uma **abordagem de defesa em profundidade**, garantindo **autenticação forte, comunicação segura e proteção de dados**.

Este documento descreve os principais mecanismos de segurança implementados, incluindo:
- **Autenticação e autorização** baseadas em OAuth 2.0 + JWT.
- **Proteção de comunicação** via TLS 1.2+ em todas as APIs e banco de dados.
- **Gerenciamento seguro de credenciais** utilizando Azure Key Vault.
- **Monitoramento de segurança** para detecção de atividades suspeitas.

---

## 2️⃣ Visão Geral da Segurança

A segurança da solução é estruturada da seguinte forma:

### 🛡 **Camadas de Segurança**
1. **Autenticação e Autorização** → O **Azure AD** (ou outro IdP) gerencia autenticação via **OAuth 2.0 e OpenID Connect**.
2. **API Gateway** → Valida **JWT Tokens**, protege contra ataques de força bruta e faz **rate limiting**.
3. **Autorização nos Serviços** → As APIs utilizam **JWT** para validar permissões e aplicar controle de acesso.
4. **Segurança na Comunicação** → **TLS 1.2+ obrigatório** para todas as APIs, RabbitMQ e banco de dados.
5. **Proteção de Dados** → Dados sensíveis são armazenados com **criptografia (TDE e AES-256)**.
6. **Gerenciamento de Segredos** → Credenciais e chaves são armazenadas no **Azure Key Vault**.
7. **Monitoramento de Segurança** → Logs estruturados são enviados para detecção de ameaças.

---

## 3️⃣ Diagrama de Segurança

O seguinte diagrama ilustra a **arquitetura de segurança** da solução:

![Diagrama de Segurança](../images/diagrama-de-seguranca.png)

🔹 **Explicação do diagrama**:
- **Usuário se autentica via OAuth 2.0/OpenID Connect** e recebe um **JWT Token**.
- **O API Gateway valida o token** antes de encaminhar requisições para as APIs.
- **A API de Lançamentos interage com o RabbitMQ** para processamento assíncrono.
- **A API de Relatórios consulta dados consolidados** de maneira segura.
- **Todos os acessos a banco são protegidos** por criptografia TDE e segredos armazenados no **Key Vault**.
- **Monitoramento e logs estruturados** garantem auditoria de eventos.

---

## 4️⃣ Mecanismos de Segurança

### 🔑 **Autenticação e Autorização**
- **OAuth 2.0 e OpenID Connect** via **Azure AD / Keycloak / Auth0**.
- Cada API valida o **JWT Token** antes de conceder acesso.
- Perfis de acesso:
  - **Admin** → Pode gerenciar lançamentos e acessar relatórios completos.
  - **Usuário Comum** → Apenas gerencia seus próprios lançamentos.

### 🔐 **Proteção de Dados e Segredos**
- **TDE (Transparent Data Encryption)** ativado no banco de dados.
- **Chaves de criptografia** armazenadas no **Azure Key Vault**.
- **Senhas nunca são armazenadas** → Apenas hashes seguros (Argon2/PBKDF2).

### 🛡 **Segurança na Comunicação**
- **Todas as APIs exigem HTTPS** (`TLS 1.2+`).
- **RabbitMQ protegido com TLS** e usuários autenticados.
- **Banco de Dados SQL Server com TLS** ativado para impedir tráfego em texto puro.

### 📊 **Monitoramento de Segurança**
- Logs estruturados via **Serilog** e **Azure Monitor**.
- Detecção de acessos suspeitos com alertas automáticos.

---

## 5️⃣ Conclusão
A arquitetura de segurança da solução segue padrões modernos para garantir **autenticação forte, proteção de dados e comunicação segura**.
