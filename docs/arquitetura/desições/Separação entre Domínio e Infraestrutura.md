## Contexto
Durante a modelagem do sistema surgiu a necessidade de definir como representar os utilizadores da aplicação.

O ASP.NET Core Identity disponibiliza a entidade `IdentityUser`, responsável pelas informações relacionadas à autenticação.

Entretanto, essas informações não fazem parte do domínio da ótica.

---

## Problema
Permitir que o domínio permaneça independente da tecnologia utilizada para autenticação.

---

## Decisão
Foi adotada a seguinte abordagem:

Utilizar:
```
ApplicationUser : IdentityUser
```

apenas na camada Infrastructure.

Criar uma entidade própria:
```
Utilizador
```

na camada Domain.

O domínio representa exclusivamente conceitos do negócio.

A autenticação é tratada como um detalhe de infraestrutura.

---

## Benefícios
- Domínio desacoplado do Identity.
- Melhor aderência à Clean Architecture.
- Facilidade para substituir a autenticação futuramente.
- Maior clareza entre negócio e infraestrutura.

---

## Consequências
Durante o cadastro de um utilizador será necessário criar:

- ApplicationUser
- Utilizador

Ambos compartilharão o mesmo identificador lógico.

Essa pequena complexidade adicional foi considerada aceitável diante dos benefícios obtidos.

---

## Conclusão
O domínio permanece independente de qualquer tecnologia de autenticação, preservando sua responsabilidade exclusivamente voltada às regras de negócio.