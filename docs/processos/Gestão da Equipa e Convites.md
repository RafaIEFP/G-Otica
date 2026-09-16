## Objetivo

Descrever o processo de gestão dos utilizadores associados a uma ótica, incluindo:

- consulta da equipa;
    
- envio de convites;
    
- validação e aceitação de convites;
    
- atribuição e alteração de papéis;
    
- desativação de membros;
    
- reativação de associações.
    

A criação, atualização, transferência de propriedade e desativação da própria ótica são tratadas separadamente no processo de **Gestão da Ótica**.

---

## Participantes

- Proprietário da ótica;
    
- Utilizador convidado;
    
- Membro da ótica;
    
- Sistema;
    
- Serviço de email.
    

---

# 1. Associação entre Utilizador e Ótica

A relação entre um utilizador e uma ótica é representada por:

```text
UserOpticalStore
```

Essa entidade mantém:

- `UserId`;
    
- `OpticalStoreId`;
    
- `EntranceDate`;
    
- `Role`;
    
- `IsActive`.
    

Um mesmo utilizador pode pertencer a diferentes óticas e possuir um papel diferente em cada uma.

Exemplo:

```text
Utilizador A
├── Ótica 1 → owner
└── Ótica 2 → manager
```

A associação possui chave composta:

```text
UserId + OpticalStoreId
```

Por esse motivo, não podem existir duas associações diferentes entre o mesmo utilizador e a mesma ótica.

---

# 2. Papéis da Equipa

Os papéis atualmente existentes são:

```text
owner
manager
salesperson
```

---

## Owner

Representa o proprietário da ótica.

O `owner` possui permissões administrativas específicas, incluindo:

- envio de convites;
    
- alteração de papéis;
    
- desativação de membros;
    
- reativação de membros;
    
- atualização da ótica;
    
- transferência de propriedade;
    
- desativação da ótica.
    

Uma ótica possui o seu proprietário definido através de `UserOpticalStore`.

---

## Manager

Representa um membro com papel de gestão.

O papel pode ser atribuído:

- através de convite;
    
- através da alteração do papel de um membro;
    
- ao antigo proprietário após uma transferência de propriedade.
    

---

## Salesperson

Representa um membro da equipa responsável pelas operações comuns de atendimento e vendas.

O papel pode ser atribuído através de convite ou através da alteração de papel de um membro existente.

---

# 3. Consulta da Equipa

Utilizadores com acesso ativo à ótica podem consultar os membros associados à unidade conforme as permissões definidas pela aplicação.

A consulta utiliza `UserOpticalStore` para apresentar informações relacionadas à participação do utilizador naquela ótica.

Podem ser disponibilizadas informações como:

- identificador do utilizador;
    
- nome;
    
- email;
    
- papel;
    
- data de entrada;
    
- estado da associação.
    

O papel apresentado pertence à associação com aquela ótica e não à conta global do utilizador.

---

# 4. Criação de Convite

O ingresso de um novo membro numa ótica é realizado através de um convite.

A criação do convite é uma operação reservada ao proprietário da ótica.

---

## Pré-condições

Para criar um convite:

- o utilizador deve estar autenticado;
    
- a ótica deve estar acessível ao utilizador;
    
- o utilizador deve possuir papel `owner`;
    
- o email do convidado deve ser válido;
    
- o papel solicitado deve ser permitido pelo processo de convite.
    

---

## Papéis Permitidos

Um convite pode atribuir apenas:

```text
manager
salesperson
```

O papel:

```text
owner
```

não pode ser atribuído através de convite.

A propriedade da ótica é tratada exclusivamente pelos processos de criação da ótica e transferência de propriedade.

---

## Verificação de Associação Existente

Antes de criar o convite, o sistema verifica se o email já corresponde a um utilizador que possui uma associação com a ótica.

Essa verificação considera associações:

- ativas;
    
- inativas.
    

Caso já exista `UserOpticalStore`, o convite não deve ser criado.

Exemplo:

```text
Utilizador
    ↓
UserOpticalStore existente
    ↓
IsActive = false
```

Nesse caso:

```text
Novo convite
→ não permitido
```

O processo correto é:

```text
Reativação da associação existente
```

---

## Verificação de Convite Pendente

O sistema também verifica se já existe um convite pendente e ainda válido para o mesmo email e ótica.

