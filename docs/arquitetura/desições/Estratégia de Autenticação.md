## Contexto
O sistema necessita autenticar utilizadores de forma segura, mantendo controle sobre os endpoints disponibilizados pela API.

---

## Problema
O método `MapIdentityApi()` disponibiliza diversos endpoints prontos que não serão utilizados na primeira versão da aplicação.

Além disso, reduz o controle sobre o contrato da API.

---

## Alternativas consideradas

### Utilizar `MapIdentityApi()`
Implementação rápida utilizando endpoints fornecidos automaticamente pelo ASP.NET Core Identity.

---

### Implementar autenticação própria utilizando apenas JWT

Maior controle, porém exigindo implementação completa da autenticação.

---

### Utilizar Identity sem `MapIdentityApi()` (Escolhida)
Aproveitar o gerenciamento de utilizadores e hash de senhas do ASP.NET Core Identity.

Implementar manualmente os endpoints de autenticação da aplicação.

---

## Decisão

O projeto utilizará:
- ASP.NET Core Identity
- Endpoints próprios
- JWT
- Refresh Token

A autenticação permanecerá isolada na camada Infrastructure.

---

## Benefícios

- Controle total da API.
- Endpoints enxutos.
- Aproveitamento dos mecanismos de segurança do Identity.
- Flexibilidade para futuras evoluções.

---

## Conclusão
Essa abordagem combina a robustez do ASP.NET Core Identity com a flexibilidade de uma API construída especificamente para as necessidades da aplicação.