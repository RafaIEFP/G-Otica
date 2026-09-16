# G-Otica

## Sobre o projeto

O **G-Otica** é uma **API de gestão para óticas**, desenvolvida utilizando **ASP.NET Core e .NET 10**. O principal objetivo do projeto é centralizar os principais processos de uma ótica, permitindo gerir utilizadores, clientes, receitas, produtos, stock, fornecedores, compras, vendas, lentes, tratamentos e pagamentos.

A aplicação utiliza uma **arquitetura em camadas**, procurando manter as regras de negócio separadas dos detalhes técnicos da aplicação. O backend é disponibilizado através de uma **API REST**, permitindo que diferentes interfaces possam consumir os seus recursos através de HTTP.

Os dados são armazenados numa base de dados **PostgreSQL**, com **Entity Framework Core** utilizado no acesso aos dados e **FluentMigrator** no controlo da estrutura da base de dados. O **FluentValidation** é utilizado para validação das requisições e o **Mapster** auxilia no mapeamento entre os modelos da aplicação. A autenticação é realizada através de **JWT e Refresh Tokens**, com as passwords protegidas utilizando **BCrypt**.

A API também disponibiliza documentação interativa através do **Scalar/OpenAPI**, facilitando a visualização e o teste dos endpoints durante o desenvolvimento.

### Features

- **Gestão de Óticas e Utilizadores**: Gestão das óticas, membros, papéis e convites.
- **Gestão de Clientes e Receitas**: Registo de clientes e acompanhamento do seu histórico de prescrições.
- **Gestão de Produtos e Stock**: Controlo dos produtos, quantidades disponíveis e histórico de movimentações.
- **Gestão de Fornecedores e Compras**: Registo de fornecedores e aquisição de produtos com atualização automática do stock.
- **Gestão de Vendas**: Vendas com produtos, lentes personalizadas, tratamentos, descontos e acompanhamento do processo de produção.
- **Gestão de Pagamentos**: Controlo dos valores recebidos e dos pagamentos pendentes associados às vendas.
- **Autenticação e Autorização**: Autenticação através de JWT e Refresh Tokens, com controlo de acesso às diferentes óticas.
- **API REST com Scalar/OpenAPI**: Documentação interativa para consulta e teste dos endpoints.

### Construído com

![badge-dotnet]
![badge-csharp]
![badge-postgresql]
![badge-efcore]
![badge-visual-studio]

## Documentação

A documentação detalhada do projeto encontra-se na pasta [`docs`](./docs), incluindo:

- regras e entidades do domínio;
- processos de negócio;
- requisitos;
- diagramas entidade-relacionamento;
- decisões arquiteturais.

## Getting Started

Para obter uma cópia local do projeto, siga os passos abaixo.

### Requisitos

- Visual Studio 2022+, JetBrains Rider ou Visual Studio Code
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL ou acesso a uma base de dados PostgreSQL
- Git

### Configuração

O projeto utiliza **.NET User Secrets** para armazenar configurações locais e informações sensíveis, evitando que credenciais sejam adicionadas ao repositório.

Antes de executar a API, configure os seguintes valores no projeto `GOtica.API`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<CONNECTION_STRING>"
  },

  "Settings": {
    "Jwt": {
      "ExpiresMinutes": 60,
      "SigningKey": "<SIGNING_KEY>",
      "Issuer": "<ISSUER>",
      "Audience": "<AUDIENCE>"
    },
    "RefreshToken": {
      "RefreshTokenValidityDays": 7
    },
    "InviteToken": {
      "InviteValidityDays": 3
    }
  },

  "SmtpSettings": {
    "Server": "<SMTP_SERVER>",
    "Port": 587,
    "Login": "<SMTP_LOGIN>",
    "Password": "<SMTP_PASSWORD>",
    "SenderName": "<SENDER_NAME>",
    "SenderEmail": "<SENDER_EMAIL>"
  }
}
```

Os User Secrets podem ser configurados através do Visual Studio ou da CLI do .NET:

```sh
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<CONNECTION_STRING>" --project src/Backend/GOtica.API
```

> Nunca adicione passwords, connection strings, Signing Keys ou outras credenciais diretamente ao repositório.


### Instalação

1. Clone o repositório:

    ```sh
    git clone https://github.com/RafaIEFP/G-Otica.git
    ```

2. Entre na pasta do projeto:

    ```sh
    cd G-Otica
    ```

3. Restaure as dependências:

    ```sh
    dotnet restore
    ```

4. Configure as informações necessárias da aplicação, como a conexão com a base de dados e as credenciais utilizadas pelos serviços externos.

5. Execute a API:

    ```sh
    dotnet run --project src/Backend/GOtica.API
    ```

6. Aceda à documentação interativa da API através do **Scalar**.

---

[badge-dotnet]: https://img.shields.io/badge/.NET%2010-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-csharp]: https://img.shields.io/badge/C%23-512BD4?logo=csharp&logoColor=fff&style=for-the-badge
[badge-postgresql]: https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=fff&style=for-the-badge
[badge-efcore]: https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-visual-studio]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge
