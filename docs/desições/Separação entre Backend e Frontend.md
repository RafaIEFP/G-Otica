## Contexto

O G-Otica necessita disponibilizar uma aplicação web para interação com os utilizadores e uma camada responsável pelas regras de negócio e acesso aos dados.

Existem diferentes formas de estruturar essas partes.

Uma aplicação ASP.NET Core MVC poderia manter interface e backend dentro da mesma aplicação.

Outra alternativa seria manter uma API independente e utilizar uma aplicação frontend separada para consumir os seus endpoints.

---

## Problema

Era necessário definir como a interface do utilizador deveria comunicar com as funcionalidades do sistema.

Manter frontend e backend dentro da mesma aplicação reduziria a quantidade inicial de projetos e simplificaria algumas configurações.

Entretanto, também criaria maior dependência entre a interface web e a implementação do backend.

O G-Otica foi pensado para que as funcionalidades do sistema possam ser expostas através de uma API independente da tecnologia utilizada na interface.

---

## Alternativas Consideradas

### ASP.NET Core MVC

Nessa abordagem:

```text
Browser
   ↓
ASP.NET Core MVC
├── Controllers
├── Views
├── Application
└── Backend
```

A mesma aplicação seria responsável por:

- receber requisições;
    
- executar as operações do sistema;
    
- renderizar HTML;
    
- disponibilizar a interface.
    

### Backend Web API + Frontend Separado

Nessa abordagem:

```text
Frontend
    ↓ HTTP
Web API
    ↓
Application
    ↓
Domain / Infrastructure
```

O frontend utiliza a API através de contratos HTTP.

Essa foi a estratégia escolhida.

---

## Decisão

O G-Otica utilizará backend e frontend como aplicações separadas.

O backend é implementado através de:

```text
ASP.NET Core Web API
```

e concentra:

- regras de negócio;
    
- autenticação e autorização;
    
- persistência;
    
- validações;
    
- acesso aos dados;
    
- operações da aplicação.
    

O frontend será responsável pela interface apresentada ao utilizador e consumirá a Web API através de HTTP.

---

# Backend

O backend não depende da interface gráfica para executar as suas funcionalidades.

Os recursos são disponibilizados através de endpoints.

Exemplo:

```text
Frontend
    ↓
POST /sales
    ↓
API
    ↓
RegisterSaleUseCase
```

A resposta é devolvida utilizando os contratos definidos pela aplicação.

---

# Frontend

O frontend é tratado como um cliente da API.

A sua responsabilidade é:

- apresentar a interface;
    
- recolher dados do utilizador;
    
- enviar requests;
    
- interpretar responses;
    
- apresentar erros e resultados;
    
- gerir o estado necessário à experiência do utilizador.
    

As regras críticas de negócio permanecem no backend.

---

## Comunicação

A comunicação ocorre através de HTTP.

De forma simplificada:

```text
Frontend
   │
   │ HTTP Request
   ▼
Backend API
   │
   │ HTTP Response
   ▼
Frontend
```

Isso significa que estar dentro da mesma solution não transforma as duas aplicações numa única aplicação em tempo de execução.

Mesmo que os projetos estejam organizados como:

```text
G-Otica.slnx

src/
├── Backend/
└── Frontend/
```

frontend e backend continuam sendo aplicações distintas.

---

## CORS

Como a comunicação ocorre entre aplicações distintas, o navegador considera a origem de cada uma.

Durante o desenvolvimento, é comum que sejam executadas em endereços diferentes.

Exemplo:

```text
Frontend
https://localhost:7001
```

```text
Backend
https://localhost:7002
```

Nesse cenário, o backend deve configurar CORS para permitir as origens autorizadas.

Estarem dentro da mesma solution não elimina essa necessidade.

Em produção, a necessidade e configuração exatas dependem da forma como as aplicações forem publicadas e dos domínios utilizados.

---

## Contratos

A separação reforça a utilização de contratos próprios para comunicação.

Por exemplo:

```text
RequestRegisterSale
        ↓
      API
        ↓
Application
        ↓
Sale
```

e:

```text
Sale
  ↓
Application
  ↓
ResponseRegisterSale
  ↓
Frontend
```

O frontend não deve depender diretamente das entidades persistidas pelo backend.

---

## Autenticação

Como o frontend é um cliente da API, a autenticação também ocorre através dos endpoints disponibilizados pelo backend.

De forma simplificada:

```text
Frontend
   ↓
Login
   ↓
Backend
   ↓
Access Token + Refresh Token
   ↓
Frontend
```

As chamadas protegidas utilizam os mecanismos de autenticação definidos pela API.

---

## Benefícios

A separação permite:

- independência entre interface e backend;
    
- evolução separada das aplicações;
    
- possibilidade de substituir o frontend sem alterar as regras de negócio;
    
- reutilização da API por outros clientes;
    
- contratos explícitos através de HTTP;
    
- maior isolamento das responsabilidades;
    
- desenvolvimento e testes independentes.
    

No futuro, a mesma API poderia ser utilizada, por exemplo, por:

```text
Aplicação Web
Aplicação Mobile
Outro Cliente
```

sem duplicar as regras de negócio.

---

## Consequências

A estratégia também introduz responsabilidades adicionais.

É necessário tratar aspectos como:

- comunicação HTTP;
    
- serialização;
    
- autenticação entre cliente e API;
    
- CORS;
    
- tratamento de erros HTTP;
    
- configuração dos endereços da API;
    
- publicação de duas aplicações.
    

Em uma solução MVC tradicional, parte dessa complexidade não existiria porque interface e backend estariam executando dentro da mesma aplicação.

---

## Organização na Solution

A separação física dos projetos também ajuda a representar essa decisão.

Uma estrutura possível é:

```text
src/
├── Backend/
│   ├── GOtica.API
│   ├── GOtica.Application
│   ├── GOtica.Domain
│   ├── GOtica.Infrastructure
│   ├── GOtica.Communication
│   └── GOtica.Exceptions
│
└── Frontend/
```

A solution funciona apenas como organização dos projetos.

Ela não altera a separação entre as aplicações durante a execução.

---

## Conclusão

Foi escolhida a separação entre backend e frontend para que a lógica do G-Otica permaneça independente da interface utilizada.

A ASP.NET Core Web API funciona como a fonte das operações e regras do sistema, enquanto o frontend atua como cliente dessa API.

Essa abordagem introduz maior complexidade de comunicação quando comparada a uma aplicação MVC única, mas oferece maior independência, reutilização e flexibilidade para a evolução futura do projeto.