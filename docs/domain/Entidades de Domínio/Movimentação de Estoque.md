## 1. Contexto

A entidade `StockMovement` representa uma alteração ocorrida no estoque de um produto.

Embora a entidade `Produto` mantenha a quantidade atual disponível através de `StockQuantity`, esse valor representa apenas o estado atual do estoque e não explica como essa quantidade foi alcançada.

Durante o funcionamento da ótica, o estoque de um produto pode ser alterado por diferentes operações, como:

- cadastro inicial do produto;
- compra de produtos;
- venda de produtos;
- ajustes manuais de estoque.

Por esse motivo, foi criada uma entidade específica para registrar cada movimentação realizada.

A `StockMovement` funciona como o histórico do estoque, permitindo identificar quanto foi adicionado ou removido, qual operação originou a alteração, quando ela ocorreu e qual utilizador foi responsável pela operação.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Toda alteração de estoque deve possuir uma origem

O estoque de um produto não deve ser alterado arbitrariamente.

Cada alteração deve estar associada a uma operação que explique o motivo da movimentação.

As principais origens consideradas inicialmente são:

- estoque inicial;
- compra;
- venda;
- ajuste manual.

---

#### Compras aumentam o estoque

Quando uma compra de produtos for efetivada, a quantidade adquirida deve ser adicionada ao estoque atual do produto.

Exemplo:

```text
Estoque atual: 10
Compra:         +5
Estoque final:  15
````

Essa alteração gera uma movimentação de estoque do tipo `Purchase`.

---

#### Vendas diminuem o estoque

Quando uma venda for efetivada, a quantidade vendida deve ser removida do estoque atual do produto.

Exemplo:

```
Estoque atual: 10
Venda:          -3
Estoque final:   7
```

Essa alteração gera uma movimentação de estoque do tipo `Sale`.

O estoque de um produto não pode tornar-se negativo.

---

#### O estoque pode ser ajustado manualmente

Além das movimentações provenientes de compras e vendas, a ótica pode precisar corrigir diferenças entre o estoque registado no sistema e o estoque físico.

Exemplos:

- produto danificado;
- produto perdido;
- erro de contagem;
- correção de cadastro;
- diferença identificada durante inventário.

Um ajuste manual pode aumentar ou diminuir a quantidade disponível.

---

#### Ajustes manuais devem possuir uma justificativa

Diferentemente das movimentações provenientes de compras e vendas, cuja origem já explica a alteração, um ajuste manual deve informar o motivo pelo qual o estoque foi modificado.

Exemplo:

```
Estoque atual: 10
Ajuste:         -2
Motivo: "Duas armações danificadas"
Estoque final:   8
```

---

#### O estoque inicial também faz parte do histórico

Quando um produto for cadastrado com uma quantidade inicial maior que zero, essa quantidade deve ser registrada como a primeira movimentação de estoque.

Exemplo:

```
Produto cadastrado
Quantidade inicial: 20

Movimentação:
Tipo: InitialStock
Quantidade: +20
```

Caso o produto seja cadastrado com estoque igual a zero, não é necessário criar uma movimentação inicial.

---

#### Toda movimentação pertence a um produto

Uma movimentação de estoque existe sempre em relação a um produto específico.

Por esse motivo, `StockMovement` mantém uma referência para `Product`.

Como cada produto pertence a uma ótica, a movimentação também permanece dentro do contexto da mesma unidade.

---

#### A movimentação registra o utilizador responsável

Cada movimentação deve identificar o utilizador responsável pela operação que alterou o estoque.

Em uma movimentação manual, representa diretamente o utilizador que realizou o ajuste.

Em operações como compra e venda, representa o utilizador responsável pela operação que originou a alteração.

---

#### Movimentações representam histórico

Uma movimentação de estoque representa um evento que já ocorreu.

Por esse motivo, movimentações existentes não devem ser alteradas ou removidas para corrigir o estoque.

Caso seja necessário corrigir uma movimentação anterior, uma nova movimentação deve ser registrada.

Exemplo:

```
Movimentação incorreta: +10

Correção necessária: -2

Histórico:
+10
-2
```

O resultado do estoque passa a refletir a correção sem apagar o histórico anterior.

---

## 3. Decisões de Modelagem

#### A quantidade atual continua armazenada em Produto

Foi decidido manter `StockQuantity` dentro da entidade `Product`.

Esse valor representa a quantidade atual disponível e permite consultar o estoque sem precisar calcular todas as movimentações anteriores.

Exemplo:

```
Product.StockQuantity = 12
```

A entidade `StockMovement`, por outro lado, explica como essa quantidade foi alcançada.

Essa abordagem separa:

```
Product.StockQuantity
→ estado atual

