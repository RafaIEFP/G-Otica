# Estratégia de Autenticação

## Contexto

O sistema necessita autenticar utilizadores de forma segura, mantendo controle total sobre o fluxo de autenticação e sobre os endpoints disponibilizados pela API.

Além disso, a autenticação deve permanecer desacoplada das regras de negócio, permitindo sua evolução sem impactar o domínio da aplicação.

---

## Problema

O ASP.NET Core Identity oferece uma solução completa para autenticação, porém adiciona funcionalidades e estruturas que não fazem parte das necessidades da primeira versão do projeto.

Sua utilização introduziria maior complexidade arquitetural e operacional sem benefícios proporcionais ao contexto atual da aplicação.

---

## Alternativas consideradas

### Utilizar ASP.NET Core Identity

Implementação baseada na infraestrutura fornecida pelo ASP.NET Core, incluindo gerenciamento de utilizadores, roles, claims e demais funcionalidades prontas.

---

### Implementar autenticação própria utilizando JWT (Escolhida)

Implementar apenas os mecanismos necessários para a aplicação:

- Hash de senhas
- Login
- JWT
- Refresh Token
- Controle de sessões

Mantendo total controle sobre o comportamento da autenticação.

---

## Decisão

O projeto utilizará uma implementação própria de autenticação composta por:

- JWT
- Refresh Token
- Hash seguro de senhas
- Endpoints próprios

Os mecanismos criptográficos e de geração de tokens permanecerão isolados na camada Infrastructure, enquanto a camada Application coordenará o processo de autenticação.

---

## Benefícios

- Menor complexidade.
- Controle total sobre a autenticação.
- Apenas funcionalidades realmente necessárias.
- Facilidade para compreender e evoluir a solução.
- Possibilidade de substituir a estratégia de autenticação futuramente sem impacto significativo na arquitetura.

---

## Conclusão

Optou-se por uma implementação própria de autenticação por oferecer uma solução mais simples e alinhada às necessidades atuais do projeto, preservando a flexibilidade para futuras evoluções.