Caso exista:

```text
Pending
+
ExpiresAt > momento atual
```

um novo convite não deve ser criado.

Um convite pendente cuja data de expiração já tenha passado não impede a criação de um novo convite.

---

## Fluxo Principal

1. O proprietário inicia o convite.
    
2. Informa:
    
    - email do convidado;
        
    - papel pretendido.
        
3. O sistema normaliza e valida os dados.
    
4. O sistema verifica se já existe uma associação entre o utilizador e a ótica.
    
5. O sistema verifica se já existe um convite pendente e válido.
    
6. É gerado um token de convite.
    
7. Apenas o hash desse token é armazenado no convite.
    
8. O convite é criado com estado:
    

```text
Pending
```

9. São registados:
    
    - ótica;
        
    - email convidado;
        
    - papel;
        
    - utilizador que enviou o convite;
        
    - data de criação;
        
    - data de expiração.
        
10. O token original é utilizado para gerar o link enviado ao convidado.
    
11. O convite é enviado por email.
    

---

# 5. Segurança do Token de Convite

O token enviado ao utilizador não é armazenado diretamente no banco de dados.

O sistema mantém:

```text
Token original
→ enviado ao convidado
```

e:

```text
Hash do token
→ armazenado em Invite.TokenHash
```

Quando o token é posteriormente apresentado, o sistema utiliza-o para localizar e validar o convite correspondente.

Essa abordagem reduz a exposição do token original caso os dados persistidos sejam comprometidos.

---

# 6. Estado e Expiração do Convite

Os estados atualmente existentes são:

```text
Pending
Accepted
```

A expiração **não é representada por um estado próprio**.

Um convite está expirado quando:

```text
Status = Pending
e
ExpiresAt <= momento atual
```

Assim:

```text
Pending + ainda dentro da validade
→ convite válido
```

```text
Pending + prazo ultrapassado
→ convite expirado
```

```text
Accepted
→ convite já utilizado
```

---

# 7. Validação do Convite

Antes da aceitação, o convite pode ser validado através do token recebido.

O sistema deverá verificar:

- existência do convite;
    
- validade do token;
    
- estado `Pending`;
    
- data de expiração.
    

---

## Utilizador ainda não possui Conta

Caso não exista uma conta com o email convidado, a validação indica que é necessário realizar o registo.

Fluxo:

```text
Convite válido
    ↓
Conta inexistente
    ↓
Registo de utilizador necessário
```

Após criar a conta e autenticar-se, o utilizador poderá prosseguir para a aceitação.

---

## Utilizador possui Conta Inativa

Caso exista uma conta com o email convidado, mas ela esteja inativa:

```text
Convite válido
    ↓
Conta inativa
    ↓
Reativação da conta necessária
```

Após reativar a conta e autenticar-se, o utilizador poderá continuar o processo.

---

## Utilizador já possui Conta Ativa

Caso a conta já exista e esteja ativa, não é necessário registo nem reativação.

O utilizador poderá autenticar-se e prosseguir para a aceitação do convite.

---

# 8. Aceitação do Convite

A aceitação transforma um convite válido numa associação efetiva entre o utilizador e a ótica.

---

## Pré-condições

Para aceitar um convite:

- o utilizador deve possuir uma conta ativa;
    
- deve estar autenticado;
    
- o convite deve existir;
    
- o convite deve estar `Pending`;
    
- o convite não pode estar expirado;
    
- o email da conta autenticada deve corresponder ao `GuestEmail` do convite;
    
- não pode já existir uma associação `UserOpticalStore` entre o utilizador e a ótica.
    

---

## Correspondência do Email

O convite pertence ao email para o qual foi enviado.

Por isso:

```text
LoggedUser.Email
```

deve corresponder a:

```text
Invite.GuestEmail
```

Um utilizador autenticado com outro email não pode aceitar o convite.

---

## Nova Verificação de Associação

Mesmo que não existisse uma associação quando o convite foi criado, o sistema verifica novamente essa condição no momento da aceitação.

Isso evita situações como:

```text
Convite criado
    ↓
Utilizador entra na ótica por outro fluxo
    ↓
Convite antigo é utilizado
```

Caso já exista associação ativa ou inativa, o convite não deve criar uma segunda relação.