StockMovement
→ histórico das alterações
```

---

#### A movimentação armazena a variação do estoque

Foi decidido armazenar a alteração realizada através de `QuantityChange`.

Valores positivos representam entrada de produtos.

Valores negativos representam saída de produtos.

Exemplo:

```
InitialStock       +10
Purchase            +5
Sale                -2
ManualAdjustment    -1
```

Dessa forma, a própria movimentação indica tanto a quantidade quanto a direção da alteração.

---

#### O tipo identifica a origem da movimentação

Foi definido um tipo de movimentação para identificar a operação responsável pela alteração.

Inicialmente serão considerados:

```
InitialStock
Purchase
Sale
ManualAdjustment
```

Essa informação será representada através de um enum `StockMovementType`.

---

#### Movimentações são criadas pelas operações que alteram o estoque

`StockMovement` não será criada livremente através de um endpoint genérico.

Cada operação responsável por modificar o estoque deve criar a movimentação correspondente.

Exemplos:

```
RegisterProduct
→ InitialStock

Purchase
→ Purchase

Sale
→ Sale

AdjustProductStock
→ ManualAdjustment
```

Essa decisão evita a criação de movimentações sem uma operação de negócio que as justifique.

---

#### O ajuste manual possui um fluxo próprio

Como ajustes manuais representam uma ação realizada diretamente sobre o estoque, será criado um fluxo específico para essa operação.

Esse fluxo será responsável por:

1. validar o produto;
2. validar a quantidade;
3. garantir que o estoque não ficará negativo;
4. atualizar `Product.StockQuantity`;
5. registrar uma `StockMovement`;
6. guardar a justificativa do ajuste.

---

#### Atualização do estoque e criação da movimentação são atômicas

A alteração de `Product.StockQuantity` e a criação de `StockMovement` fazem parte da mesma operação de negócio.

Por esse motivo, ambas devem ocorrer dentro da mesma transação.

Não deve ser possível ocorrer:

```
Stock atualizado
Movimentação não registrada
```

nem:

```
Movimentação registrada
Stock não atualizado
```

Caso qualquer etapa falhe, toda a operação deve ser revertida.

---

#### A movimentação não substitui Compra ou Venda

`StockMovement` registra apenas o impacto de uma operação sobre o estoque.

Ela não substitui entidades como `Compra`, `Venda`, `ItemCompra` ou `ItemVenda`.

Essas entidades continuam responsáveis pelas informações comerciais da operação.

A movimentação possui somente a responsabilidade de registrar a alteração ocorrida no estoque.

---

#### Movimentações poderão ser consultadas

Embora movimentações não possam ser criadas diretamente pelo cliente da API, seu histórico poderá ser consultado.

Isso permite visualizar todas as alterações ocorridas no estoque de determinado produto.

Exemplo:

```
Produto: Armação XP10

+10  InitialStock
 +5  Purchase
 -2  Sale
 -1  ManualAdjustment
 -3  Sale
```

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Mantém o histórico completo das alterações de estoque.
- Permite identificar por que o estoque foi alterado.
- Permite identificar quem realizou cada operação.
- Evita alterações arbitrárias em `StockQuantity`.
- Facilita a identificação de erros e divergências de inventário.
- Permite realizar ajustes manuais sem perder o histórico.
- Mantém compras, vendas e estoque com responsabilidades separadas.
- Facilita futuras auditorias.
- Permite construir relatórios de entrada e saída de produtos.
- Mantém a quantidade atual disponível para consultas rápidas através de `Product.StockQuantity`.

---

## 5. Possíveis Evoluções

A entidade poderá evoluir futuramente caso surjam novas necessidades relacionadas ao controle de estoque.

Exemplos:

- classificação específica dos motivos de ajuste;
- inventários periódicos;
- transferência de estoque entre óticas;
- reservas de produtos;
- estoque mínimo;
- alertas de baixo estoque;
- identificação do documento ou operação que originou a movimentação;
- lotes de produtos;
- localização física do produto dentro da ótica;
- relatórios avançados de movimentação;
- controle de estoque reservado e disponível.

Essas funcionalidades não fazem parte do escopo inicial.

---

## 6. Conclusão

A entidade `StockMovement` representa o histórico das alterações realizadas no estoque dos produtos.

Enquanto `Product.StockQuantity` mantém a quantidade atual disponível, `StockMovement` registra como essa quantidade foi modificada ao longo do tempo.

As movimentações podem ser originadas pelo cadastro inicial do produto, compras, vendas ou ajustes manuais.

Essa separação permite manter o estoque atual simples de consultar sem perder o histórico das operações que o modificaram.

Além disso, a modelagem garante que alterações de estoque sejam rastreáveis, justificáveis e associadas às operações responsáveis, preparando o sistema para futuros recursos de inventário, auditoria e relatórios.