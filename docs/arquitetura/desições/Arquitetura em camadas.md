# Arquitetura em Camadas

## Contexto
O sistema precisava de uma organização que favorecesse manutenção, testes e evolução do código ao longo do tempo.

---

## Problema
Misturar regras de negócio com detalhes técnicos aumenta o acoplamento e dificulta futuras alterações.

---

## Decisão

Foi adotada uma arquitetura em camadas composta por:
- API
- Application
- Domain
- Infrastructure
- Exceptions

Cada camada possui responsabilidades bem definidas.

---

## Responsabilidades

### API
Responsável pela exposição dos endpoints HTTP.

Não contém regras de negócio.

---

### Application
Orquestra os casos de uso da aplicação.

Coordena entidades, validações e serviços necessários para executar cada operação.

---

### Domain
Representa o núcleo do sistema.

Contém entidades, regras de negócio, enums e contratos do domínio.

Não conhece banco de dados, autenticação ou qualquer tecnologia específica.

---

### Infrastructure
Implementa detalhes técnicos da aplicação.

Exemplos:
- Entity Framework Core
- ASP.NET Core Identity
- Repositórios
- JWT
- Serviços externos

---

### Exceptions
Centraliza exceções específicas da aplicação.

---

## Benefícios
- Separação de responsabilidades.
- Baixo acoplamento.
- Facilidade para testes.
- Melhor organização do código.
- Evolução mais segura da aplicação.

---

## Conclusão
A arquitetura em camadas fornece uma base simples, organizada e suficientemente flexível para a evolução do sistema.