## 1. Contexto

A `Ótica` representa uma unidade de negócio dentro do sistema.

É dentro do contexto de uma ótica que acontecem as operações comerciais, como cadastro de clientes, gestão de produtos, vendas, compras junto aos fornecedores, tratamentos e demais operações relacionadas ao funcionamento da unidade.

Embora o sistema seja utilizado por proprietários e funcionários, as informações de negócio pertencem à ótica e não aos utilizadores que as registaram.

Por esse motivo, a ótica é considerada uma das entidades centrais do domínio.

---

## 2. Regras de Domínio

Durante o levantamento de requisitos foram identificadas as seguintes regras do negócio.

#### Um utilizador pode ser proprietário de várias óticas

O sistema permite que um mesmo utilizador seja proprietário de diferentes unidades.

Exemplo:

- Ótica Centro;
    
- Ótica Shopping;
    
- Ótica Norte.
    

Cada uma representa uma unidade independente dentro do sistema.

A propriedade de uma ótica não é armazenada diretamente na entidade `Ótica`, sendo representada através da associação `UtilizadorOtica` com a função de proprietário (`owner`).

---

#### Cada ótica possui os seus próprios dados

Clientes, produtos, fornecedores, tratamentos, vendas, compras e demais registos pertencem ao contexto da unidade onde foram cadastrados.

Não existe partilha automática de dados entre diferentes óticas.

Por exemplo, um cliente cadastrado na Ótica Centro não pertence automaticamente à Ótica Shopping.

---

#### Funcionários podem trabalhar em uma ou mais óticas

Uma ótica pode possuir diversos utilizadores associados.

Da mesma forma, um utilizador pode estar associado a mais de uma ótica.

Essa relação é representada através da entidade `UtilizadorOtica`, que também armazena informações como:

- função do utilizador na ótica;
    
- data de entrada;
    
- estado da associação.
    

---

#### A criação de uma ótica define o proprietário inicial

Quando uma nova ótica é registada, o utilizador responsável pelo registo é automaticamente associado à unidade através de `UtilizadorOtica` com a função `owner`.

Dessa forma, toda ótica possui inicialmente um utilizador responsável pela sua administração.

---

#### A propriedade da ótica pode ser transferida

O proprietário pode transferir a propriedade da ótica para outro utilizador.

Para receber a propriedade, o novo proprietário deve:

- existir e estar ativo;
    
- já estar associado à ótica;
    
- ser diferente do proprietário atual.
    

Após a transferência:

- o novo utilizador assume a função `owner`;
    
- o antigo proprietário passa a possuir a função `manager`.
    

A transferência de propriedade é uma operação própria e não ocorre automaticamente durante outras ações do sistema.

---

#### O número fiscal da ótica deve ser único

Cada ótica possui um número fiscal (`TaxNumber`) que identifica a unidade no sistema.

Não é permitido registar duas óticas com o mesmo número fiscal.

A mesma regra é aplicada quando os dados de uma ótica são atualizados.

---

#### A ótica pode ser desativada

A entidade possui um estado (`IsActive`) que permite desativar uma ótica sem eliminar os seus dados da base de dados.

Quando uma ótica é desativada, as associações ativas entre utilizadores e essa ótica também são desativadas.

Os dados históricos da unidade permanecem armazenados.

---

#### A ótica é responsável pelo contexto das operações do negócio

As operações realizadas pelo sistema acontecem dentro do contexto de uma ótica.

Entre elas:

- cadastro de clientes;
    
- gestão de produtos;
    
- gestão de fornecedores;
    
- compras;
    
- vendas;
    
- tratamentos;
    
- gestão dos utilizadores associados à unidade.
    

---

## 3. Decisões de Modelagem

#### A Ótica é uma entidade própria

Foi criada uma entidade específica para representar cada unidade.

Ela não é apenas um atributo pertencente a outra entidade.

Atualmente, a entidade armazena:

- Nome;
    
- Email;
    
- Telefone;
    
- Número fiscal;
    
- Estado do cadastro.
    

Essa estrutura permite que cada unidade possua identidade e dados próprios.

---

#### Não existe uma entidade Empresa na modelagem atual

Embora um mesmo proprietário possa administrar várias óticas, o modelo atual não possui uma entidade específica para representar uma empresa ou grupo empresarial.

