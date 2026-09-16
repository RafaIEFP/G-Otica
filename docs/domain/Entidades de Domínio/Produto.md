## 1. Contexto

A entidade `Produto` representa os itens comercializados pela ótica.

Ela é utilizada tanto no processo de venda para os clientes quanto no processo de compra junto aos fornecedores.

O seu objetivo é manter o catálogo de produtos da unidade, incluindo informações de identificação, tipo, preço base, quantidade atual em stock e estado do cadastro.

O produto representa o item comercial propriamente dito. Informações específicas de uma determinada compra ou venda são armazenadas nas entidades relacionadas a essas operações.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um produto pertence a uma Ótica

Todo produto é cadastrado dentro do contexto de uma única ótica.

Essa associação é representada por `OpticalStoreId`.

O catálogo de produtos de cada unidade é independente.

Um produto cadastrado em uma ótica não passa automaticamente a existir nas demais unidades.

---

#### Um produto pode ser vendido diversas vezes

Um mesmo produto pode participar de inúmeras vendas ao longo do tempo.

Cada `ItemVenda` mantém uma referência ao produto comercializado através de `ProductId`.

O preço utilizado na venda é armazenado separadamente no próprio item através de `UnitPrice`.

Dessa forma, alterações posteriores no preço base do produto não modificam o valor registado em vendas anteriores.

---

#### Um produto pode ser comprado diversas vezes

Um produto também pode aparecer em diferentes compras realizadas junto aos fornecedores para reposição de stock.

Cada `ItemCompra` referencia o produto correspondente.

O preço pago na compra é armazenado no próprio item de compra e não altera diretamente o `BasePrice` do produto.

Dessa forma, o preço de aquisição e o preço base utilizado na comercialização possuem responsabilidades diferentes.

---

#### Existem diferentes tipos de produtos

A ótica comercializa diferentes tipos de produtos.

Atualmente são suportados:

- Lente (`Lens`);
    
- Armação (`Frame`);
    
- Estojo (`Case`);
    
- Flanela (`CleaningCloth`);
    
- Acessório (`Accessory`).
    

Embora possuam características e comportamentos diferentes, todos fazem parte do mesmo catálogo de produtos.

---

#### Produtos do tipo Lente possuem regras específicas durante a venda

Quando um produto com `ProductType.Lens` é incluído em uma venda, são necessárias informações adicionais para representar a lente personalizada destinada ao cliente.

Essas informações não pertencem ao cadastro do produto.

Por esse motivo, cada `ItemVenda` correspondente a uma lente deve possuir um `ItemLente`.

Além disso:

- a quantidade do `ItemVenda` deve ser igual a 1;
    
- a venda deve possuir uma `Receita`;
    
- as características específicas da lente são armazenadas em `ItemLente`.
    

Produtos que não sejam do tipo `Lens` não podem possuir `ItemLente`.

---

#### O código do produto deve ser único dentro da ótica

Cada produto possui um `ProductCode`.

O código funciona como uma referência utilizada para identificar o produto dentro do catálogo da unidade.

O sistema não determina atualmente se esse valor representa um código do fabricante, referência comercial ou código interno.

Essa interpretação pode variar conforme o produto utilizado pela ótica.

Entretanto, dentro da mesma ótica, não podem existir dois produtos com o mesmo `ProductCode`.

A regra também se aplica aos produtos inativos, pois a desativação não elimina o respetivo cadastro.

Óticas diferentes podem utilizar o mesmo código para produtos dos seus próprios catálogos.

---

#### O preço base deve ser maior que zero

Todo produto possui um `BasePrice` utilizado como preço base durante o registo de uma venda.

Esse valor deve ser superior a zero.

O preço pode ser posteriormente atualizado sem modificar os preços históricos armazenados nos itens de vendas anteriores.

---

#### O stock inicial não pode ser negativo

Durante o cadastro de um produto, pode ser informada uma quantidade inicial em stock.

Essa quantidade deve ser maior ou igual a zero.

Caso seja informado stock inicial superior a zero, o sistema cria uma `Movimentação de Estoque` do tipo:

```text
InitialStock
```

registrando a entrada inicial do produto.

Caso o produto seja criado com stock igual a zero, nenhuma movimentação inicial é necessária.

---

#### O stock não é alterado através da atualização comum do produto

A atualização cadastral permite alterar informações como:

- Nome;
    
