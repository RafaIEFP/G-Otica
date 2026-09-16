## Contexto

O G-Otica possui diferentes responsabilidades, como:

- exposição de endpoints HTTP;
    
- execução de casos de uso;
    
- aplicação de regras de negócio;
    
- persistência de dados;
    
- autenticação e autorização;
    
- integração com serviços externos;
    
- definição dos contratos utilizados pela API.
    

À medida que o sistema cresce, manter todas essas responsabilidades no mesmo projeto aumentaria o acoplamento e dificultaria a manutenção.

---

## Problema

Era necessário definir uma estrutura que permitisse:

- separar regras de negócio de detalhes técnicos;
    
- reduzir o acoplamento entre componentes;
    
- facilitar testes;
    
- permitir a evolução independente das diferentes partes da aplicação;
    
- tornar clara a responsabilidade de cada projeto.
    

Uma organização baseada apenas em pastas dentro de um único projeto não ofereceria uma separação suficientemente explícita entre essas responsabilidades.

---

## Alternativas Consideradas

### Projeto Único

Manter toda a aplicação dentro de um único projeto ASP.NET Core, utilizando apenas pastas para organizar:

- controllers;
    
- entidades;
    
- serviços;
    
- repositórios;
    
- validações.
    

Essa abordagem seria mais simples inicialmente, porém permitiria maior acoplamento entre componentes e dificultaria a imposição de limites arquiteturais.

---

### Arquitetura em Camadas

Separar a aplicação em projetos distintos, cada um responsável por uma parte específica do sistema.

Essa foi a alternativa escolhida.

---

## Decisão

O G-Otica utiliza uma arquitetura em camadas composta por:

```text
API
Communication
Application
Domain
Infrastructure
Exceptions
```

Cada camada possui uma responsabilidade específica.

---

# API

A camada `API` representa o ponto de entrada HTTP da aplicação.

É responsável por:

- controllers e endpoints;
    
- configuração da aplicação;
    
- autenticação e autorização no pipeline HTTP;
    
- políticas e attributes;
    
- tratamento da comunicação HTTP;
    
- configuração da injeção de dependências;
    
- inicialização da aplicação.
    

A API não deve conter regras de negócio.

Seu papel principal é receber uma requisição, encaminhá-la para o caso de uso correspondente e devolver a resposta apropriada.

De forma simplificada:

```text
HTTP Request
     ↓
    API
     ↓
Application
     ↓
HTTP Response
```

---

# Communication

A camada `Communication` contém os contratos utilizados na comunicação com a aplicação.

Exemplos:

- requests;
    
- responses;
    
- enums utilizados pelos contratos externos.
    

Essa camada evita que entidades do domínio sejam expostas diretamente através da API.

Assim:

```text
Cliente HTTP
     ↓
Request
     ↓
Application
     ↓
Response
     ↓
Cliente HTTP
```

As entidades internas permanecem separadas dos modelos enviados ou recebidos externamente.

---

# Application

A camada `Application` contém os casos de uso do sistema.

É responsável por coordenar as operações necessárias para executar uma ação de negócio.

Exemplos:

```text
RegisterSaleUseCase
RegisterPurchaseUseCase
AcceptInviteUseCase
AdjustProductStockUseCase
```

Um caso de uso pode coordenar:

- validações;
    
- entidades;
    
- repositórios;
    
- serviços;
    
- transações;
    
- mapeamentos.
    

A camada `Application` determina **como uma operação é executada**, utilizando abstrações definidas pelas camadas internas.

Ela não deve implementar detalhes específicos de persistência ou infraestrutura.

---

# Domain

A camada `Domain` contém os principais conceitos internos da aplicação.

Inclui:

- entidades;
    
- enums;
    
- contratos;
    
- abstrações relacionadas ao domínio.
    

Exemplos:

```text
Sale
Product
Client
Purchase
Prescription
IProductReadOnlyRepository
ISaleWriteOnlyRepository
```

O domínio não deve depender de detalhes concretos como:

- Entity Framework Core;
    
- PostgreSQL;
    
- JWT;
    
- MailKit;
    
- BCrypt.
    

Esses detalhes são implementados externamente.

---

# Infrastructure

A camada `Infrastructure` contém as implementações técnicas necessárias para suportar a aplicação.

Atualmente inclui responsabilidades relacionadas a:

- Entity Framework Core;
    
- PostgreSQL através do Npgsql;
    
- implementação dos repositórios;
    
- `GOticaDbContext`;
    
- FluentMigrator;
    
- geração e validação de JWT;
    
- refresh tokens;
    
- BCrypt;
    
- envio de emails através do MailKit;
    
- integração com serviços externos.
    

Exemplo:

```text
Domain
IProductReadOnlyRepository
        ↑
        │ implementado por
        │
Infrastructure
ProductRepository
```

Dessa forma, os casos de uso trabalham com abstrações e não precisam conhecer o mecanismo concreto utilizado para persistência ou serviços externos.

Ele também é responsável por lidar com tudo aquilo que está "fora" do scopo da aplicação, ele lida com coisas externas.

---

# Exceptions

A camada `Exceptions` centraliza exceções específicas utilizadas pela aplicação.

Exemplos:

```text
ConflictException
UnauthorizedException
ErrorOnValidationException
```

Isso permite representar falhas de negócio ou validação de forma consistente sem espalhar definições de exceções pelos diferentes projetos.

O tratamento HTTP dessas exceções permanece responsabilidade da camada de entrada da aplicação.

---

## Fluxo entre as Camadas

De forma simplificada:

```text
        ┌─────────────────┐
        │       API       │
        └────────┬────────┘
                 │
        ┌────────▼────────┐
        │  Communication  │
        └─────────────────┘
                 │
        ┌────────▼────────┐
        │   Application   │
        └────────┬────────┘
                 │
        ┌────────▼────────┐
        │     Domain      │
        └─────────────────┘

        Infrastructure
              ↑
     implementa abstrações
       utilizadas acima
```

A intenção não é eliminar completamente dependências entre projetos, mas garantir que cada responsabilidade permaneça no local apropriado.

---

## Benefícios

A arquitetura escolhida oferece:

- separação clara de responsabilidades;
    
- menor acoplamento entre regras de negócio e infraestrutura;
    
- facilidade para substituir implementações técnicas;
    
- maior testabilidade dos casos de uso;
    
- melhor organização do código;
    
- proteção do modelo de domínio;
    
- contratos externos separados das entidades internas;
    
- maior facilidade para evolução do projeto.
    

---

## Consequências

A separação também introduz alguma complexidade adicional.

Uma funcionalidade simples pode envolver vários projetos.

Por exemplo:

```text
Request
   ↓
Controller
   ↓
UseCase
   ↓
Repository Interface
   ↓
Repository Implementation
   ↓
Database
```

Para o G-Otica, esse custo foi considerado aceitável porque o sistema possui múltiplos contextos e tende a crescer ao longo do desenvolvimento.

---

## Conclusão

Foi adotada uma arquitetura em camadas para manter as responsabilidades da aplicação explicitamente separadas.

A estrutura utilizada não procura seguir rigidamente uma arquitetura específica, mas aplica princípios como separação de responsabilidades, baixo acoplamento e dependência de abstrações.

Essa organização permite que regras de negócio, comunicação HTTP e detalhes técnicos evoluam com menor impacto entre si.