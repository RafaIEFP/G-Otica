## 1. Contexto
A entidade `ItemVenda` representa cada produto incluído em uma venda.

Ela faz a ligação entre a entidade `Venda` e a entidade `Produto`, permitindo que uma mesma venda contenha vários produtos diferentes.

Cada registo representa uma única linha da venda.

---

## 2. Responsabilidade
A responsabilidade da entidade `ItemVenda` é armazenar as informações específicas de cada produto vendido.

Essas informações incluem:
- Produto vendido;
- Quantidade;
- Valor unitário;
- Desconto aplicado;
- Valor total do item;
- Observações específicas.

Quando o produto corresponde a uma lente graduada, o `ItemVenda` pode possuir um `ItemLente` associado com as informações necessárias para sua fabricação.

---

## 3. Decisões de Modelagem

#### ItemVenda faz a ligação entre Venda e Produto
Foi adotada uma entidade intermediária para representar os produtos de uma venda.

Essa abordagem permite que uma venda contenha qualquer quantidade de produtos.

---

#### Informações do item permanecem no ItemVenda

Dados como:
- quantidade;
- desconto;
- valor unitário;
- valor total;

pertencem ao item da venda e não ao cadastro do produto.

Isso garante que alterações futuras no preço de um produto não afetem vendas já realizadas.

---

#### Informações específicas de lentes não pertencem ao ItemVenda
Quando o produto vendido é uma lente, as informações necessárias para sua fabricação são armazenadas na entidade `ItemLente`.

Essa separação mantém o `ItemVenda` simples e aplicável a qualquer tipo de produto.

---

## 4. Benefícios
A modelagem adotada permite:

- representar vendas com vários produtos;
- armazenar o histórico correto dos preços praticados;
- manter separadas as informações comerciais e as informações específicas da fabricação das lentes;
- reutilizar o mesmo modelo para qualquer tipo de produto.

---

## 5. Conclusão

A entidade `ItemVenda` representa cada linha de uma venda.

Sua responsabilidade é registrar as informações comerciais de cada produto vendido, enquanto detalhes especializados permanecem nas entidades responsáveis, como `ItemLente`.