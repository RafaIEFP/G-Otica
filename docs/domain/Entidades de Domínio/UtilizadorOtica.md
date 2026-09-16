## 1. Contexto

A entidade `UtilizadorOtica` representa o vínculo entre um utilizador e uma ótica.

Embora um utilizador possua uma identidade única dentro do sistema, a sua atuação profissional acontece dentro do contexto de uma ou mais unidades.

É nesse vínculo que são definidas informações como a função exercida, a data de entrada e o estado da associação do utilizador naquela ótica.

Dessa forma, a entidade não existe apenas para conectar `Utilizador` e `Ótica`, mas para representar a relação profissional e de acesso entre uma pessoa e uma unidade.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um utilizador pode trabalhar em várias óticas

Uma mesma pessoa pode estar vinculada a diferentes unidades.

Exemplos:

- proprietário de duas óticas;
    
- gerente em duas unidades;
    
- gerente em uma ótica e vendedor em outra;
    
- vendedor associado a diferentes unidades.
    

Cada vínculo é independente e pode possuir uma função e um estado diferentes.

---

#### Uma ótica possui vários utilizadores

Cada unidade pode possuir diversos utilizadores associados.

As funções atualmente existentes são:

- proprietário (`owner`);
    
- gerente (`manager`);
    
- vendedor (`salesperson`).
    

---

#### A função depende da ótica

A função exercida por um utilizador não é uma característica da pessoa.

Ela depende da unidade onde o utilizador atua.

Exemplo:

```text
João
 ├── Ótica Centro (owner)
 └── Ótica Shopping (manager)
```

O mesmo utilizador pode, portanto, exercer funções diferentes em unidades distintas.

---

#### Existe apenas um vínculo entre o mesmo utilizador e a mesma ótica

A associação é identificada pelo conjunto:

- `UserId`;
    
- `OpticalStoreId`.
    

Esse conjunto forma a chave primária da entidade.

Dessa forma, um utilizador não pode possuir dois vínculos distintos com a mesma ótica.

Caso um vínculo seja desativado, o respetivo registo permanece existente e pode ser posteriormente reativado.

---

#### O vínculo pode ser desativado sem desativar a conta

Um colaborador pode deixar de atuar em determinada ótica sem perder a sua conta no sistema.

Da mesma forma, pode continuar ativo em outras unidades.

Por esse motivo, `UtilizadorOtica` possui o seu próprio estado (`IsActive`), independente do estado da entidade `Utilizador`.

---

#### Um vínculo só pode ser reativado se a conta do utilizador estiver ativa

A reativação de um vínculo exige que a conta correspondente em `Utilizador` esteja ativa.

Uma conta inativa não pode recuperar acesso a uma ótica através da simples reativação da associação.

---

#### A data de entrada pertence ao vínculo

`EntranceDate` representa a data em que o utilizador passou a estar associado àquela ótica.

Essa informação pertence à relação entre o utilizador e a unidade, e não à conta do utilizador.

A reativação de um vínculo não altera automaticamente essa data.

---

#### A criação de uma ótica cria o vínculo do proprietário

Quando um utilizador regista uma nova ótica, é automaticamente criado um vínculo `UtilizadorOtica` com:

- função `owner`;
    
- estado ativo;
    
- data de entrada correspondente à data da criação da associação.
    

Assim, a propriedade da ótica é representada pelo vínculo e não por uma propriedade diretamente armazenada em `Ótica`.

---

#### Novos colaboradores podem ser associados através de convite

Um utilizador pode passar a integrar uma ótica através da aceitação de um convite válido.

Ao aceitar o convite, é criado o vínculo com:

- a ótica correspondente;
    
- a função definida no convite;
    
- a data de entrada;
    
- o estado ativo.
    

Os convites comuns podem atribuir as funções:

- `manager`;
    
- `salesperson`.
    

A função `owner` não é atribuída através de convite.

---

#### A função `owner` possui regras próprias

O proprietário possui um tratamento específico dentro do domínio.

Um vínculo com função `owner`:

- não pode ser desativado através do fluxo comum de remoção de colaboradores;
    
- não pode ter a sua função alterada diretamente para `manager` ou `salesperson`;
    
- só deixa de possuir a função `owner` através do fluxo específico de transferência de propriedade.
    

---

#### A propriedade da ótica pode ser transferida

A transferência de propriedade altera os vínculos dos utilizadores envolvidos.

Quando a transferência é realizada:

- o novo proprietário passa a possuir a função `owner`;
    
- o proprietário anterior passa a possuir a função `manager`.
    

O novo proprietário deve possuir uma conta ativa e já estar associado ativamente à ótica.

---

#### Apenas o proprietário pode gerir determinadas características dos vínculos

Na versão atual do sistema, operações de gestão de colaboradores, como:

- alterar a função de um utilizador;
    
- desativar um vínculo;
    
- reativar um vínculo;
    

são restritas ao proprietário da ótica.

A consulta dos utilizadores associados à unidade pode ser realizada por membros ativos da ótica.

---

#### A desativação da conta afeta os seus vínculos ativos

Quando uma conta de utilizador é desativada, os vínculos ativos desse utilizador com as óticas também são desativados.

A posterior reativação da conta não reativa automaticamente essas associações.

