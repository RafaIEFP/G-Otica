## 1. Contexto

A entidade `ItemVenda` representa cada produto incluído em uma venda.

Ela faz a ligação entre a entidade `Venda` e a entidade `Produto`, permitindo que uma mesma venda contenha vários produtos.

Cada registo representa uma linha da venda e preserva as informações comerciais daquele produto no momento da operação.

Quando o produto corresponde a uma lente, o `ItemVenda` possui também um `ItemLente` com as informações necessárias para representar a lente personalizada.

---

## 2. Responsabilidade

A responsabilidade da entidade `ItemVenda` é armazenar as informações específicas de cada produto vendido.

Atualmente são armazenados:

- Produto vendido;
    
- Quantidade;
    
- Valor unitário;
    
- Desconto aplicado;
    
- Valor total do item;
    
- Observações;
    
- Venda à qual pertence;
    
- `ItemLente`, quando aplicável.
    

Essas informações pertencem à operação de venda e não ao cadastro permanente do produto.

---

## 3. Regras de Domínio

#### Todo ItemVenda pertence a uma Venda

Cada `ItemVenda` possui uma referência obrigatória para a venda através de `SaleId`.

Uma venda pode possuir vários itens.

---

#### Todo ItemVenda referencia um Produto

Cada item representa a comercialização de um produto existente no catálogo da ótica.

Essa relação é mantida através de `ProductId`.

Um mesmo produto pode participar de várias vendas e também pode aparecer em mais de um item dentro da mesma venda quando necessário.

Esse comportamento é especialmente útil no caso de lentes, permitindo, por exemplo, utilizar o mesmo produto para representar separadamente as lentes dos olhos direito e esquerdo.

---

#### A quantidade deve ser maior que zero

Todo item deve possuir uma quantidade superior a zero.

Não é permitido registar um item com quantidade igual ou inferior a zero.

---

#### Itens de lente possuem quantidade igual a um

Quando o produto possui `ProductType.Lens`, o respetivo `ItemVenda` deve possuir:

```text
Quantity = 1
```

Isso ocorre porque cada `ItemLente` representa individualmente uma lente personalizada.

Assim, caso sejam necessárias duas lentes, elas são representadas através de dois itens distintos.

---

#### O valor unitário preserva o preço histórico do Produto

`UnitPrice` representa o preço base do produto no momento em que a venda foi registada.

Esse valor é obtido através de:

```text
Product.BasePrice
```

e armazenado no próprio `ItemVenda`.

Dessa forma, alterações posteriores no preço do produto não modificam vendas já realizadas.

Assim:

```text
Product.BasePrice
→ preço atual do produto
```

enquanto:

```text
SaleItem.UnitPrice
→ preço utilizado naquela venda
```

---

#### O desconto é aplicado ao ItemVenda

Cada item pode possuir um `DiscountAmount`.

O desconto:

- não pode ser negativo;
    
- deve ser inferior ao valor bruto do produto.
    

O valor bruto é calculado através de:

```text
UnitPrice × Quantity
```

Portanto:

```text
DiscountAmount < UnitPrice × Quantity
```

Isso significa que o modelo atual não permite que o desconto reduza o valor do produto a zero ou resulte em um valor negativo.

---

#### O valor total do ItemVenda é calculado

O `TotalAmount` do item não é informado diretamente.

Para um produto comum:

```text
TotalAmount
=
(UnitPrice × Quantity)
- DiscountAmount
```

Quando o item possui uma lente com tratamentos:

```text
TotalAmount
=
(UnitPrice × Quantity)
- DiscountAmount
+ Tratamentos
```

Os valores dos tratamentos são obtidos através de:

```text
ItemLensTreatment.UnitPrice
```

---

#### O desconto não é aplicado aos tratamentos

Na implementação atual, o desconto pertence ao valor do produto.

Primeiro é calculado:

```text
Valor do produto
- desconto
```

e posteriormente são adicionados os tratamentos da lente.

Exemplo:

```text
Lente:          100 €
Desconto:       -10 €
Tratamentos:    +20 €

Total do item:  110 €
```

Assim, `DiscountAmount` não reduz diretamente os valores dos tratamentos associados ao `ItemLente`.

---

#### As observações são opcionais

O item pode possuir uma observação específica através de `Notes`.

Essa informação é opcional e possui um limite máximo de 500 caracteres.

Ela pode ser utilizada para registar informações relevantes relacionadas especificamente àquele item da venda.

---

#### Produtos do tipo Lens exigem ItemLente

Quando o produto associado ao item possui:

```text
ProductType.Lens
```

o `ItemVenda` deve possuir um `ItemLente`.

Essa entidade contém as informações específicas da lente personalizada.

---

#### Produtos que não sejam Lens não podem possuir ItemLente

Um `ItemLente` somente pode existir associado a um item cujo produto seja do tipo `Lens`.

Produtos como:

- armações;
    
- estojos;
    
- flanelas;
    
- acessórios;
    

não podem possuir informações de `ItemLente`.

