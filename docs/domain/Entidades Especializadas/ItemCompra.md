## 1. Contexto
A entidade `ItemCompra` representa cada produto incluído em uma compra realizada pela ótica junto a um fornecedor.

Ela faz a ligação entre a entidade `Compra` e a entidade `Produto`, permitindo que uma mesma compra seja composta por diversos produtos.

Cada registo representa uma única linha da compra.

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

## 3. Decisões de Modelagem

#### ItemCompra faz a ligação entre Compra e Produto
Foi adotada uma entidade intermediária para representar os produtos de uma compra.

Essa abordagem permite que uma compra contenha qualquer quantidade de produtos.

---

#### Informações do item permanecem no ItemCompra

Dados como:
- quantidade;
- valor unitário;
- valor total;

pertencem ao item da compra e não ao cadastro do produto.

Isso garante que alterações futuras no preço de um produto não afetem o histórico das compras já realizadas.

---

#### ItemCompra não possui informações específicas de fabricação
Embora alguns produtos adquiridos sejam lentes, a compra representa apenas a aquisição desses produtos junto ao fornecedor.

Informações relacionadas à personalização de uma lente para um cliente, como DP, DNP, índice ou tratamentos, não fazem parte desse processo e, por isso, não pertencem ao `ItemCompra`.

---

## 4. Benefícios
A modelagem adotada permite:

- representar compras com diversos produtos;
- manter o histórico dos valores pagos aos fornecedores;
- separar corretamente as informações da compra das informações do produto;
- reutilizar o mesmo cadastro de produtos utilizado nas vendas.

---

## 5. Conclusão

A entidade `ItemCompra` representa cada linha de uma compra realizada pela ótica.

Sua responsabilidade é registrar as informações comerciais de cada produto adquirido, preservando o histórico das compras e mantendo a separação entre o processo de abastecimento do estoque e o cadastro dos produtos.