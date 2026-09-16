## 1. Contexto

A entidade `Utilizador` representa uma pessoa que possui uma conta de acesso ao sistema.

Ela é responsável pela identidade e pelo estado da conta do utilizador, armazenando as informações necessárias para autenticação e identificação.

Um utilizador pode trabalhar em uma ou mais óticas, exercendo funções diferentes em cada uma delas.

Por esse motivo, a entidade `Utilizador` não representa um funcionário de uma ótica específica, mas sim uma pessoa cuja conta pode estar vinculada a diferentes unidades.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um utilizador pode trabalhar em várias óticas

Uma mesma pessoa pode estar vinculada a diversas unidades.

Exemplos:

- proprietário de duas óticas;
    
- gerente em uma unidade e vendedor em outra;
    
- vendedor associado a diferentes unidades.
    

Cada vínculo é independente e possui a sua própria função e estado.

---

#### Uma ótica possui vários utilizadores

Cada unidade pode possuir diversos utilizadores associados.

As funções atualmente existentes são:

- proprietário (`owner`);
    
- gerente (`manager`);
    
- vendedor (`salesperson`).
    

---

#### Os cargos pertencem à relação com a ótica

O cargo exercido por um utilizador depende da unidade onde ele atua.

Uma mesma pessoa pode exercer funções diferentes em óticas distintas.

Exemplo:

```text
João
 ├── Ótica Centro (owner)
 └── Ótica Shopping (manager)
```

Por esse motivo, o cargo não pertence diretamente ao utilizador.

Essa regra é representada pela entidade `UtilizadorOtica`.

---

#### O e-mail identifica uma conta única

Cada utilizador possui um endereço de e-mail único no sistema.

Não é permitido registar duas contas com o mesmo e-mail.

Essa regra também é validada quando o utilizador altera os seus dados de perfil.

---

#### Apenas utilizadores ativos podem autenticar-se normalmente

A entidade possui um estado (`IsActive`) que indica se a conta está ativa.

Um utilizador inativo não pode realizar login nem utilizar operações protegidas que exigem uma conta autenticada válida.

A conta pode ser posteriormente reativada através do fluxo específico de reativação, desde que as credenciais sejam válidas.

---

#### A conta pode ser desativada sem eliminar o utilizador

A desativação da conta não remove o registo do utilizador da base de dados.

Quando a conta é desativada:

- o utilizador passa a ficar inativo;
    
- os vínculos ativos com óticas são desativados;
    
- os refresh tokens associados ao utilizador são removidos.
    

Essa abordagem preserva os dados históricos relacionados às operações realizadas anteriormente.

---

#### Um proprietário não pode desativar a própria conta

Um utilizador que ainda possui a função `owner` em alguma ótica não pode desativar a própria conta.

Antes da desativação, a propriedade das respetivas óticas deve ser transferida para outro utilizador através do fluxo específico de transferência de propriedade.

A desativação da conta não realiza essa transferência automaticamente.

---

#### A reativação da conta não reativa automaticamente os vínculos com óticas

Quando uma conta é reativada, apenas o estado da entidade `Utilizador` volta a ficar ativo.

Os vínculos `UtilizadorOtica` anteriormente desativados permanecem inativos e devem ser reativados através do fluxo específico de gestão dos utilizadores da ótica.

Essa separação evita que a reativação de uma conta conceda automaticamente acesso a unidades das quais o utilizador havia sido afastado.

---

#### O utilizador regista a autoria de determinadas operações

Algumas operações armazenam o utilizador responsável pela sua realização.

Entre elas:

- registo de vendas;
    
- registo de compras;
    
- movimentações de stock;
    
- criação de convites;
    
- recebimento de pagamentos.
    

Essa informação permite identificar quem executou determinadas operações dentro do sistema.

O pertencimento dos dados, no entanto, continua associado à ótica correspondente.

---

## 3. Decisões de Modelagem

#### Utilizador representa identidade e conta, não vínculo com a ótica

Foi decidido manter a entidade `Utilizador` responsável pela identidade e pelo estado da conta da pessoa.

