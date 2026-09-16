## 1. Contexto

O `ConviteUtilizador` representa um convite enviado para que uma pessoa passe a fazer parte de uma ótica.

O convite é utilizado durante o processo de associação de um utilizador a uma ótica, permitindo que o proprietário da unidade convide uma pessoa para integrar a sua equipa.

O convite não representa, por si só, uma associação efetiva entre o utilizador e a ótica.

A associação somente é criada após a aceitação do convite, através da entidade `UtilizadorOtica`.

Por esse motivo, o `ConviteUtilizador` representa uma etapa intermediária entre o envio do convite e a criação da associação efetiva.

---

## 2. Regras de Domínio

Durante o levantamento de requisitos foram identificadas as seguintes regras do negócio.

#### Um convite pertence a uma ótica

Todo convite é criado dentro do contexto de uma ótica.

A ótica determina a unidade à qual o convidado será associado caso aceite o convite.

Por exemplo:

- Ótica Centro;
    
- Ótica Shopping;
    
- Ótica Norte.
    

Um mesmo utilizador pode receber convites de diferentes óticas, desde que ainda não possua um vínculo com essas unidades.

---

#### Apenas o proprietário pode criar convites

Na versão atual do sistema, a criação de convites é uma operação restrita ao proprietário (`owner`) da ótica.

Gerentes e vendedores não podem convidar diretamente novos utilizadores para a unidade.

Essa regra mantém a gestão da equipa sob responsabilidade do proprietário.

---

#### Um convite é direcionado para um endereço de email

O convite é enviado para o endereço de email informado pelo proprietário.

O email identifica a pessoa convidada durante todo o processo.

Antes da criação, o endereço é normalizado e validado.

Na aceitação do convite, o email do utilizador autenticado deve corresponder ao `GuestEmail` armazenado no convite.

Caso contrário, o convite não poderá ser aceite por esse utilizador.

---

#### O destinatário pode ainda não possuir uma conta

A pessoa convidada não precisa possuir uma conta no sistema no momento em que o convite é criado.

Essa decisão permite que uma ótica convide pessoas que ainda não utilizam o sistema.

Durante a validação do convite, o sistema verifica a existência de uma conta associada ao email.

Caso nenhuma conta exista, a resposta indica:

```json
{
  "requiresRegistration": true,
  "requiresReactivation": false
}
```

O utilizador deverá criar a sua conta antes de poder concluir a aceitação.

---

#### O destinatário pode possuir uma conta inativa

Caso já exista uma conta associada ao email do convite, mas essa conta esteja desativada, o sistema identifica que será necessária a sua reativação.

Nesse caso, a validação do convite indica:

```json
{
  "requiresRegistration": false,
  "requiresReactivation": true
}
```

A conta deverá ser reativada antes que o utilizador possa concluir normalmente o processo de aceitação.

---

#### A aceitação do convite exige uma conta autenticada

A validação inicial do token pode ser realizada sem autenticação, permitindo identificar se o destinatário precisa criar ou reativar a sua conta.

A aceitação, entretanto, exige que o utilizador esteja autenticado.

Além disso, o email da conta autenticada deve corresponder ao email para o qual o convite foi emitido.

Dessa forma, possuir o token do convite não é suficiente para associar qualquer conta à ótica.

---

#### A aceitação do convite cria a associação com a ótica

O convite não cria imediatamente um registo em `UtilizadorOtica`.

A associação somente é criada após:

- validação do token;
    
- autenticação do utilizador;
    
- confirmação de que o email da conta corresponde ao destinatário;
    
- verificação de que ainda não existe um vínculo entre o utilizador e a ótica.
    

O fluxo é:

```text
Convite
   ↓
Validação do token
   ↓
Conta existente / criação ou reativação da conta
   ↓
Autenticação do utilizador
   ↓
Aceitação
   ↓
UtilizadorOtica
```

Após a aceitação, o vínculo é criado com a função definida no convite e a respetiva data de entrada.

---

#### O convite define a função do utilizador

O convite possui a função que será atribuída ao utilizador caso seja aceite.

As funções permitidas nesse fluxo são:

- `manager`;
    
- `salesperson`.
    

A função `owner` não pode ser atribuída através de convite.

A propriedade da ótica possui fluxos específicos:

- criação da ótica;
    
- transferência de propriedade.
    

---

#### Um convite possui um período de validade

Cada convite possui uma data de criação (`CreatedAt`) e uma data de expiração (`ExpiresAt`).

A duração do convite é definida pela configuração do sistema.

Após a data de expiração, o token deixa de ser considerado válido e o convite não pode mais ser validado ou aceite.

