## 1. Contexto

O `ConviteUtilizador` representa um convite enviado para que uma pessoa passe a fazer parte de uma ótica.

O convite é utilizado durante o processo de associação de um utilizador a uma ótica, permitindo que um proprietário ou utilizador autorizado convide uma pessoa para integrar a equipa da unidade.

O convite não representa, por si só, uma associação efetiva entre o utilizador e a ótica.

A associação somente será criada após a aceitação do convite, através da entidade `UtilizadorOtica`.

Por esse motivo, o `ConviteUtilizador` representa uma etapa intermediária entre o envio do convite e a criação da associação efetiva.

---

## 2. Regras de Domínio

Durante o levantamento de requisitos foram identificadas as seguintes regras do negócio.

#### Um convite pertence a uma ótica

Todo convite é criado dentro do contexto de uma ótica.

A ótica é responsável por determinar a qual unidade o convidado será associado caso aceite o convite.

Por exemplo:

- Ótica Centro;
- Ótica Shopping;
- Ótica Norte.

Um mesmo utilizador pode receber convites de diferentes óticas.

---

#### Um convite é direcionado para um endereço de email

O convite será enviado para um endereço de email informado pelo utilizador que realiza o convite.

O email será utilizado para identificar o destinatário durante o processo de aceitação.

---

#### O destinatário pode ainda não possuir uma conta

A pessoa convidada não precisa possuir uma conta no sistema no momento em que o convite é criado.

Essa decisão permite que uma ótica convide pessoas que ainda não utilizam o sistema.

Durante a aceitação, o sistema verificará se já existe um utilizador associado ao email do convite.

Caso não exista, o fluxo deverá indicar que é necessário realizar o registo da conta antes de concluir a associação à ótica.

---

#### A aceitação do convite cria a associação com a ótica

O convite não cria imediatamente um registo em `UtilizadorOtica`.

A associação somente será criada após a aceitação do convite e a conclusão das validações necessárias.

O fluxo será:
Convite
   ↓
Aceitação
   ↓
Utilizador existente ou registado
   ↓
UtilizadorOtica

Dessa forma, apenas pessoas que concluíram o processo de aceitação passam a possuir acesso à ótica.

---

#### O convite define o cargo do utilizador

O convite possui o cargo que será atribuído ao utilizador caso seja aceite.

Exemplos:

- Employee;
- Manager;
- Accountant.

O cargo `Owner` não será atribuído através do fluxo normal de convite.

A responsabilidade de `Owner` possui fluxos específicos, como a criação da ótica e a transferência de propriedade.

---

#### Um convite possui um período de validade

Os convites possuem uma data de expiração.

Após o período definido, o convite não poderá mais ser aceite.

Essa regra impede que convites antigos permaneçam indefinidamente válidos.

---

#### Um convite possui um estado

O convite possui um estado que representa o seu ciclo de vida.

Os estados inicialmente definidos são:

- `Pending`;
- `Accepted`;
- `Expired`;
- `Cancelled`.

Um convite inicia como `Pending`.

Após ser aceite, passa para `Accepted`.

Caso ultrapasse a data de validade sem ser aceite, passa para `Expired`.

Caso seja cancelado antes da aceitação, passa para `Cancelled`.

---

#### O convite possui um token de segurança

Cada convite possui um token utilizado para validar o link recebido pelo destinatário.

O token funciona como uma credencial secreta associada ao convite e impede que o identificador do convite, por si só, seja suficiente para realizar a aceitação.

O token original será enviado através do link do convite, enquanto apenas o seu hash será armazenado na base de dados.

---

#### O convite identifica quem o criou

O convite mantém uma referência para o utilizador responsável pela sua criação.

Essa informação permite identificar quem realizou o convite e poderá ser utilizada futuramente para auditoria e histórico das operações.

---

#### Não devem existir convites pendentes duplicados

Não deve ser permitido manter múltiplos convites pendentes para o mesmo email dentro da mesma ótica.

Caso já exista um convite pendente, deverá ser utilizado um fluxo específico para reenvio ou o novo convite deverá ser rejeitado.

---

#### Um utilizador que já pertence à ótica não deve ser convidado novamente

Caso o email informado pertença a um utilizador que já possui uma associação ativa com a ótica, um novo convite não deverá ser criado.

A pessoa já pertence à equipa da unidade e não necessita de um novo convite.