Atualmente são armazenados:

- Nome;
    
- E-mail;
    
- Password armazenada de forma protegida;
    
- Estado da conta;
    
- Data de criação;
    
- Data da última atualização dos dados do perfil.
    

Nenhuma informação relacionada diretamente à ótica ou ao cargo é armazenada nessa entidade.

---

#### Não existe `OpticalStoreId` em `Utilizador`

Inicialmente poderia parecer natural adicionar uma chave estrangeira para a ótica.

Essa abordagem foi descartada.

Ela impediria que um mesmo utilizador trabalhasse em mais de uma unidade e obrigaria à duplicação de contas.

Para representar corretamente o domínio foi criada a entidade intermediária `UtilizadorOtica`.

---

#### O cargo pertence ao vínculo

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

#### O utilizador não representa diretamente permissões de uma ótica

A entidade `Utilizador` não armazena funções ou permissões específicas de uma unidade.

Essas informações dependem do contexto da ótica e pertencem ao vínculo `UtilizadorOtica`.

Essa decisão aumenta a flexibilidade do sistema e evita inconsistências quando o mesmo utilizador atua em diferentes unidades.

---

#### A conta pode existir sem vínculo com uma ótica

O registo de um utilizador cria a sua conta de acesso, mas não obriga à existência imediata de uma associação com uma ótica.

A associação com uma unidade ocorre através de processos específicos, como:

- criação de uma nova ótica;
    
- aceitação de um convite.
    

Dessa forma, a identidade do utilizador permanece independente dos seus vínculos profissionais.

---

#### A autenticação é separada da entidade Utilizador

A password é armazenada na entidade apenas na sua forma protegida.

O sistema utiliza autenticação baseada em tokens, com:

- access token;
    
- refresh token.
    

Os refresh tokens são representados por uma entidade própria e não são armazenados diretamente em `Utilizador`.

Essa separação evita misturar a identidade da pessoa com o ciclo de vida das sessões de autenticação.

---

#### O utilizador regista a autoria das operações

Entidades como `Venda` e `Compra` armazenam uma referência ao utilizador responsável pelo registo.

Outras operações, como movimentações de stock, convites e recebimentos de pagamentos, também podem identificar o utilizador que as executou.

Esse relacionamento indica a autoria da operação, mas não altera o pertencimento dos dados.

Os registos continuam pertencendo ao contexto da ótica onde foram criados.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Evita duplicação de contas de utilizadores.
    
- Permite que uma pessoa trabalhe em várias óticas.
    
- Permite funções diferentes para a mesma pessoa em unidades distintas.
    
- Separa a identidade do utilizador dos seus vínculos profissionais.
    
- Permite desativar contas sem eliminar dados históricos.
    
- Mantém separadas as responsabilidades de autenticação e de associação com óticas.
    
- Permite invalidar sessões através da remoção dos refresh tokens.
    
- Facilita o controlo de acesso dentro do contexto de cada unidade.
    

---

## 5. Possíveis Evoluções

A entidade poderá evoluir futuramente com informações adicionais, como:

- foto de perfil;
    
- último acesso;
    
- autenticação em dois fatores;
    
- idioma preferencial;
    
- preferências pessoais;
    
- recuperação de password através de fluxo dedicado.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `Utilizador` representa a identidade e a conta de uma pessoa dentro do sistema.

Ela não armazena diretamente as óticas em que o utilizador trabalha nem as funções que exerce em cada unidade.

Essa separação permite reutilizar a mesma conta em diferentes óticas, evita duplicação de utilizadores e mantém a modelagem alinhada ao funcionamento do sistema.

As informações relacionadas ao vínculo entre utilizador e ótica são tratadas pela entidade `UtilizadorOtica`, enquanto os dados relacionados às sessões de autenticação são mantidos separadamente através dos mecanismos de access token e `RefreshToken`.

A conta pode ser desativada e posteriormente reativada sem eliminar os dados históricos do utilizador, mantendo independentes o estado da conta e os vínculos com as diferentes óticas.