Essa regra impede que convites antigos permaneçam utilizáveis indefinidamente.

---

#### Um convite possui um estado

O convite possui um estado que representa o seu ciclo de vida.

Os estados atualmente definidos são:

- `Pending`;
    
- `Accepted`;
    
- `Expired`.
    

Todo convite é criado inicialmente como `Pending`.

Quando o convite é aceite com sucesso, passa para `Accepted`.

A expiração é determinada através de `ExpiresAt`. Na implementação atual, convites cuja data de validade já terminou deixam de ser considerados válidos, mesmo que o estado persistido ainda permaneça como `Pending`.

O estado `Expired` está definido no domínio, porém atualmente não existe um processo responsável por atualizar automaticamente o estado de um convite expirado.

---

#### O convite possui um token de segurança

Cada convite possui um token aleatório utilizado para validar o processo.

O token original é enviado ao destinatário através do link do convite.

A base de dados não armazena diretamente esse token.

Em vez disso, é armazenado apenas o seu hash em `TokenHash`.

Quando o token é posteriormente recebido pelo sistema, o seu hash é novamente calculado e utilizado para localizar um convite válido.

Essa abordagem evita que o acesso à tabela de convites revele diretamente os tokens utilizados nos links enviados aos utilizadores.

---

#### O convite identifica quem o criou

O convite mantém uma referência ao utilizador responsável pela sua criação através de `InvitedByUserId`.

Essa informação permite identificar quem realizou o convite e preservar a autoria da operação.

---

#### Não devem existir convites pendentes válidos duplicados

Não é permitido criar múltiplos convites pendentes e ainda válidos para o mesmo email dentro da mesma ótica.

Caso já exista um convite com:

- o mesmo email;
    
- a mesma ótica;
    
- estado `Pending`;
    
- data de expiração ainda não atingida;
    

um novo convite é rejeitado.

Um convite pendente que já tenha expirado não impede a criação de um novo convite.

---

#### Um utilizador que já possui vínculo com a ótica não deve ser convidado novamente

Caso o utilizador identificado pelo email já possua um registo em `UtilizadorOtica` para aquela unidade, um novo convite não deve ser criado.

Essa regra é válida tanto para vínculos ativos quanto para vínculos inativos.

Se o vínculo estiver ativo, o utilizador já pertence atualmente à equipa.

Se o vínculo estiver inativo, deve ser utilizado o fluxo específico de reativação de `UtilizadorOtica`, em vez de criar uma nova associação através de convite.

Essa regra também evita a tentativa de criar uma segunda associação para o mesmo par:

```text
UserId + OpticalStoreId
```

que representa a chave composta de `UtilizadorOtica`.

---

## 3. Decisões de Modelagem

#### O ConviteUtilizador é uma entidade própria

Foi criada uma entidade específica para representar o processo de convite.

Ela não é representada diretamente através de `UtilizadorOtica`.

Essa separação permite diferenciar:

```text
Convite pendente
```

de:

```text
Associação efetiva
```

O `ConviteUtilizador` representa uma intenção de associação, enquanto `UtilizadorOtica` representa um vínculo já estabelecido.

---

#### O convite não possui `UserId` do destinatário

Não é armazenado um `UserId` referente à pessoa convidada.

Isso ocorre porque ela pode ainda não possuir uma conta no sistema no momento da criação do convite.

O email é utilizado para identificar o destinatário.

Após a criação ou identificação da conta e a aceitação do convite, o respetivo `UserId` passa a fazer parte da associação criada em `UtilizadorOtica`.

---

#### O convite possui referência ao utilizador que o criou

Embora o destinatário não seja identificado através de `UserId`, o utilizador responsável pela criação do convite é armazenado através de:

```text
InvitedByUserId
```

Essa informação representa a autoria da operação.

---

#### O convite armazena o hash do token

O token original não é armazenado diretamente na base de dados.

O sistema gera um token aleatório e calcula o seu hash.

O funcionamento é:

```text
Token original
    ↓
Enviado por email ao convidado

Hash do token
    ↓
Armazenado no Invite
```

Durante a validação ou aceitação:

```text
Token recebido
    ↓
Hash calculado novamente
    ↓
Comparação com TokenHash
```

Essa decisão reduz a exposição do token original caso os dados armazenados sejam comprometidos.

---

#### Apenas convites pendentes e dentro da validade são considerados válidos

A existência do registo na base de dados não significa que o convite ainda possa ser utilizado.

Para ser considerado válido, o convite deve:

- possuir estado `Pending`;
    
- ainda não ter atingido `ExpiresAt`;
    
- possuir um token correspondente ao hash armazenado.
    

Convites aceites ou expirados não podem ser utilizados novamente.

