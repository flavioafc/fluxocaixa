# ADR-003: Decisão Sobre Cache para Relatórios Diários

## Contexto

O sistema precisa suportar **consultas frequentes** de relatórios de saldos consolidados, especialmente durante picos de uso (50 requisições por segundo). Consultar diretamente o banco de dados analítico pode resultar em:

1. **Latência alta** para o usuário.
2. **Sobrecarga no banco analítico**.

## Problema

Sem um mecanismo de cache:

- As consultas frequentes impactariam negativamente a performance do banco analítico.
- Usuários experimentariam alta latência ao acessar relatórios.
- Custos de infraestrutura poderiam aumentar devido à necessidade de escalar o banco analítico.

## Decisão

Adotar **Azure Cache for Redis** como solução de cache distribuído para:

- Armazenar resultados de saldos consolidados frequentemente acessados.
- Reduzir a latência das consultas ao evitar acessos repetitivos ao banco analítico.

## Justificativa

1. **Performance**:
   - O Redis é otimizado para leitura e escrita em memória, garantindo baixa latência.

2. **Desempenho em Picos**:
   - Consultas de relatórios frequentes são atendidas diretamente pelo cache, aliviando o banco analítico.

3. **Custo**:
   - Reduz a necessidade de escalar o banco analítico, diminuindo custos de infraestrutura.

4. **Escalabilidade**:
   - Redis é altamente escalável e pode ser configurado para suportar grandes volumes de tráfego.

5. **Fácil Integração**:
   - O Azure Cache for Redis é compatível com .NET e outras ferramentas usadas no sistema.

## Consequências

### Positivas:

- Redução significativa na latência de consultas frequentes.
- Menor carga no banco analítico, melhorando o desempenho geral do sistema.
- Escalabilidade para lidar com picos de carga.

### Negativas:

- Introduz a necessidade de sincronizar dados entre o banco analítico e o cache.
- Custo adicional do Azure Cache for Redis.

## Alternativas Consideradas

1. **Sem Cache**:
   - Simples de implementar.
   - Desvantagem: Latência alta e impacto na performance do banco analítico.

2. **Cache em Memória Local**:
   - Baixo custo e simples.
   - Desvantagem: Não escalável para ambientes distribuídos.

3. **Armazenamento em Disco (Table Storage)**:
   - Reduz custos.
   - Desvantagem: Latência maior comparada ao Redis.

## Decisão Final

Usar o **Azure Cache for Redis** para armazenar os resultados de saldos consolidados e otimizar consultas frequentes.