Cada vínculo deve ser reativado separadamente.

---

#### A desativação da ótica afeta os vínculos ativos

Quando uma ótica é desativada, os vínculos ativos associados àquela unidade também são desativados.

Os registos permanecem armazenados, preservando o histórico das associações.

---

## 3. Decisões de Modelagem

#### Foi criada uma entidade intermediária

Em vez de adicionar `OpticalStoreId` diretamente em `Utilizador`, foi criada a entidade `UtilizadorOtica`.

Essa abordagem representa corretamente a relação muitos-para-muitos existente entre utilizadores e óticas.

---

#### O vínculo utiliza uma chave primária composta

A entidade não possui um identificador próprio como `Id`.

A sua chave primária é composta por:

```text
UserId + OpticalStoreId
```

Essa decisão representa naturalmente a identidade do vínculo e impede a criação de mais de uma associação entre o mesmo utilizador e a mesma ótica.

---

#### A função pertence ao vínculo

Foi decidido armazenar a função em `UtilizadorOtica`.

Essa decisão evita inconsistências quando um utilizador desempenha funções diferentes em unidades distintas.

Exemplo:

```text
João
 ├── Ótica Centro (owner)
 └── Ótica Shopping (manager)

Rafael
 └── Ótica Centro (salesperson)

Yan
 ├── Ótica Centro (manager)
 └── Ótica Shopping (salesperson)
```

---

#### As funções são limitadas pelo domínio

As funções atualmente reconhecidas pelo sistema são:

```text
owner
manager
salesperson
```

As funções `manager` e `salesperson` podem ser atribuídas através de convite ou alteração de função.

A função `owner` possui um fluxo próprio e só é atribuída durante:

- a criação de uma ótica;
    
- a transferência de propriedade.
    

---

#### O vínculo possui informações próprias

Além das chaves estrangeiras, a entidade armazena informações que pertencem exclusivamente à relação entre utilizador e ótica.

Atualmente são armazenados:

- `EntranceDate`;
    
- `Role`;
    
- `IsActive`.
    

Esses dados não pertencem exclusivamente nem a `Utilizador` nem a `Ótica`, pois descrevem o relacionamento entre ambos.

---

#### O estado da conta e o estado do vínculo são independentes

`User.IsActive` representa o estado geral da conta.

`UserOpticalStore.IsActive` representa se o utilizador possui atualmente um vínculo ativo com determinada ótica.

Assim, um utilizador pode possuir:

```text
Conta ativa
+
Vínculo ativo com Ótica A
+
Vínculo inativo com Ótica B
```

Essa separação permite controlar o acesso a cada unidade independentemente da existência da conta.

---

#### O vínculo participa diretamente da autorização

O sistema utiliza `UtilizadorOtica` para verificar se o utilizador autenticado pertence à ótica indicada na operação.

Para operações restritas ao proprietário, também é verificado se o vínculo ativo possui a função `owner`.

Dessa forma, a entidade é uma das principais fontes de informação para as regras de autorização dentro do contexto de cada unidade.

---

#### As entidades de negócio continuam pertencendo à ótica

A existência de um vínculo entre utilizador e ótica não altera o pertencimento dos dados comerciais.

Entidades como:

- `Cliente`;
    
- `Produto`;
    
- `Venda`;
    
- `Compra`;
    
- `Fornecedor`;
    
- `Tratamento`;
    

continuam pertencendo à ótica correspondente.

Quando uma entidade também armazena uma referência ao utilizador, essa informação representa normalmente a autoria ou participação em determinada operação, e não o proprietário do registo.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Permite múltiplas unidades por utilizador.
    
- Evita duplicação de contas.
    
- Permite funções diferentes para uma mesma pessoa.
    
- Mantém o histórico dos vínculos mesmo após a sua desativação.
    
- Permite controlar individualmente o acesso a cada ótica.
    
- Separa o estado da conta do estado da associação profissional.
    
- Centraliza as funções dos utilizadores dentro do contexto correto.
    
- Facilita as regras de autorização.
    
- Permite representar a propriedade da ótica sem adicionar `OwnerId` diretamente à entidade `Ótica`.
    
- Aproxima a modelagem da estrutura organizacional do negócio.
    

---

## 5. Possíveis Evoluções

Caso novas necessidades surjam, a entidade poderá armazenar outras informações relacionadas ao vínculo.

Exemplos:

- data de saída;
    
- motivo do desligamento;
    
- jornada de trabalho;
    
- comissão;
    
- meta de vendas;
    
- histórico de alterações de função;
    
- permissões mais granulares dentro da unidade.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `UtilizadorOtica` representa o relacionamento profissional e de acesso entre um utilizador e uma ótica.

Ela permite que uma mesma pessoa atue em diferentes unidades, exercendo funções distintas em cada uma delas, sem necessidade de duplicar a sua conta.

Além de resolver a relação muitos-para-muitos entre utilizadores e óticas, a entidade armazena informações próprias do vínculo, como função, data de entrada e estado.

Também desempenha um papel central nas regras de autorização, na gestão de colaboradores e na representação da propriedade da ótica.

Essa separação mantém independentes a identidade do utilizador, o estado da sua conta e a sua atuação em cada unidade.