---

#### O convite não é apagado após ser aceite

Após a aceitação, o convite permanece armazenado e o seu estado passa para `Accepted`.

Essa decisão permite preservar informações como:

- email convidado;
    
- função oferecida;
    
- utilizador que realizou o convite;
    
- ótica correspondente;
    
- data de criação;
    
- data originalmente definida para expiração.
    

Atualmente não é armazenada uma data específica de aceitação.

---

#### A ausência de um utilizador não é considerada uma falha do convite

Caso o email ainda não esteja associado a uma conta, o convite continua sendo válido.

O endpoint de validação informa essa situação através de:

```json
{
  "requiresRegistration": true
}
```

Da mesma forma, caso exista uma conta inativa, é informado:

```json
{
  "requiresReactivation": true
}
```

Essas situações representam etapas possíveis do fluxo de convite, e não a inexistência ou invalidade do convite.

---

#### O método de entrega atual é o email

Os convites são atualmente enviados por email.

A entidade `ConviteUtilizador` não é responsável pelo mecanismo de envio.

Ela representa apenas os dados e o estado do convite.

Essa separação permite que o mecanismo de entrega evolua futuramente sem alterar a responsabilidade da entidade.

---

#### O `owner` não é atribuído através de convite

A função `owner` possui regras próprias dentro do domínio.

O convite é utilizado para atribuir somente:

```text
manager
salesperson
```

A propriedade da ótica é determinada através:

- da criação da ótica;
    
- da transferência explícita de propriedade.
    

Essa decisão impede que o fluxo comum de entrada de colaboradores seja utilizado para alterar a propriedade da unidade.

---

#### Um vínculo inativo não é recriado através de convite

Como `UtilizadorOtica` utiliza `UserId + OpticalStoreId` como chave composta, uma associação continua existindo mesmo quando está inativa.

Por esse motivo, o convite não deve criar uma nova associação para um antigo colaborador.

Nesse cenário, deve ser utilizado o fluxo específico de reativação do vínculo existente.

Essa decisão preserva a identidade e o histórico da associação entre o utilizador e a ótica.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Separa convites de associações efetivas.
    
- Permite convidar pessoas que ainda não possuem conta.
    
- Permite identificar quando uma conta precisa ser reativada.
    
- Garante que apenas a conta correspondente ao email convidado possa aceitar o convite.
    
- Permite controlar a validade dos convites.
    
- Evita reutilização de convites já aceites ou expirados.
    
- Evita múltiplos convites pendentes válidos para o mesmo destinatário e ótica.
    
- Evita duplicação de vínculos em `UtilizadorOtica`.
    
- Permite identificar quem realizou cada convite.
    
- Preserva o histórico dos convites aceites.
    
- Protege o token através do armazenamento apenas do seu hash.
    
- Mantém separadas as responsabilidades de convite, autenticação e associação à ótica.
    
- Permite evoluir futuramente o mecanismo de envio sem alterar a entidade.
    

---

## 5. Possíveis Evoluções

Embora o fluxo atual já permita criar, validar e aceitar convites, algumas funcionalidades poderão ser adicionadas futuramente.

Exemplos:

- reenvio de convite;
    
- cancelamento de convite;
    
- listagem de convites pendentes;
    
- consulta ao histórico de convites;
    
- registo da data de aceitação;
    
- identificação explícita do utilizador que aceitou o convite;
    
- atualização automática do estado para `Expired`;
    
- diferentes períodos de validade conforme configuração ou contexto;
    
- envio de convites através de SMS;
    
- envio de convites através de WhatsApp;
    
- notificações ao proprietário quando um convite for aceite.
    

Essas funcionalidades não fazem parte do fluxo atual.

---

## 6. Conclusão

O `ConviteUtilizador` representa o processo utilizado para convidar uma pessoa a integrar uma ótica.

Ele funciona como uma etapa intermediária entre a intenção de adicionar um colaborador e a criação efetiva da associação representada por `UtilizadorOtica`.

O convite pertence a uma ótica, identifica o email do destinatário, define a função que será atribuída e mantém a referência ao proprietário responsável pela sua criação.

O processo utiliza um token de segurança cujo valor original não é armazenado na base de dados, sendo mantido apenas o respetivo hash.

A validação permite identificar se o destinatário precisa criar ou reativar a sua conta, enquanto a aceitação exige um utilizador autenticado cujo email corresponda ao convite.

A associação à ótica somente é criada depois da aceitação e apenas quando ainda não existe um vínculo entre o utilizador e a unidade.

Essa separação mantém as responsabilidades bem definidas, protege o processo de associação e evita a criação duplicada de vínculos entre utilizadores e óticas.