- Tipo;
    
- Código;
    
- Preço base.
    

`StockQuantity` não faz parte dessa atualização.

Alterações de stock acontecem através de operações específicas do domínio, permitindo que cada mudança relevante possa ser associada à sua origem.

---

#### Compras aumentam o stock

Quando uma compra é registada, a quantidade adquirida de cada produto é adicionada ao respetivo stock.

Essa alteração também gera uma `Movimentação de Estoque` do tipo:

```text
Purchase
```

permitindo manter o histórico da entrada dos produtos.

Apenas produtos ativos pertencentes à mesma ótica podem participar da compra.

---

#### Vendas diminuem o stock

Quando uma venda é registada, a quantidade vendida é removida do stock dos respetivos produtos.

Essa alteração gera uma `Movimentação de Estoque` do tipo:

```text
Sale
```

A venda não pode ser concluída caso algum produto não possua quantidade suficiente em stock.

Dessa forma, o stock não pode tornar-se negativo através de uma venda.

Apenas produtos ativos pertencentes à mesma ótica podem ser vendidos.

---

#### O stock pode ser ajustado manualmente

O sistema permite realizar ajustes manuais para aumentar ou diminuir o stock.

Todo ajuste manual exige:

- tipo do ajuste;
    
- quantidade positiva;
    
- motivo da alteração.
    

A quantidade informada é convertida internamente numa entrada ou saída de stock.

Cada ajuste gera uma `Movimentação de Estoque` do tipo:

```text
ManualAdjustment
```

O sistema não permite uma redução manual superior à quantidade disponível.

---

#### O cancelamento de uma venda pode restaurar o stock

Quando uma venda é cancelada, determinados produtos podem retornar ao stock.

Essa restauração gera uma `Movimentação de Estoque` do tipo:

```text
SaleCancellation
```

As regras específicas dependem do estado da venda.

Se a venda for cancelada ainda em `Confirmed`, todos os seus itens retornam ao stock.

Caso a venda já esteja em `InProduction` ou `Ready`, produtos comuns retornam ao stock, mas itens que possuem `ItemLente` não são restaurados, pois representam lentes personalizadas cuja produção já foi iniciada.

---

#### O produto pode ser desativado e reativado

O produto possui um estado definido por `IsActive`.

A desativação permite retirar um produto das operações comerciais sem eliminar o seu cadastro ou histórico.

Produtos inativos não podem ser utilizados em novas vendas ou compras.

Caso o produto volte a ser comercializado, pode ser reativado.

A desativação não elimina o stock armazenado nem as referências existentes em operações anteriores.

---

## 3. Decisões de Modelagem

#### Foi criada uma única entidade para representar os produtos

Inicialmente foi considerada a criação de entidades específicas para lentes, armações e demais itens.

Essa abordagem foi descartada.

Todos os itens comercializados compartilham características comuns, como:

- identificação;
    
- código;
    
- preço base;
    
- stock;
    
- pertencimento à ótica;
    
- estado do cadastro.
    

Além disso, participam dos mesmos processos gerais de compra, venda e controlo de stock.

Por esse motivo, uma única entidade `Produto` atende ao modelo atual.

---

#### O tipo do produto é identificado por um enum

Foi adotado o atributo `ProductType`.

Os valores atualmente definidos são:

```text
Lens
Frame
Case
CleaningCloth
Accessory
```

O tipo permite que o sistema determine comportamentos específicos quando necessário.

O principal exemplo atual é `Lens`, que possui regras adicionais durante o processo de venda.

---

#### O Produto armazena a quantidade atual em stock

A propriedade `StockQuantity` representa a quantidade atualmente disponível daquele produto.

Ela funciona como o estado atual do stock.

O histórico das alterações não é armazenado diretamente na entidade `Produto`.

Para isso existe a entidade `Movimentação de Estoque`.

Dessa forma:

```text
Product.StockQuantity
→ situação atual
```

enquanto:

```text
StockMovement
→ histórico das alterações
```

---

#### O stock possui operações específicas

Foi decidido não permitir que `StockQuantity` seja simplesmente alterado através da edição cadastral do produto.

As alterações acontecem através de operações com significado próprio, como:

- stock inicial;
    
- compra;
    
- venda;
    
- ajuste manual;
    
- cancelamento de venda.
    

Essa separação permite manter o histórico e identificar a origem das alterações de stock.

