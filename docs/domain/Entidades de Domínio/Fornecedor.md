## 1. Contexto

A entidade `Fornecedor` representa os fornecedores responsáveis pelo abastecimento de produtos utilizados pela ótica.

O seu objetivo é identificar a origem das aquisições realizadas e manter a relação entre as compras registadas e os respetivos fornecedores.

Na versão atual do sistema, o fornecedor possui uma responsabilidade predominantemente cadastral, enquanto as informações específicas de cada aquisição permanecem concentradas na entidade `Compra`.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um fornecedor pertence a uma Ótica

Todo fornecedor é cadastrado dentro do contexto de uma única ótica.

Essa associação é representada através de `OpticalStoreId`.

Dessa forma, os fornecedores de uma unidade são independentes dos fornecedores cadastrados nas demais óticas.

Um fornecedor cadastrado na Ótica Centro, por exemplo, não passa automaticamente a fazer parte do cadastro da Ótica Shopping.

---

#### Um fornecedor pode fornecer diversos produtos

Ao longo do tempo, uma ótica pode adquirir diferentes produtos do mesmo fornecedor.

Exemplos:

- lentes;
    
- armações;
    
- estojos;
    
- flanelas;
    
- acessórios.
    

Entretanto, não existe uma associação permanente entre `Fornecedor` e `Produto`.

Os produtos efetivamente adquiridos de determinado fornecedor são identificados através das compras realizadas.

O relacionamento ocorre através de:

```text
Fornecedor
    ↓
Compra
    ↓
ItemCompra
    ↓
Produto
```

Dessa forma, um mesmo produto também pode ser adquirido de diferentes fornecedores em compras distintas.

---

#### Um fornecedor pode participar de várias compras

Ao longo do tempo, diversas compras podem ser realizadas junto ao mesmo fornecedor.

Cada compra permanece armazenada individualmente, permitindo preservar o histórico das aquisições realizadas pela ótica.

---

#### Cada compra possui um único fornecedor

Uma compra é sempre realizada junto a um único fornecedor.

Caso seja necessário adquirir produtos de fornecedores diferentes, devem ser registadas compras distintas.

---

#### O fornecedor utilizado em uma compra deve pertencer à mesma ótica

Durante o registo de uma compra, o fornecedor informado deve pertencer à ótica onde a operação está sendo realizada.

Dessa forma, não é possível utilizar em uma compra um fornecedor cadastrado em outra unidade.

---

#### Apenas fornecedores ativos podem ser utilizados em novas compras

O fornecedor possui um estado definido por `IsActive`.

Para que seja utilizado numa nova compra, o fornecedor deve:

- existir;
    
- pertencer à ótica correspondente;
    
- estar ativo.
    

Um fornecedor inativo permanece registado no sistema e continua associado às compras realizadas anteriormente, mas não pode ser utilizado em novas aquisições enquanto estiver desativado.

---

#### O fornecedor pode ser desativado e reativado

Um fornecedor pode deixar de ser utilizado pela ótica sem que o seu cadastro seja eliminado.

A desativação altera apenas o seu estado.

As compras realizadas anteriormente permanecem associadas ao fornecedor, preservando o histórico das operações.

Caso a ótica volte a trabalhar com esse fornecedor, o cadastro pode ser reativado.

---

## 3. Decisões de Modelagem

#### O fornecedor será mantido simples

Foi decidido que a entidade possuirá apenas informações básicas relacionadas à identificação e contacto.

Atualmente são armazenados:

- Nome;
    
- Telefone;
    
- Email;
    
- Estado do cadastro;
    
- Ótica à qual pertence.
    

O nome é obrigatório.

O telefone e o email são opcionais e, quando informados, devem possuir formatos válidos.

Essa estrutura atende às necessidades atuais do sistema sem introduzir informações que ainda não possuem utilização no domínio.

---

#### Não existe uma relação direta entre Fornecedor e Produto

Não foi criada uma relação muitos-para-muitos entre `Fornecedor` e `Produto`.

A entidade `Produto` representa o catálogo da ótica independentemente de quem forneceu determinado item.