---

## Fluxo Principal

1. O utilizador abre o convite.
    
2. Autentica-se, caso necessário.
    
3. Solicita a aceitação.
    
4. O sistema valida o token.
    
5. O sistema verifica se o convite está pendente e dentro da validade.
    
6. O sistema verifica se o email autenticado corresponde ao email convidado.
    
7. O sistema verifica novamente se já existe associação com a ótica.
    
8. É criado um novo `UserOpticalStore`.
    
9. A associação recebe:
    
    - `UserId` do utilizador;
        
    - `OpticalStoreId` do convite;
        
    - papel definido no convite;
        
    - estado ativo;
        
    - data de entrada.
        
10. O convite passa para:
    

```text
Accepted
```

11. As alterações são persistidas.
    

---

## Resultado

Antes:

```text
Invite
Status = Pending
```

Depois:

```text
Invite
Status = Accepted
```

e:

```text
User
   ↓
UserOpticalStore
   ↓
OpticalStore
```

O utilizador passa a integrar ativamente a equipa da ótica.

---

# 9. Alteração de Papel

O proprietário pode alterar o papel de um membro da equipa.

---

## Pré-condições

Para alterar o papel:

- o utilizador responsável pela operação deve ser `owner`;
    
- o membro deve possuir uma associação com a ótica;
    
- a associação deve estar ativa;
    
- o novo papel deve ser permitido pelo fluxo comum.
    

---

## Papéis Permitidos

A alteração comum permite utilizar os papéis definidos para membros da equipa, sem transferir a propriedade da ótica.

O papel `owner` não deve ser atribuído através desse fluxo.

Para alterar o proprietário deverá ser utilizado o processo específico de:

```text
Transferência de Propriedade
```

descrito em **Gestão da Ótica**.

---

## Proprietário Atual

O proprietário não deve ter o seu papel removido através de uma alteração comum.

Exemplo não permitido:

```text
owner
  ↓
salesperson
```

Para deixar de ser proprietário, deverá primeiro transferir a propriedade.

Após a transferência, o antigo proprietário torna-se:

```text
manager
```

---

# 10. Desativação de um Membro

A associação de um utilizador com a ótica pode ser desativada sem eliminar o registo.

A operação altera:

```text
UserOpticalStore.IsActive
```

para:

```text
false
```

---

## Pré-condições

Para desativar um membro:

- a operação deve ser realizada pelo proprietário;
    
- o membro deve possuir uma associação ativa com a ótica;
    
- o membro não pode ser o proprietário da ótica.
    

---

## Efeito da Desativação

A desativação afeta apenas a relação daquele utilizador com aquela ótica.

Exemplo:

```text
Utilizador A

Ótica 1 → manager → IsActive = false
Ótica 2 → salesperson → IsActive = true
```

O utilizador continua com acesso à Ótica 2.

A sua conta global também permanece ativa.

---

## Proprietário

O `owner` não pode ser removido da ótica através desse processo.

Caso pretenda deixar de fazer parte da unidade:

1. deverá transferir a propriedade;
    
2. passará a `manager`;
    
3. poderá então ter a sua associação desativada.
    

---

# 11. Reativação de um Membro

Uma associação anteriormente desativada pode ser reativada.

Não deve ser criado um novo `UserOpticalStore`.

O mesmo registo é reutilizado.

---

## Pré-condições

Para reativar uma associação:

- ela deve existir;
    
- deve estar inativa;
    
- a conta `User` correspondente deve estar ativa;
    
- a operação deve ser realizada por um utilizador autorizado.
    

---

## Fluxo Principal

1. O proprietário seleciona o antigo membro.
    
2. O sistema localiza a associação existente.
    
3. Verifica se a conta do utilizador está ativa.
    
4. Altera:
    

```text
IsActive = false
```

para:

```text
IsActive = true
```

5. A associação volta a permitir acesso à ótica.
    

---

## Data de Entrada

A reativação não cria uma nova associação e não altera automaticamente:

```text
EntranceDate
```

A data original de entrada é preservada.

---

# 12. Relação com a Desativação da Conta

Existe uma diferença entre:

```text
User.IsActive
```

e:

```text
UserOpticalStore.IsActive
```

---

## Conta do Utilizador

`User.IsActive` indica se a conta pode ser utilizada no sistema.