---

## 4. Decisões de Modelagem

#### ItemVenda faz a ligação entre Venda e Produto

Foi adotada uma entidade especializada para representar os produtos existentes dentro de uma venda.

Cada `ItemVenda` mantém referências para:

```text
Venda
   ↑
ItemVenda
   ↓
Produto
```

Essa abordagem permite que uma venda contenha vários produtos e que um mesmo produto participe de diferentes vendas.

---

#### Informações comerciais permanecem no ItemVenda

Dados como:

- quantidade;
    
- desconto;
    
- valor unitário;
    
- valor total;
    
- observações;
    

pertencem ao item da venda e não ao cadastro do produto.

Isso preserva as condições comerciais existentes no momento em que a operação foi realizada.

---

#### O preço do produto é preservado historicamente

O preço utilizado durante a venda é copiado de `Product.BasePrice` para `SaleItem.UnitPrice`.

Essa decisão impede que alterações posteriores no catálogo modifiquem operações anteriores.

---

#### O valor total inclui tratamentos quando aplicável

No caso de um item correspondente a uma lente, `TotalAmount` também considera os tratamentos selecionados.

Os preços individuais desses tratamentos não são armazenados diretamente no `ItemVenda`.

Eles permanecem em:

```text
ItemLenteTratamento
```

e o `ItemVenda` mantém apenas o valor total resultante da operação.

---

#### Informações específicas de lentes não pertencem diretamente ao ItemVenda

Quando o produto vendido é uma lente, informações como:

- olho correspondente;
    
- DP;
    
- DNP;
    
- tipo da lente;
    
- índice de refração;
    
- material;
    
- cor;
    
- diâmetro;
    
- tratamentos;
    

não são armazenadas diretamente no `ItemVenda`.

Essas informações pertencem ao `ItemLente`.

Essa separação mantém `ItemVenda` aplicável a qualquer tipo de produto.

---

#### A relação com ItemLente é opcional na modelagem

A entidade possui:

```text
ItemLens?
```

porque apenas itens correspondentes a lentes necessitam dessas informações.

Assim:

```text
Produto comum
→ ItemVenda
```

enquanto:

```text
Produto Lens
→ ItemVenda
   └── ItemLente
```

A obrigatoriedade de `ItemLente` para produtos do tipo `Lens` é garantida pelas regras do processo de registo da venda.

---

#### Um mesmo Produto pode originar mais de um ItemVenda

Não existe uma regra que obrigue cada `ProductId` a aparecer apenas uma vez na venda.

Essa decisão permite representar situações em que duas linhas precisam utilizar o mesmo cadastro de produto, mas possuem características próprias.

O principal exemplo é uma venda de duas lentes baseada no mesmo produto:

```text
ItemVenda
├── Produto: Lente X
└── ItemLente: Olho direito

ItemVenda
├── Produto: Lente X
└── ItemLente: Olho esquerdo
```

Cada lente permanece individualizada mesmo utilizando o mesmo produto de catálogo.

Para efeito de stock, as quantidades desses itens são posteriormente agrupadas pelo produto.

---

#### O ItemVenda não altera diretamente o stock

`ItemVenda` representa os dados comerciais da linha da venda.

A redução do stock é coordenada pelo processo de registo da `Venda`.

As quantidades dos itens são agrupadas por `ProductId` e utilizadas para:

- reduzir `Product.StockQuantity`;
    
- criar a respetiva `Movimentação de Estoque` do tipo `Sale`.
    

Essa separação mantém o item responsável pelos dados comerciais e a operação de venda responsável pela alteração do stock.

---

## 5. Benefícios

A modelagem adotada permite:

- representar vendas com vários produtos;
    
- reutilizar o mesmo produto em diferentes itens quando necessário;
    
- armazenar o histórico correto dos preços praticados;
    
- aplicar descontos individualmente;
    
- preservar observações específicas de cada produto vendido;
    
- calcular corretamente o valor de cada linha da venda;
    
- incluir tratamentos no valor das lentes;
    
- manter separadas as informações comerciais e as informações específicas da fabricação;
    
- utilizar a mesma estrutura para qualquer tipo de produto;
    
- especializar apenas os itens que representam lentes através de `ItemLente`.
    

---

## 6. Conclusão

A entidade `ItemVenda` representa cada linha de uma venda.

A sua responsabilidade é preservar as informações comerciais específicas do produto vendido, incluindo quantidade, preço unitário, desconto, valor total e observações.

O preço utilizado é armazenado historicamente, impedindo que alterações posteriores no cadastro do produto modifiquem vendas anteriores.

Quando o produto é uma lente, o item possui obrigatoriamente um `ItemLente`, responsável pelas características específicas de fabricação e pelos tratamentos selecionados.

O valor desses tratamentos também participa do `TotalAmount` do item.

Essa separação mantém `ItemVenda` genérico o suficiente para representar qualquer produto, enquanto as necessidades específicas das lentes permanecem isoladas na entidade especializada correspondente.