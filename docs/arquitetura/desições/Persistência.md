# Persistência

## Contexto

A aplicação necessita persistir tanto os dados do domínio quanto as informações relacionadas à autenticação dos utilizadores.

---

## Problema

Mesmo sem utilizar o ASP.NET Core Identity, os dados de autenticação possuem responsabilidades diferentes das entidades de negócio.

Era necessário definir uma estratégia de persistência simples e organizada.

---

## Decisão

O projeto utilizará um único DbContext responsável por toda a persistência da aplicação.

As entidades de autenticação serão tratadas como parte da infraestrutura, porém persistidas juntamente com as entidades do domínio.

Essa decisão reduz a complexidade da solução sem comprometer a separação lógica entre as responsabilidades.

---

## Benefícios

- Menor quantidade de infraestrutura.
- Menor complexidade de configuração.
- Apenas um conjunto de migrations.
- Transações simplificadas.
- Facilidade para manutenção.

---

## Conclusão

Embora autenticação e domínio possuam responsabilidades distintas, a utilização de um único DbContext mostrou-se suficiente para o contexto atual da aplicação, mantendo a arquitetura simples e coesa.