Quando uma conta é desativada, as suas associações ativas com óticas também são desativadas.

---

## Associação com uma Ótica

`UserOpticalStore.IsActive` indica se o utilizador possui acesso ativo àquela unidade específica.

Um utilizador pode possuir:

```text
User.IsActive = true
```

e ao mesmo tempo:

```text
Ótica A → IsActive = false
Ótica B → IsActive = true
```

---

## Reativação da Conta

A reativação de `User` não reativa automaticamente as antigas associações com óticas.

Após a conta voltar a ficar ativa, cada associação necessária deverá ser reativada pelo fluxo de gestão da equipa.

---

# 13. Relação com a Desativação da Ótica

Quando uma ótica é desativada:

- `OpticalStore.IsActive` passa para `false`;
    
- todas as associações `UserOpticalStore` ativas daquela ótica também são desativadas.
    

Os registos permanecem armazenados.

Uma eventual reativação futura da ótica deverá definir como as antigas associações serão tratadas.

---

# 14. Fluxos Alternativos

## Utilizador já pertence à Ótica

Caso exista qualquer associação entre o utilizador e a ótica:

```text
Ativa
ou
Inativa
```

um novo convite não deverá ser criado.

Se estiver inativa, deverá ser utilizado o processo de reativação.

---

## Convite Pendente já Existente

Caso exista convite:

```text
Pending
+
ainda válido
```

para o mesmo email e ótica, um novo convite deverá ser rejeitado.

---

## Convite Expirado

Caso:

```text
ExpiresAt <= momento atual
```

o convite não poderá ser utilizado.

Um novo convite poderá ser criado.

---

## Convite já Aceite

Um convite com:

```text
Status = Accepted
```

não poderá ser aceite novamente.

---

## Email Diferente

Caso o utilizador autenticado possua email diferente do `GuestEmail`, a aceitação deverá ser rejeitada.

---

## Papel Owner num Convite

Um convite com papel:

```text
owner
```

não deverá ser permitido.

---

## Reativação com Conta Inativa

Caso:

```text
User.IsActive = false
```

uma associação `UserOpticalStore` não poderá ser reativada.

Primeiro deverá ocorrer a reativação da conta.

---

# 15. Consistência das Operações

Algumas etapas alteram múltiplos registos relacionados e devem preservar a consistência.

---

## Aceitação do Convite

A aceitação envolve:

```text
Invite
Pending → Accepted
```

e:

```text
Criação de UserOpticalStore
```

Essas alterações fazem parte da mesma operação de negócio.

Não deve ser possível obter:

```text
Invite = Accepted
```

sem que a associação tenha sido criada corretamente.

---

## Unicidade da Associação

A combinação:

```text
UserId + OpticalStoreId
```

é única.

Por isso, o sistema deve impedir que fluxos concorrentes ou convites antigos resultem numa segunda associação para o mesmo utilizador e ótica.

---

# 16. Funcionalidades Não Implementadas

O processo atual não possui funcionalidades específicas para:

- cancelar manualmente um convite pendente;
    
- reenviar um convite existente;
    
- alterar o papel definido num convite já criado;
    
- atribuir `owner` através de convite;
    
- manter histórico das alterações de papel;
    
- manter histórico das ativações e desativações de membros.
    

Essas funcionalidades poderão ser incorporadas futuramente caso se tornem necessárias.

---

# Resultado

O processo de Gestão da Equipa e Convites controla a entrada e permanência dos utilizadores dentro das óticas.

O fluxo principal de entrada é:

```text
Owner
   ↓
Cria Invite
   ↓
Email enviado
   ↓
Convite validado
   ↓
Utilizador possui/cria/reativa conta
   ↓
Utilizador autenticado aceita
   ↓
UserOpticalStore criado
   ↓
Novo membro ativo
```

Depois da entrada, o proprietário pode gerir a relação através de:

```text
Alteração de papel
Desativação
Reativação
```

A propriedade da ótica permanece protegida por um fluxo separado de transferência, evitando que o papel `owner` seja atribuído ou removido através das operações comuns de gestão da equipa.

A distinção entre conta (`User`) e associação (`UserOpticalStore`) permite que um mesmo utilizador participe de várias óticas de forma independente e preserve o histórico das suas relações com cada unidade.