## 1. Contexto

A entidade `Compra` representa o registo da aquisição de produtos pela ótica junto aos seus fornecedores.

O seu objetivo é controlar quais produtos foram adquiridos, de qual fornecedor, por qual utilizador e para qual unidade, além de refletir essa aquisição no stock da ótica.

Assim como uma venda é composta por diversos itens, uma compra também é formada por um ou mais `ItemCompra`.

Na implementação atual, `Compra` representa a aquisição já registada no sistema e não um pedido de compra com etapas de envio e recebimento.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Uma compra pertence a uma única ótica

Cada compra é registada dentro do contexto de uma ótica específica.

Mesmo que um proprietário possua várias óticas, cada compra pertence exclusivamente à unidade responsável pela aquisição.

Os produtos e o fornecedor utilizados na operação também devem pertencer a essa mesma ótica.

---

#### Uma compra é realizada junto a um fornecedor

Toda compra possui um único fornecedor responsável pelo fornecimento dos produtos.

Ao longo do tempo, um mesmo fornecedor pode participar de diversas compras.

Para ser utilizado numa nova compra, o fornecedor deve:

- existir;
    
- pertencer à ótica da operação;
    
- estar ativo.
    

---

#### Uma compra é registada por um utilizador

Cada compra mantém uma referência ao utilizador responsável pelo seu registo no sistema.

Essa informação permite preservar a autoria da operação.

O `UserId` não determina o pertencimento da compra, que continua sendo definido através da ótica.

---

#### Uma compra deve possuir pelo menos um item

Uma compra não pode ser registada sem produtos.

Cada produto adquirido é representado através de um `ItemCompra`.

---

#### Uma compra pode conter vários produtos

Uma compra pode incluir diferentes produtos em uma mesma operação.

Exemplos:

- lentes;
    
- armações;
    
- estojos;
    
- flanelas;
    
- acessórios.
    

Cada produto adquirido é representado por um `ItemCompra`, contendo a quantidade e o preço unitário daquela aquisição.

---

#### Apenas produtos ativos da mesma ótica podem ser adquiridos

Os produtos incluídos numa compra devem:

- existir;
    
- pertencer à mesma ótica da compra;
    
- estar ativos.
    

Produtos inexistentes, inativos ou pertencentes a outra unidade não podem ser utilizados numa nova compra.

---

#### Quantidade e preço são definidos por ItemCompra

Cada item da compra possui:

- produto;
    
- quantidade;
    
- preço unitário;
    
- valor total.
    

A quantidade deve ser superior a zero.

O preço unitário não pode ser negativo.

O valor total do item é calculado através de:

```text
Quantidade × Preço Unitário
```

---

#### O valor total da compra é calculado a partir dos itens

A entidade `Compra` mantém o seu `TotalAmount`.

Esse valor não é informado diretamente pelo utilizador.

Ele é calculado através da soma dos valores totais dos `ItemCompra`.

```text
Compra.TotalAmount
=
Soma dos ItemCompra.TotalAmount
```

---

#### O registo de uma compra aumenta o stock

Na implementação atual, o registo da compra representa também a entrada dos produtos no stock da ótica.

A quantidade adquirida de cada produto é adicionada ao respetivo `StockQuantity`.

Caso o mesmo produto apareça mais de uma vez na operação, o impacto no stock é agrupado pela quantidade total adquirida.

---

#### A entrada em stock gera uma Movimentação de Estoque

Para cada produto afetado pela compra é criada uma `Movimentação de Estoque` do tipo:

```text
Purchase
```

A movimentação regista:

- produto;
    
- utilizador responsável;
    
- quantidade adicionada;
    
- tipo da movimentação.
    

Dessa forma, `Product.StockQuantity` representa o estado atual, enquanto `StockMovement` preserva o histórico da alteração.

---

#### O registo da compra e a atualização do stock fazem parte da mesma operação

A criação da compra, o aumento do stock e as respetivas movimentações são executados dentro de uma transação.

Dessa forma, a operação deve ser concluída de forma consistente.

Caso alguma etapa falhe, as alterações realizadas durante o registo não devem permanecer parcialmente aplicadas.

---

## 3. Decisões de Modelagem

#### Compra foi separada da Venda

Inicialmente foi considerada a utilização de uma única entidade para representar compras e vendas.

Essa abordagem foi descartada.

Embora ambas representem operações comerciais, os seus objetivos, participantes e regras de negócio são diferentes.

A `Compra` representa uma aquisição realizada pela ótica junto a um fornecedor.

A `Venda` representa uma operação realizada pela ótica para um cliente.

A separação torna o domínio mais claro e permite que cada processo evolua de forma independente.

---

#### Os produtos da compra são representados por ItemCompra

A entidade `Compra` não possui uma coleção direta de produtos sem informações adicionais.

Cada produto adquirido é representado por um `ItemCompra`, permitindo armazenar dados específicos daquela aquisição:

- quantidade;
    
- preço unitário;
    
- valor total.
    