A origem dos produtos é registada no momento da compra através de `Compra` e `ItemCompra`.

Essa decisão permite que:

- um fornecedor forneça vários produtos;
    
- um produto seja comprado de diferentes fornecedores;
    
- o histórico de cada aquisição seja preservado individualmente.
    

Dessa forma, não é necessário manter uma lista permanente de produtos dentro da entidade `Fornecedor`.

---

#### Não serão armazenadas informações comerciais detalhadas

Dados como:

- condições de pagamento;
    
- prazos de entrega;
    
- contratos;
    
- representantes comerciais;
    
- tabelas de preços;
    
- limite de crédito;
    

não são controlados no modelo atual.

Essas informações não são necessárias para os objetivos atuais do projeto.

---

#### O fornecedor representa a origem da compra

A principal relação de negócio da entidade é com `Compra`.

Uma compra mantém uma referência através de:

```text
SupplierId
```

permitindo identificar de qual fornecedor os produtos daquela operação foram adquiridos.

As demais informações da operação, como:

- produtos adquiridos;
    
- quantidades;
    
- preços de aquisição;
    
- valor total;
    
- utilizador responsável;
    
- data da operação;
    

pertencem à própria `Compra` e aos seus `ItemCompra`.

---

#### Os preços não pertencem ao cadastro do fornecedor

O fornecedor não mantém uma tabela de preços dos produtos.

O preço efetivamente pago por determinado produto é armazenado no `ItemCompra` correspondente.

Dessa forma, diferentes compras do mesmo produto e fornecedor podem possuir preços distintos sem necessidade de alterar o cadastro do fornecedor ou do produto.

---

#### O fornecedor não é eliminado para deixar de ser utilizado

Foi adotado `IsActive` para controlar a disponibilidade do cadastro.

Essa decisão evita a eliminação de fornecedores que já participam do histórico de compras.

Assim:

```text
Fornecedor ativo
→ pode ser utilizado em novas compras
```

enquanto:

```text
Fornecedor inativo
→ permanece no histórico
→ não pode ser utilizado em novas compras
```

Caso necessário, o fornecedor pode ser posteriormente reativado.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Simplifica o cadastro de fornecedores.
    
- Mantém os fornecedores isolados por ótica.
    
- Permite identificar a origem das compras.
    
- Preserva o histórico mesmo após a desativação do fornecedor.
    
- Permite reutilizar o mesmo fornecedor em diversas compras.
    
- Permite adquirir o mesmo produto de fornecedores diferentes.
    
- Evita uma associação desnecessária entre fornecedor e produto.
    
- Mantém preços de aquisição associados às operações em que realmente ocorreram.
    
- Evita complexidade comercial que ainda não é necessária.
    
- Mantém responsabilidades bem definidas.
    
- Facilita futuras expansões caso novas necessidades surjam.
    

---

## 5. Possíveis Evoluções

Dependendo da evolução do sistema, poderão ser adicionadas novas informações e funcionalidades relacionadas aos fornecedores.

Exemplos:

- pessoa de contacto;
    
- morada;
    
- número de identificação fiscal;
    
- condições de pagamento;
    
- prazo médio de entrega;
    
- avaliação do fornecedor;
    
- catálogo de produtos fornecidos;
    
- tabelas de preços;
    
- representantes comerciais;
    
- histórico de negociações;
    
- documentos ou contratos associados ao fornecedor.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `Fornecedor` representa os fornecedores utilizados pela ótica para realizar as suas aquisições.

Cada fornecedor pertence a uma única ótica e pode participar de diversas compras ao longo do tempo.

Não existe uma relação direta entre fornecedor e produto. Os produtos adquiridos de cada fornecedor são identificados através das respetivas compras e dos seus itens, permitindo inclusive que o mesmo produto seja adquirido de fornecedores diferentes.

O fornecedor possui um estado ativo ou inativo, permitindo interromper a sua utilização em novas compras sem eliminar o histórico existente.

Essa modelagem mantém o cadastro simples, preserva corretamente o histórico das aquisições e evita introduzir complexidade comercial que ainda não é necessária ao domínio.