---

#### O preço base não representa o preço histórico das operações

`BasePrice` representa o preço base atual utilizado pelo produto.

Quando uma venda é realizada, o valor utilizado naquele momento é copiado para:

```text
SaleItem.UnitPrice
```

Da mesma forma, numa compra, o preço efetivamente pago é armazenado em:

```text
PurchaseItem.UnitPrice
```

Assim, mudanças futuras no cadastro do produto não alteram operações já realizadas.

---

#### O Produto não armazena características da lente personalizada

Embora uma lente vendida possa possuir informações como:

- olho correspondente;
    
- DP;
    
- DNP;
    
- tipo da lente;
    
- índice de refração;
    
- material;
    
- cor;
    
- diâmetro;
    
- tratamentos;
    

esses dados não fazem parte do cadastro do produto.

Eles representam características da lente preparada para um cliente específico e são armazenados através de `ItemLente` e `ItemLenteTratamento`.

Essa decisão mantém separado:

```text
Produto
→ item existente no catálogo
```

de:

```text
ItemLente
→ configuração personalizada daquele item numa venda
```

---

#### Não foram criadas entidades específicas para cada tipo de produto

Durante a modelagem também foi considerada a criação de estruturas específicas como:

- Lente;
    
- Armação;
    
- CategoriaProduto.
    

Essa abordagem não é necessária no modelo atual.

As diferenças relevantes entre os produtos são representadas através de `ProductType` e, no caso das lentes vendidas, pelas informações adicionais de `ItemLente`.

Caso o domínio futuramente necessite de cadastros muito diferentes para cada categoria, essa decisão poderá ser revisitada.

---

#### Algumas características da lente permanecem representadas por valores simples ou enums

Na versão atual:

- o material da lente é representado por `LensMaterial`;
    
- o tipo da lente é representado por `LensType`;
    
- a cor é armazenada como texto;
    
- o tipo do produto é representado por `ProductType`.
    

Tratamentos possuem uma entidade própria, pois fazem parte do catálogo da ótica e possuem preço e ciclo de vida independentes.

---

#### O produto não é eliminado para deixar de ser comercializado

Foi adotado `IsActive` para controlar a disponibilidade do cadastro.

Dessa forma, produtos que já participam de compras, vendas ou movimentações de stock não precisam ser eliminados da base de dados.

A desativação preserva as referências históricas existentes.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Mantém um catálogo único para os diferentes tipos de produtos.
    
- Evita excesso de entidades específicas para cada categoria.
    
- Permite utilizar os mesmos produtos em compras e vendas.
    
- Mantém separadas as informações cadastrais e as características de lentes personalizadas.
    
- Preserva preços históricos das operações.
    
- Mantém separadas a quantidade atual e o histórico de movimentações de stock.
    
- Evita alterações de stock sem uma operação correspondente.
    
- Permite desativar produtos sem eliminar o seu histórico.
    
- Garante isolamento dos catálogos entre diferentes óticas.
    
- Permite comportamentos específicos conforme o tipo do produto.
    
- Facilita futuras evoluções da modelagem.
    

---

## 5. Possíveis Evoluções

Dependendo da evolução do sistema, a entidade poderá ser expandida.

Exemplos:

- categoria de produtos;
    
- fabricante;
    
- marca;
    
- código de barras;
    
- imagens do produto;
    
- controlo por lotes;
    
- stock mínimo;
    
- alertas de stock baixo;
    
- diferentes preços de venda;
    
- histórico de alterações do preço base;
    
- transferência de stock entre óticas.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `Produto` representa os itens que compõem o catálogo de uma ótica.

Ela mantém informações como nome, tipo, código, preço base, quantidade atual em stock, estado do cadastro e a ótica à qual pertence.

O mesmo produto pode participar de diferentes compras e vendas, enquanto os valores específicos dessas operações permanecem armazenados nos respetivos itens, preservando o histórico.

O controlo de stock é integrado aos principais processos do sistema e cada alteração relevante possui uma `Movimentação de Estoque` correspondente.

Produtos do tipo `Lens` recebem tratamento específico durante a venda, exigindo uma receita e um `ItemLente` responsável pelas características personalizadas da lente.

Essa separação mantém o catálogo simples, preserva o histórico das operações e permite que o domínio evolua sem misturar informações gerais do produto com dados específicos de compras, vendas ou fabricação de lentes.