Essa estrutura também preserva o preço histórico pago pelo produto.

Uma alteração posterior no cadastro do `Produto` não modifica o preço registado em compras anteriores.

---

#### O preço de compra não altera o preço base do Produto

`PurchaseItem.UnitPrice` representa o valor pago pelo produto naquela aquisição específica.

`Product.BasePrice` possui outra responsabilidade e não é automaticamente atualizado quando uma compra é registada.

Dessa forma, o sistema mantém separados:

```text
PurchaseItem.UnitPrice
→ preço histórico de aquisição
```

e:

```text
Product.BasePrice
→ preço base atual do produto
```

---

#### A Compra não possui Receita

Diferentemente das vendas de lentes personalizadas, compras realizadas junto aos fornecedores não dependem de receita.

Por esse motivo, não existe relacionamento entre `Compra` e `Receita`.

---

#### A Compra não possui informações específicas de lentes

Durante uma compra, a ótica adquire produtos para o seu stock.

Informações relacionadas à personalização de lentes, como:

- olho correspondente;
    
- DP;
    
- DNP;
    
- índice de refração;
    
- tratamentos;
    
- material;
    
- cor;
    
- diâmetro;
    

não fazem parte desse processo.

Essas informações pertencem ao `ItemLente` criado durante uma venda de lentes para um cliente específico.

---

#### A Compra não possui ciclo de estados na implementação atual

A entidade não possui atualmente propriedades como:

```text
Status
ReceivedAt
ExpectedDeliveryDate
```

Por esse motivo, o sistema não diferencia etapas como:

```text
Pedido criado
→ Enviado
→ Recebido
```

O registo da compra representa diretamente a aquisição dos produtos e provoca a respetiva entrada em stock.

Caso futuramente seja necessário controlar pedidos antes do recebimento dos produtos, será necessário introduzir um ciclo de vida específico para a compra.

---

#### O pagamento da compra não é controlado atualmente

O sistema não possui uma entidade de pagamento associada às compras realizadas junto aos fornecedores.

O objetivo atual é controlar a aquisição dos produtos e o impacto no stock.

Questões financeiras como:

- valor já pago ao fornecedor;
    
- pagamentos parcelados;
    
- vencimentos;
    
- contas a pagar;
    

não fazem parte do modelo atual.

---

## 4. Fluxo da Compra

Na implementação atual, o processo ocorre da seguinte maneira.

1. O utilizador seleciona um fornecedor ativo da ótica.
    
2. Informa os produtos, quantidades e preços de aquisição.
    
3. O sistema valida o fornecedor e os produtos.
    
4. São criados os respetivos `ItemCompra`.
    
5. O valor de cada item é calculado.
    
6. O valor total da compra é calculado.
    
7. A compra é registada.
    
8. As quantidades adquiridas são adicionadas ao stock dos produtos.
    
9. São criadas as respetivas movimentações de stock do tipo `Purchase`.
    

O registo da compra, a atualização do stock e as movimentações são realizados dentro da mesma transação.

---

## 5. Benefícios

A modelagem adotada oferece diversas vantagens.

- Separa claramente os processos de compra e venda.
    
- Permite controlar o histórico de aquisições junto aos fornecedores.
    
- Preserva o preço histórico de cada produto adquirido.
    
- Identifica o utilizador responsável pelo registo.
    
- Mantém as operações isoladas por ótica.
    
- Atualiza o stock como parte do processo de compra.
    
- Mantém histórico das entradas através de `Movimentação de Estoque`.
    
- Garante consistência entre a compra e a alteração do stock através de transação.
    
- Evita misturar regras de abastecimento com regras de venda ao cliente.
    

---

## 6. Possíveis Evoluções

Dependendo da evolução do sistema, a entidade poderá incorporar novas funcionalidades.

Exemplos:

- ciclo de estados da compra;
    
- criação de pedidos antes do recebimento;
    
- prazo previsto para entrega;
    
- data efetiva de recebimento;
    
- número da fatura ou documento equivalente;
    
- recebimento parcial de mercadorias;
    
- cancelamento de pedidos;
    
- devoluções ao fornecedor;
    
- controlo de pagamentos ao fornecedor;
    
- contas a pagar;
    
- histórico de alterações de estado.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 7. Conclusão

A entidade `Compra` representa a aquisição de produtos realizada pela ótica junto a um fornecedor.

Ela mantém informações sobre o fornecedor, o utilizador responsável, a ótica, os produtos adquiridos, os respetivos preços e o valor total da operação.

Cada produto é representado através de `ItemCompra`, preservando a quantidade e o preço praticado naquela aquisição.

Na implementação atual, o registo da compra provoca imediatamente a entrada dos produtos no stock e a criação das respetivas `Movimentações de Estoque`.

A entidade não possui atualmente um ciclo de estados para representar pedido, envio ou recebimento, nem controla pagamentos realizados ao fornecedor.

Essa modelagem atende ao processo atual de abastecimento da ótica, mantendo o histórico das aquisições e a consistência com o controlo de stock.