## Contexto

A aplicação possui dois conjuntos distintos de dados:
- Dados do domínio.
- Dados relacionados à autenticação.

---

## Problema
Misturar essas responsabilidades no mesmo contexto aumenta o acoplamento e dificulta a manutenção.

---

## Decisão
A persistência foi dividida em dois DbContexts.

### ApplicationDbContext
Responsável pelas entidades do domínio.

Exemplos:
- Cliente
- Venda
- Compra
- Produto
- Receita

---

### IdentityDbContext
Responsável pelas entidades do ASP.NET Core Identity.

Exemplos:
- Users
- Roles
- Claims
- Tokens

---

## Benefícios
- Separação de responsabilidades.
- Migrations independentes.
- Melhor organização do banco.
- Facilidade para manutenção.

---

## Conclusão

Cada DbContext possui uma responsabilidade específica, mantendo a infraestrutura organizada e alinhada aos princípios arquiteturais do projeto.