Cada ótica é uma unidade independente e a relação de propriedade é determinada através de `UtilizadorOtica`.

Caso futuramente seja necessário representar formalmente grupos empresariais compostos por várias unidades, essa estrutura poderá evoluir.

---

#### As entidades de negócio pertencem ao contexto da Ótica

Algumas entidades possuem uma referência direta para a ótica à qual pertencem.

Entre elas:

- `Cliente`;
    
- `Produto`;
    
- `Fornecedor`;
    
- `Compra`;
    
- `Venda`;
    
- `Tratamento`;
    
- `Convite`;
    
- `UtilizadorOtica`.
    

Outras entidades pertencem ao contexto da ótica de forma indireta, através das suas relações.

Exemplos:

- `Pagamento` pertence a uma `Venda`, que pertence a uma ótica;
    
- `Receita` pertence a um `Cliente`, que pertence a uma ótica;
    
- `Movimentação de Estoque` pertence a um `Produto`, que pertence a uma ótica;
    
- itens de venda e de compra pertencem às respetivas operações.
    

Essa abordagem mantém o contexto da unidade sem necessidade de repetir `OpticalStoreId` em todas as entidades do domínio.

---

#### A relação entre Utilizador e Ótica é N:N

Não foi adicionada uma chave estrangeira `OpticalStoreId` diretamente em `Utilizador`.

Em vez disso, foi criada a entidade intermediária `UtilizadorOtica`.

Essa decisão permite representar corretamente cenários em que:

- uma ótica possui vários utilizadores;
    
- um utilizador trabalha em diferentes óticas;
    
- o mesmo utilizador possui funções diferentes em unidades distintas.
    

A modelagem dessa relação é detalhada no documento [[UtilizadorOtica]].

---

#### O proprietário é definido através de UtilizadorOtica

A entidade `Ótica` não possui uma propriedade como `OwnerId`.

A propriedade da unidade é determinada pela função `owner` existente na associação entre `Utilizador` e `Ótica`.

Essa decisão mantém as responsabilidades relacionadas aos utilizadores e às suas funções centralizadas na entidade `UtilizadorOtica`.

---

#### A Ótica é o limite de pertencimento dos dados

Foi definido que os registos comerciais pertencem à ótica e não ao utilizador que realizou determinada operação.

O utilizador representa quem executou ou registou uma ação quando essa informação é necessária.

Já a ótica representa a unidade à qual aquela informação pertence.

Essa separação é especialmente importante para impedir a mistura de informações entre diferentes unidades.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Permite que um mesmo utilizador administre uma ou várias óticas.
    
- Mantém os dados organizados por unidade.
    
- Evita a mistura de informações entre diferentes óticas.
    
- Facilita o controlo de permissões entre utilizadores e unidades.
    
- Permite atribuir funções diferentes ao mesmo utilizador em óticas distintas.
    
- Mantém o histórico da unidade mesmo após a sua desativação.
    
- Facilita futuras implementações de relatórios por unidade.
    
- Permite evoluir futuramente para estruturas empresariais mais complexas sem alterar o conceito central de ótica.
    

---

## 5. Possíveis Evoluções

Embora a versão atual mantenha um cadastro simples de óticas, a entidade poderá evoluir futuramente.

Exemplos:

- Endereço completo;
    
- Horário de funcionamento;
    
- Logotipo;
    
- Configurações específicas da unidade;
    
- Dados fiscais adicionais;
    
- Informações de contacto secundárias;
    
- Configurações de emissão de documentos;
    
- Preferências de notificações;
    
- Agrupamento de várias óticas numa entidade empresarial;
    
- Funcionalidade específica de reativação de óticas.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A `Ótica` representa a unidade organizacional e comercial do sistema e constitui um dos principais conceitos do domínio.

As informações de negócio são mantidas dentro do contexto da respetiva ótica, de forma direta ou através das relações entre as entidades.

Os utilizadores relacionam-se com as óticas através de `UtilizadorOtica`, permitindo representar funções, múltiplas unidades e a propriedade da ótica sem acoplar essas responsabilidades diretamente à entidade.

A ótica também possui um estado ativo ou inativo, permitindo a sua desativação sem eliminação dos dados históricos.

Essa modelagem mantém os dados isolados por unidade, facilita o controlo de acesso e permite que o sistema suporte utilizadores responsáveis por uma ou várias óticas.