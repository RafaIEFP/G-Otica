## 1. Contexto

A entidade `ItemCompra` representa cada produto incluído em uma compra realizada pela ótica junto a um fornecedor.

Ela faz a ligação entre a entidade `Compra` e a entidade `Produto`, permitindo que uma mesma compra seja composta por diversos produtos.

Cada registo representa uma linha da compra e mantém as informações específicas daquela aquisição.

---

## 2. Responsabilidade

A responsabilidade da entidade `ItemCompra` é armazenar as informações comerciais de cada produto adquirido.

Essas informações incluem:

- Produto adquirido;
    
- Quantidade;
    
- Valor unitário;
    
- Valor total do item.
    

A entidade não armazena informações relacionadas ao fornecedor ou à compra como um todo, pois essas responsabilidades pertencem à entidade `Compra`.

---

## 3. Regras de Domínio

#### A quantidade deve ser maior que zero

Todo `ItemCompra` deve possuir uma quantidade superior a zero.

Não é permitido registar um item de compra sem quantidade ou com quantidade negativa.

---

#### O valor unitário não pode ser negativo

O `UnitPrice` representa o preço pago por unidade do produto naquela aquisição.

Esse valor deve ser maior ou igual a zero.

---

#### O valor total é calculado a partir da quantidade e do preço unitário

O `TotalAmount` do item não é informado diretamente.

Ele é calculado através de:

```text
TotalAmount = Quantity × UnitPrice
```

O valor total da `Compra` é posteriormente obtido através da soma dos valores totais dos seus itens.

---

## 4. Decisões de Modelagem

#### ItemCompra faz a ligação entre Compra e Produto

Foi adotada uma entidade intermediária para representar os produtos de uma compra.

Cada `ItemCompra` mantém uma referência para:

- `Compra`, através de `PurchaseId`;
    
- `Produto`, através de `ProductId`.
    

Essa abordagem permite que uma compra contenha vários produtos e que um mesmo produto participe de diversas compras ao longo do tempo.

---

#### Informações do item permanecem no ItemCompra

Dados como:

- quantidade;
    
- valor unitário;
    
- valor total;
    

pertencem ao item da compra e não ao cadastro do produto.

Isso garante que alterações futuras no produto não afetem o histórico das compras já realizadas.

---

#### O valor unitário representa o preço histórico da aquisição

`UnitPrice` representa o preço efetivamente registado para o produto naquela compra específica.

Esse valor é preservado no `ItemCompra` e não depende de alterações posteriores realizadas em `Product.BasePrice`.

Dessa forma:

```text
PurchaseItem.UnitPrice
→ preço praticado naquela aquisição
```

enquanto:

```text
Product.BasePrice
→ preço base atual do produto
```

Essa separação preserva corretamente o histórico comercial das compras.

---

#### ItemCompra não possui informações específicas de fabricação

Embora alguns produtos adquiridos sejam lentes, a compra representa apenas a aquisição desses produtos para o stock da ótica.

Informações relacionadas à personalização de uma lente para um cliente, como:

- DP;
    
- DNP;
    
- índice de refração;
    
- material;
    
- cor;
    
- tratamentos;
    

não fazem parte desse processo e, por isso, não pertencem ao `ItemCompra`.

Essas informações são armazenadas em `ItemLente` durante o processo de venda.

---

#### O ItemCompra não altera diretamente o stock

O `ItemCompra` representa os dados da aquisição.

O impacto no stock é realizado pelo processo de registo da `Compra`, que utiliza as quantidades dos seus itens para aumentar o stock dos respetivos produtos e criar as `Movimentações de Estoque` correspondentes.

Dessa forma, a entidade mantém a responsabilidade de representar a linha da compra, enquanto a operação de compra coordena a alteração do stock.

---

## 5. Benefícios

A modelagem adotada permite:

- representar compras com diversos produtos;
    
- armazenar quantidade e preço de cada produto adquirido;
    
- calcular o valor total de cada item;
    
- preservar o histórico dos valores pagos aos fornecedores;
    
- separar corretamente as informações da compra das informações do produto;
    
- evitar que alterações futuras no produto modifiquem compras anteriores;
    
- reutilizar o mesmo cadastro de produtos utilizado nas vendas.
    

---

## 6. Conclusão

A entidade `ItemCompra` representa cada linha de uma compra realizada pela ótica.

A sua responsabilidade é registar o produto adquirido, a quantidade, o preço unitário e o valor total correspondente.

O preço unitário permanece armazenado como informação histórica daquela aquisição, independentemente de alterações futuras no cadastro do produto.

O `ItemCompra` também serve como ligação entre `Compra` e `Produto`, enquanto a atualização do stock é coordenada pelo processo de registo da compra.

Essa separação preserva o histórico das aquisições e mantém bem definidas as responsabilidades entre produto, item e compra.