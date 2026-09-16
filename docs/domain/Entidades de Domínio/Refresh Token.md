## 1. Contexto

A entidade `RefreshToken` representa a informação necessária para manter e renovar a sessão autenticada de um utilizador.

O sistema utiliza dois tipos de token durante a autenticação:

- Access Token;
    
- Refresh Token.
    

O Access Token é um JWT de curta duração utilizado para autenticar as requisições realizadas à API.

O Refresh Token possui uma duração maior e permite gerar um novo par de tokens sem exigir que o utilizador informe novamente o email e a password.

Além de permitir a renovação do Access Token, o `RefreshToken` também participa do controlo das sessões ativas do sistema.

Por esse motivo, embora não represente uma entidade diretamente relacionada ao negócio de uma ótica, ele é uma entidade de suporte ao processo de autenticação e segurança.

---

## 2. Regras de Domínio

Durante a implementação da autenticação foram definidas as seguintes regras.

#### Um Refresh Token pertence a um utilizador

Todo `RefreshToken` está associado a uma conta através de `UserId`.

O token representa uma sessão autenticada daquele utilizador.

Ele não pertence a uma ótica específica.

A autorização para acesso a uma determinada ótica é tratada posteriormente através das associações existentes em `UtilizadorOtica`.

---

#### O Refresh Token está associado a um Access Token específico

Quando um novo par de tokens é gerado, o Access Token recebe um identificador único através do claim `Jti`.

Esse identificador também é armazenado no `RefreshToken` através de:

```text
AccessTokenId
```

Dessa forma, existe uma associação entre:

```text
Access Token
    ↓
Jti
    ↓
RefreshToken.AccessTokenId
```

Essa relação permite verificar se determinado Access Token ainda corresponde à sessão registada no sistema.

---

#### Apenas um Refresh Token é mantido por utilizador

Na implementação atual, cada utilizador mantém apenas um `RefreshToken` registado.

Antes de armazenar um novo token, os refresh tokens anteriores daquele utilizador são removidos.

Isso ocorre em operações como:

- registo da conta;
    
- login;
    
- renovação dos tokens;
    
- reativação da conta.
    

Consequentemente, um novo login ou uma renovação substitui a sessão anteriormente registada.

---

#### Um novo login invalida a sessão anterior

Como apenas um `RefreshToken` é mantido por utilizador, realizar um novo login remove o token anteriormente associado à conta.

Além disso, o novo par de tokens possui um novo `AccessTokenId`.

Dessa forma, o Access Token da sessão anterior deixa de possuir um `RefreshToken` correspondente no sistema.

Essa regra faz com que a implementação atual trabalhe, na prática, com uma única sessão ativa por utilizador.

---

#### O Refresh Token possui um período de validade

Cada token possui uma data de expiração armazenada em:

```text
ExpiresAt
```

A duração é definida através da configuração:

```text
RefreshTokenValidityDays
```

Um token é considerado expirado quando:

```text
DateTime.UtcNow >= ExpiresAt
```

A propriedade `IsExpired` representa essa condição de forma derivada.

Não existe um estado específico como `Expired`, pois a expiração pode ser determinada diretamente através da data.

---

#### Um Refresh Token expirado não pode renovar a sessão

Durante a renovação dos tokens, o sistema verifica se o `RefreshToken` ainda está dentro do seu período de validade.

Caso esteja expirado, não é permitido gerar um novo par de tokens através dele.

Nesse cenário, o utilizador deverá autenticar-se novamente.

---

#### A renovação exige o Access Token e o Refresh Token

Para renovar a sessão, o sistema recebe:

- o Access Token;
    
- o Refresh Token.
    

O `RefreshToken` é localizado através do valor recebido.

Em seguida, o sistema obtém o identificador `Jti` do Access Token e verifica se corresponde ao `AccessTokenId` armazenado no `RefreshToken`.

Portanto, não basta possuir apenas um Refresh Token válido: ele deve estar associado ao Access Token enviado na mesma operação.

---

#### A renovação realiza rotação do Refresh Token

Quando uma renovação é realizada com sucesso, o mesmo Refresh Token não continua sendo utilizado.

O sistema gera:

- um novo Access Token;
    
- um novo Refresh Token;
    
- um novo `AccessTokenId`.
    

O registo anterior é substituído pelo novo.

O fluxo é:

```text
Access Token atual
+
Refresh Token atual
        ↓
      Validação
        ↓
Novo Access Token
+
Novo Refresh Token
        ↓
Token anterior substituído
```

Essa abordagem impede que o mesmo Refresh Token continue sendo utilizado indefinidamente após uma renovação bem-sucedida.

---

#### O logout remove o Refresh Token

Quando o utilizador realiza logout, o `RefreshToken` associado à sua conta é removido.

Como as operações protegidas também verificam se o identificador do Access Token possui uma sessão correspondente, o Access Token anteriormente emitido deixa de ser aceite pelas políticas de autenticação do sistema.

Dessa forma, não é necessário aguardar apenas pela expiração natural do JWT para encerrar a sessão.

---

#### A desativação da conta remove o Refresh Token

Quando um utilizador desativa a própria conta, os seus refresh tokens são removidos.

Essa operação encerra a sessão atual e impede que os tokens existentes continuem sendo utilizados para renovar o acesso.

A própria conta também passa a ficar inativa.

---

#### A reativação da conta cria uma nova sessão

Quando uma conta é reativada com sucesso, um novo par de tokens é gerado.

Um novo `RefreshToken` é armazenado para representar essa nova sessão.

A reativação, portanto, não recupera tokens utilizados anteriormente.

---

#### O Refresh Token também participa da validação do utilizador autenticado

