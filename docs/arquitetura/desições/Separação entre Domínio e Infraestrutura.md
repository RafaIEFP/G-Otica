# Separação entre Domínio e Infraestrutura

## Contexto

Durante a definição da arquitetura surgiu a necessidade de estabelecer quais responsabilidades pertencem ao domínio da aplicação e quais devem permanecer como detalhes de infraestrutura.

A autenticação foi um dos principais pontos dessa discussão.

---

## Problema

Evitar que conceitos técnicos contaminem o domínio da aplicação.

O domínio deve representar exclusivamente regras de negócio, enquanto mecanismos técnicos devem permanecer isolados na infraestrutura.

---

## Decisão

Foi adotada a seguinte divisão de responsabilidades:

### Domain

Responsável pelos conceitos do negócio.

Exemplos:

- Utilizador
- Cliente
- Produto
- Venda
- Compra

---

### Infrastructure

Responsável pelos detalhes técnicos.

Exemplos:

- Persistência
- Hash de senhas
- Geração de JWT
- Refresh Tokens
- Criptografia

O domínio não possui conhecimento sobre como a autenticação é implementada.

---

## Benefícios

- Domínio independente de tecnologias específicas.
- Menor acoplamento entre regras de negócio e infraestrutura.
- Facilidade para evoluir ou substituir a estratégia de autenticação.
- Melhor aderência à arquitetura em camadas adotada pelo projeto.

---

## Conclusão

A autenticação passa a ser tratada como um serviço de infraestrutura, enquanto o domínio permanece responsável apenas pela representação e pelas regras de negócio dos utilizadores.