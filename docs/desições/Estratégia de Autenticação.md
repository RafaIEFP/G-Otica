## Contexto

O G-Otica necessita autenticar utilizadores e proteger os recursos disponibilizados pela API.

Além de identificar o utilizador, a aplicação precisa controlar sessões e permitir que tokens de acesso de curta duração sejam renovados sem exigir um novo login constantemente.

A autenticação também deve permanecer separada das regras de negócio da aplicação.

---

## Problema

Era necessário escolher entre utilizar uma solução completa de gestão de identidade ou implementar apenas os mecanismos necessários para o G-Otica.

O ASP.NET Core Identity disponibiliza diversas funcionalidades prontas, porém introduziria estruturas e comportamentos que não eram necessários para a primeira versão do sistema.

Entre eles poderiam estar:

- modelo próprio de utilizadores;
    
- modelo próprio de roles;
    
- claims;
    
- recuperação de password;
    
- confirmação de email;
    
- diferentes mecanismos de autenticação.
    

No G-Otica, os papéis dos utilizadores também possuem uma característica específica:

```text
User
   ↓
UserOpticalStore
   ↓
Role dentro da ótica
```

Ou seja, o papel não pertence globalmente ao utilizador.

---

## Alternativas Consideradas

### ASP.NET Core Identity

Utilizar a infraestrutura fornecida pelo ASP.NET Core para gerir:

- utilizadores;
    
- passwords;
    
- roles;
    
- claims;
    
- tokens;
    
- autenticação.
    

A principal vantagem seria utilizar uma solução consolidada e extensível.

Entretanto, seria necessário adaptar o modelo do Identity às necessidades específicas do domínio.

---

### Autenticação Própria com JWT

Criar apenas os mecanismos necessários para a aplicação:

- armazenamento seguro de passwords;
    
- login;
    
- access token;
    
- refresh token;
    
- renovação da sessão;
    
- logout;
    
- validação das sessões.
    

Essa foi a alternativa escolhida.

---

## Decisão

O G-Otica utiliza uma implementação própria de autenticação baseada em:

```text
Password Hash
+
JWT Access Token
+
Refresh Token
```

O fluxo geral é:

```text
Email + Password
       ↓
     Login
       ↓
Access Token + Refresh Token
```

---

# Password

A password nunca é armazenada diretamente.

Antes da persistência é aplicado um algoritmo de hash através de:

```text
BCrypt
```

A aplicação trabalha através da abstração:

```text
IPasswordEncryptor
```

permitindo que os casos de uso não dependam diretamente da biblioteca utilizada.

---

# Access Token

Após uma autenticação válida, o sistema gera um JWT.

O token possui informações necessárias para identificar a sessão, incluindo:

```text
NameId
→ identificador do utilizador
```

e:

```text
Jti
→ identificador único do access token
```

O access token possui duração limitada.

---

# Refresh Token

O refresh token permite obter uma nova sessão sem que o utilizador tenha de informar novamente a password.

Cada refresh token está associado a:

```text
UserId
AccessTokenId
ExpiresAt
```

O `AccessTokenId` corresponde ao `Jti` do access token relacionado.

---

# Renovação

Quando o access token necessita ser renovado, o sistema recebe as informações necessárias para localizar e validar a sessão existente.

O fluxo pode ser representado como:

```text
Access Token
+
Refresh Token
      ↓
Validação da sessão
      ↓
Nova autenticação
      ↓
Novo Access Token
+
Novo Refresh Token
```

O token anterior deixa de representar a sessão atual.

---

# Logout

No logout, o refresh token associado à sessão é removido.

Como as políticas de autenticação da aplicação também verificam a existência da sessão persistida, a remoção permite que tokens anteriormente emitidos deixem de ser aceites pelos endpoints protegidos pela aplicação, mesmo que o JWT ainda não tenha atingido a sua expiração criptográfica.

---

# Desativação da Conta

Quando uma conta é desativada:

```text
User.IsActive = false
```

os refresh tokens associados ao utilizador são removidos.

Isso impede a continuação das sessões existentes dentro do mecanismo de autenticação da aplicação.

---

## Separação de Responsabilidades

A implementação permanece distribuída entre as camadas apropriadas.

### Domain

Define entidades e contratos necessários.

### Application

Coordena:

- login;
    
- refresh;
    
- logout;
    
- regras relacionadas à conta.
    

### Infrastructure

Implementa detalhes como:

- BCrypt;
    
- geração de JWT;
    
- manipulação dos tokens;
    
- persistência;
    
- serviços técnicos relacionados à autenticação.
    

### API

Configura:

- `JwtBearer`;
    
- políticas de autorização;
    
- handlers;
    
- acesso ao contexto HTTP.
    

---

## Benefícios

A estratégia escolhida oferece:

- controlo sobre o ciclo de autenticação;
    
- modelo de utilizador compatível com o domínio;
    
- ausência de infraestrutura desnecessária do Identity;
    
- suporte a sessões renováveis;
    
- possibilidade de invalidar sessões;
    
- separação entre autenticação e papéis dentro das óticas;
    
- implementação técnica isolada da lógica principal da aplicação.
    

---

## Consequências

A utilização de autenticação própria também significa que o projeto passa a ser responsável por implementar e manter corretamente aspectos como:

- geração segura de tokens;
    
- armazenamento seguro de passwords;
    
- expiração;
    
- rotação de refresh tokens;
    
- revogação;
    
- validação das sessões.
    

Funcionalidades que seriam fornecidas automaticamente por uma solução como ASP.NET Core Identity precisam ser implementadas explicitamente quando forem necessárias.

---

## Possíveis Evoluções

A estratégia poderá evoluir para incluir funcionalidades como:

- confirmação de email;
    
- recuperação de password;
    
- autenticação multifator;
    
- gestão mais detalhada das sessões;
    
- armazenamento do hash do refresh token em vez do token original;
    
- autenticação através de provedores externos.
    

Essas funcionalidades não fazem parte da decisão inicial e poderão ser incorporadas conforme a necessidade do sistema.

---

## Conclusão

Foi escolhida uma estratégia própria baseada em JWT e Refresh Token porque atende diretamente às necessidades atuais do G-Otica sem introduzir toda a infraestrutura do ASP.NET Core Identity.

A autenticação permanece separada das regras de negócio e pode evoluir de forma localizada conforme novos requisitos surgirem.