---

## 3. Decisões de Modelagem

#### O ConviteUtilizador será uma entidade própria

Foi criada uma entidade específica para representar o processo de convite.

Ela não será representada diretamente através de `UtilizadorOtica`.

Essa separação permite diferenciar:

Convite pendente

de:

Associação efetiva

O `ConviteUtilizador` representa uma intenção de associação, enquanto `UtilizadorOtica` representa uma associação já estabelecida.

---

#### O convite não possuirá inicialmente um UserId

Não será armazenado um `UserId` no convite.

Isso ocorre porque a pessoa convidada pode ainda não possuir uma conta no sistema.

O email será utilizado para identificar o destinatário durante o processo de aceitação.

Após a existência ou criação da conta, será criada a associação através de `UtilizadorOtica`.

---

#### O convite armazenará o hash do token

O token original não será armazenado diretamente na base de dados.

O sistema irá gerar um token aleatório, enviá-lo ao destinatário através do link do convite e armazenar apenas o seu hash.

Dessa forma, mesmo que os dados da tabela de convites sejam expostos, o token original não estará disponível diretamente.

---

#### O convite não será apagado após ser aceite

Após a aceitação, o convite permanecerá armazenado com o estado `Accepted`.

Essa decisão permite preservar o histórico das operações realizadas.

O registo poderá futuramente ser utilizado para:

- Auditoria;
- Histórico de convites;
- Identificação de quem realizou o convite;
- Identificação da data de criação;
- Identificação da data de aceitação.

---

#### A ausência de um utilizador não será considerada uma exceção

Caso o email do convite não esteja associado a um utilizador existente, isso não representa uma falha do convite.

O sistema deverá informar que é necessário realizar o registo da conta para continuar o processo.

Esse resultado poderá ser representado através de uma resposta contendo:

{

    "requiresRegistration": true

}

Dessa forma, a ausência de uma conta é tratada como uma etapa válida do fluxo de negócio.

---

#### O método de entrega será inicialmente o email

Na primeira versão, os convites serão enviados por email.

A entidade `ConviteUtilizador` não ficará diretamente responsável pelo mecanismo de envio.

Essa separação permite que futuramente sejam adicionados outros meios de comunicação, como SMS ou WhatsApp, sem alterar a responsabilidade principal da entidade.

---

#### O Owner não será atribuído através de convite

O cargo de `Owner` possui regras específicas dentro do domínio.

A propriedade da ótica será definida através dos fluxos de criação da ótica e de transferência de propriedade.

O sistema utilizará o convite para adicionar membros à equipa, e não para transferir a propriedade da unidade.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Separa convites de associações efetivas.
- Permite convidar pessoas que ainda não possuem conta.
- Permite controlar a validade dos convites.
- Permite controlar o ciclo de vida do convite.
- Evita a criação de associações antes da aceitação.
- Permite identificar quem realizou cada convite.
- Permite manter histórico dos convites realizados.
- Protege o token do convite através do armazenamento do seu hash.
- Facilita futuras implementações de reenvio e cancelamento.
- Permite adicionar novos meios de entrega sem alterar a responsabilidade da entidade.

---

## 5. Possíveis Evoluções

Embora a V1 mantenha um fluxo simples de convites, a entidade poderá evoluir futuramente.

Exemplos:

- Reenvio de convite.
- Cancelamento de convite.
- Listagem de convites pendentes.
- Histórico de convites.
- Registo da data de aceitação.
- Identificação do utilizador que aceitou o convite.
- Envio de convites através de SMS.
- Envio de convites através de WhatsApp.
- Diferentes períodos de validade.
- Notificações para o Owner quando um convite for aceite.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão

O `ConviteUtilizador` representa o processo de convite de uma pessoa para integrar uma ótica.

A entidade funciona como uma etapa intermediária entre o envio do convite e a criação da associação efetiva representada por `UtilizadorOtica`.

O convite é direcionado através de um endereço de email e utiliza um token de segurança para validar o processo de aceitação.

A pessoa convidada pode já possuir uma conta ou ainda precisar realizar o seu registo. Em ambos os casos, a associação à ótica somente será criada após a conclusão do fluxo de aceitação.

Essa separação mantém as responsabilidades bem definidas, evita a criação prematura de associações e permite que o sistema evolua futuramente com diferentes mecanismos de entrega, auditoria e gestão de convites.