Nas operações protegidas pela política de utilizador autenticado, o sistema verifica:

- a existência do `UserId` no Access Token;
    
- a existência do `Jti`;
    
- se o utilizador existe;
    
- se a conta está ativa;
    
- se existe um `RefreshToken` associado ao utilizador e ao `Jti` apresentado.
    

Portanto, a autenticação não depende exclusivamente da validade criptográfica do JWT.

O sistema também mantém um estado de sessão no servidor através da entidade `RefreshToken`.

---

## 3. Decisões de Modelagem

#### O RefreshToken é uma entidade separada de Utilizador

Os dados da sessão não são armazenados diretamente na entidade `Utilizador`.

Foi criada uma entidade própria para representar essa responsabilidade.

Atualmente são armazenados:

- `Id`;
    
- `Token`;
    
- `CreatedAt`;
    
- `AccessTokenId`;
    
- `ExpiresAt`;
    
- `UserId`.
    

Essa separação mantém a identidade do utilizador independente do ciclo de vida das suas sessões de autenticação.

---

#### Não existe relação com Ótica

O `RefreshToken` está relacionado ao utilizador, e não a uma unidade específica.

Isso ocorre porque a autenticação identifica primeiro a pessoa que está utilizando o sistema.

O acesso às diferentes óticas é posteriormente determinado através de `UtilizadorOtica` e das respetivas regras de autorização.

---

#### O AccessTokenId corresponde ao Jti do JWT

Cada Access Token possui um identificador único.

Esse identificador é gerado juntamente com o JWT e armazenado no claim:

```text
jti
```

O mesmo valor é armazenado como:

```text
RefreshToken.AccessTokenId
```

Essa decisão permite relacionar a sessão persistida no servidor com o Access Token apresentado pelo cliente.

---

#### Não existe um estado para o Refresh Token

A entidade não possui propriedades como:

```text
IsActive
Status
Revoked
```

A validade temporal é determinada por `ExpiresAt`.

Já a invalidação de uma sessão é realizada removendo o respetivo registo da base de dados.

Assim:

```text
Registo existente
→ sessão registada
```

enquanto:

```text
Registo removido
→ sessão invalidada
```

---

#### A expiração é uma propriedade derivada

`IsExpired` não precisa ser armazenado na base de dados.

O seu valor é calculado através da comparação entre a data atual e `ExpiresAt`.

Isso evita manter duas informações que poderiam entrar em conflito.

---

#### O Refresh Token é gerado de forma aleatória

O token utilizado pelo sistema é gerado através de um gerador criptograficamente seguro.

São utilizados bytes aleatórios, posteriormente convertidos para uma representação adequada para utilização em URLs e requisições HTTP.

O valor gerado não contém informações do utilizador ou da ótica.

---

#### O token é atualmente armazenado diretamente

Na implementação atual, o valor do Refresh Token é armazenado na coluna `Token`.

Isso permite que o repository localize diretamente a sessão através do token recebido durante o processo de renovação.

Diferentemente do token utilizado pelos convites, atualmente não é armazenado apenas um hash do Refresh Token.

---

#### A aplicação mantém uma única sessão por utilizador

Embora a estrutura da base de dados não utilize `UserId` como chave única, o repository remove os tokens anteriores antes de adicionar um novo.

A regra de uma única sessão é, portanto, aplicada pela lógica da aplicação.

Essa decisão simplifica o controlo das sessões e permite invalidar automaticamente sessões anteriores quando um novo login é realizado.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Separa a identidade do utilizador das suas sessões.
    
- Permite renovar Access Tokens sem solicitar novamente as credenciais.
    
- Permite associar cada sessão ao `Jti` de um Access Token específico.
    
- Permite invalidar sessões antes da expiração natural do JWT.
    
- Permite encerrar a sessão durante o logout.
    
- Impede a utilização da sessão após a desativação da conta.
    
- Realiza rotação do Refresh Token durante a renovação.
    
- Mantém a expiração baseada em uma única fonte da verdade através de `ExpiresAt`.
    
- Permite que as políticas de autenticação consultem o estado da sessão no servidor.
    

---

## 5. Possíveis Evoluções

O mecanismo de Refresh Token poderá evoluir futuramente.

Exemplos:

- permitir múltiplas sessões simultâneas por utilizador;
    
- identificar o dispositivo ou navegador associado a cada sessão;
    
- permitir ao utilizador visualizar as suas sessões ativas;
    
- permitir encerrar apenas uma sessão específica;
    
- armazenar a data da última utilização do token;
    
- armazenar informações como endereço IP ou dispositivo;
    
- permitir revogação individual de tokens;
    
- manter histórico de sessões e revogações;
    
- deteção de reutilização de Refresh Tokens já rotacionados.
    

Essas funcionalidades não fazem parte da implementação atual.

---

## 6. Conclusão

A entidade `RefreshToken` representa a sessão persistida utilizada pelo mecanismo de autenticação do sistema.

Ela está associada a um `Utilizador` e ao identificador `Jti` de um Access Token específico, permitindo verificar se determinado JWT ainda corresponde a uma sessão reconhecida pela aplicação.

O Refresh Token possui um período de validade superior ao Access Token e permite gerar um novo par de tokens sem exigir novamente as credenciais do utilizador.

A renovação realiza a rotação dos tokens, enquanto operações como logout e desativação da conta removem o registo da sessão.

Na implementação atual, apenas uma sessão é mantida por utilizador, fazendo com que novos logins e renovações substituam a sessão anterior.

Essa modelagem complementa a autenticação baseada em JWT com um controlo de sessão mantido no servidor, permitindo invalidar acessos antes da expiração natural do Access Token.