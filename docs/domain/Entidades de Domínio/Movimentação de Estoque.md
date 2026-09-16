## 1. Contexto

A entidade `StockMovement` representa uma alteração ocorrida no estoque de um produto.

Embora a entidade `Produto` mantenha a quantidade atual disponível através de `StockQuantity`, esse valor representa apenas o estado atual do estoque e não explica como essa quantidade foi alcançada.

Durante o funcionamento da ótica, o estoque de um produto pode ser alterado por diferentes operações, como:

- cadastro inicial do produto;
    
- compra de produtos;
    
- venda de produtos;
    
- ajustes manuais de estoque;
    
- cancelamento de vendas.
    

Por esse motivo, foi criada uma entidade específica para registrar cada movimentação realizada.

A `StockMovement` funciona como o histórico do estoque, permitindo identificar quanto foi adicionado ou removido, qual operação originou a alteração, quando ela ocorreu e qual utilizador foi responsável pela operação.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Toda alteração de estoque deve possuir uma origem

O estoque de um produto não deve ser alterado arbitrariamente.

Cada alteração deve estar associada a uma operação que explique a origem da movimentação.

As origens atualmente suportadas são:

- estoque inicial;
    
- compra;
    
- venda;
    
- ajuste manual;
    
- cancelamento de venda.
    

---

#### Compras aumentam o estoque

Quando uma compra é registada, a quantidade adquirida é adicionada ao estoque atual do produto.

Exemplo:

```text
Estoque atual: 10
Compra:         +5
Estoque final:  15
```

Essa alteração gera uma movimentação de estoque do tipo `Purchase`.

Caso o mesmo produto apareça em mais de um `ItemCompra` dentro da mesma compra, as quantidades são somadas para calcular o impacto total no stock.

Exemplo:

```text
Produto A
Item 1: +5
Item 2: +3

Movimentação de estoque:
Purchase +8
```

Os itens podem permanecer separados na compra, por exemplo para representar preços de aquisição diferentes, enquanto a movimentação representa o impacto total no stock.

---

#### Vendas diminuem o estoque

Quando uma venda é registada, a quantidade vendida é removida do estoque atual do produto.

Exemplo:

```text
Estoque atual: 10
Venda:          -3
Estoque final:   7
```

Essa alteração gera uma movimentação de estoque do tipo `Sale`.

Caso o mesmo produto apareça em vários itens da venda, as quantidades são agrupadas para determinar o impacto total no stock.

O estoque de um produto não pode tornar-se negativo através de uma venda.

---

#### O estoque pode ser ajustado manualmente

Além das movimentações provenientes das operações comerciais, a ótica pode precisar corrigir diferenças entre o estoque registado no sistema e o estoque físico.

Exemplos:

- produto danificado;
    
- produto perdido;
    
- erro de contagem;
    
- correção de cadastro;
    
- diferença identificada durante inventário.
    

Um ajuste manual pode aumentar ou diminuir a quantidade disponível.

O tipo do ajuste indica se a quantidade informada representa uma entrada ou uma saída.

---

#### Ajustes manuais devem possuir uma justificativa

Diferentemente das movimentações cuja própria operação já explica a alteração, um ajuste manual deve informar o motivo pelo qual o estoque foi modificado.

Exemplo:

```text
Estoque atual: 10
Ajuste:         -2
Motivo: "Duas armações danificadas"
Estoque final:   8
```

A justificativa é armazenada através de `Reason`.

Nos demais tipos de movimentação, essa propriedade pode permanecer vazia.

---

#### Um ajuste manual não pode deixar o estoque negativo

Quando o ajuste representa uma redução, a quantidade removida não pode ser superior ao stock disponível.

Exemplo:

```text
Estoque atual: 5
Ajuste solicitado: -7

Resultado:
Operação rejeitada
```

---

#### O estoque inicial também faz parte do histórico

Quando um produto é cadastrado com uma quantidade inicial maior que zero, essa quantidade é registada como a primeira movimentação de estoque.

Exemplo:

```text
Produto cadastrado
Quantidade inicial: 20

Movimentação:
Tipo: InitialStock
Quantidade: +20
```

Caso o produto seja cadastrado com stock igual a zero, não é criada uma movimentação inicial.

---

#### O cancelamento de uma venda pode restaurar o estoque

Quando uma venda é cancelada, os produtos que retornam ao stock geram uma movimentação do tipo:

```text
SaleCancellation
```

A quantidade da movimentação é positiva, pois representa uma entrada no stock.

Entretanto, nem todos os itens são necessariamente restaurados.

Se a venda ainda estiver em `Confirmed`, todos os itens podem retornar ao stock.

Se a venda já estiver em `InProduction` ou `Ready`, itens comuns retornam ao stock, mas itens que possuem `ItemLens` não são restaurados, pois representam lentes personalizadas cuja produção já foi iniciada.

Exemplo:

```text
Venda:
- 1 armação
- 1 lente personalizada

Cancelamento em InProduction:

Armação:
SaleCancellation +1

Lente personalizada:
Nenhuma movimentação de retorno
```

Dessa forma, o histórico de stock continua refletindo corretamente que aquela lente não voltou a estar disponível para comercialização.

---

#### Toda movimentação pertence a um produto

Uma movimentação de estoque existe sempre em relação a um produto específico.

Por esse motivo, `StockMovement` mantém uma referência através de `ProductId`.

Como cada produto pertence a uma ótica, a movimentação também permanece dentro do contexto da mesma unidade de forma indireta.

---

#### A movimentação registra o utilizador responsável

Cada movimentação identifica o utilizador responsável pela operação através de `UserId`.

Em uma movimentação manual, representa diretamente quem realizou o ajuste.

Em operações como:

- cadastro inicial;
    
- compra;
    
- venda;
    
- cancelamento de venda;
    

representa o utilizador responsável pela operação que originou a alteração.

---

#### Movimentações representam histórico

Uma movimentação de estoque representa um evento que já ocorreu.

Por esse motivo, movimentações existentes não devem ser alteradas ou removidas para corrigir o estoque.

Caso seja necessário corrigir uma alteração anterior, uma nova movimentação deve ser registada através da operação adequada.

Exemplo:

```text
Movimentação anterior: +10

Correção necessária: -2

Histórico:
+10
-2
```

O estado atual do stock passa a refletir a correção sem apagar o histórico anterior.

---

## 3. Decisões de Modelagem

#### A quantidade atual continua armazenada em Produto

Foi decidido manter `StockQuantity` dentro da entidade `Product`.

Esse valor representa a quantidade atualmente disponível e permite consultar o estoque sem precisar calcular todas as movimentações anteriores.

Exemplo:

```text
Product.StockQuantity = 12
```

A entidade `StockMovement`, por outro lado, explica como essa quantidade foi alcançada.

Essa abordagem separa:

```text
Product.StockQuantity
→ estado atual
```

de:

```text
StockMovement
→ histórico das alterações
```

---

#### A movimentação armazena a variação do estoque

Foi decidido armazenar a alteração realizada através de `QuantityChange`.

Valores positivos representam entradas.

Valores negativos representam saídas.

Exemplo:

```text
InitialStock       +10
Purchase            +5
Sale                -2
ManualAdjustment    -1
SaleCancellation    +2
```

Dessa forma, a própria movimentação representa simultaneamente a quantidade e a direção da alteração.

---

#### O tipo identifica a origem da movimentação

A origem é representada pelo enum `StockMovementType`.

Os valores atualmente existentes são:

```text
InitialStock
Purchase
Sale
ManualAdjustment
SaleCancellation
```

Cada valor está relacionado a um fluxo específico do domínio.

---

#### Movimentações são criadas pelas operações que alteram o estoque

`StockMovement` não é criada livremente através de um fluxo genérico de criação.

Cada operação responsável por modificar o estoque também cria a movimentação correspondente.

Atualmente:

```text
RegisterProduct
→ InitialStock

RegisterPurchase
→ Purchase

RegisterSale
→ Sale

AdjustProductStock
→ ManualAdjustment

CancelSale
→ SaleCancellation
```

Essa decisão evita movimentações sem uma operação de negócio que as justifique.

---

#### O ajuste manual possui um fluxo próprio

Como ajustes manuais representam uma ação realizada diretamente sobre o estoque, existe um fluxo específico para essa operação.

Esse fluxo é responsável por:

1. validar o produto;
    
2. validar o tipo do ajuste;
    
3. validar a quantidade;
    
4. validar a justificativa;
    
5. garantir que uma redução não deixe o estoque negativo;
    
6. atualizar `Product.StockQuantity`;
    
7. criar a `StockMovement` correspondente.
    

---

#### A justificativa é específica do ajuste manual

`Reason` é opcional na entidade porque nem todas as movimentações necessitam de uma descrição adicional.

Operações como:

```text
Purchase
Sale
InitialStock
SaleCancellation
```

já possuem uma origem suficientemente definida pelo próprio tipo.

No caso de `ManualAdjustment`, entretanto, `Reason` é obrigatório para explicar por que a alteração foi realizada.

---

#### Atualização do estoque e criação da movimentação são atômicas

A alteração de `Product.StockQuantity` e a criação de `StockMovement` fazem parte da mesma operação de negócio.

Por esse motivo, são executadas dentro de uma transação.

Não deve ocorrer:

```text
Stock atualizado
Movimentação não registada
```

nem:

```text
Movimentação registada
Stock não atualizado
```

Caso alguma etapa da operação falhe, as alterações realizadas naquela transação são revertidas.

Essa regra é aplicada nos principais fluxos que alteram stock, incluindo:

- criação do produto com stock inicial;
    
- compra;
    
- venda;
    
- ajuste manual;
    
- cancelamento de venda.
    

---

#### A movimentação não substitui Compra ou Venda

`StockMovement` regista apenas o impacto de uma operação sobre o estoque.

Ela não substitui entidades como:

- `Compra`;
    
- `Venda`;
    
- `ItemCompra`;
    
- `ItemVenda`.
    

Essas entidades continuam responsáveis pelas informações comerciais da operação.

A movimentação possui somente a responsabilidade de registar a alteração ocorrida no stock.

---

#### O cancelamento não cria uma nova saída para itens que não retornam ao stock

Quando uma lente personalizada já entrou em produção e uma venda é cancelada, esse item não retorna ao stock.

Entretanto, também não é criada uma nova movimentação negativa.

A saída original já foi registada no momento da venda através de:

```text
Sale -1
```

Criar outra saída durante o cancelamento reduziria o stock duas vezes.

Por isso:

```text
Lente vendida:
Sale -1

Cancelamento após início da produção:
nenhuma nova movimentação para a lente
```

Somente produtos efetivamente restaurados recebem uma movimentação positiva `SaleCancellation`.

---

#### As movimentações podem ser consultadas

O histórico das movimentações de um produto pode ser consultado.

A consulta é realizada por produto e retorna os registos de forma paginada.

As movimentações são ordenadas da mais recente para a mais antiga.

Atualmente, cada registo consultado inclui informações como:

- identificador da movimentação;
    
- quantidade alterada;
    
- tipo;
    
- justificativa, quando existente;
    
- data da movimentação;
    
- utilizador responsável.
    

O produto deve pertencer à ótica onde a consulta está sendo realizada.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Mantém o histórico das alterações de estoque.
    
- Permite identificar a origem de cada alteração.
    
- Permite identificar quem realizou cada operação.
    
- Evita alterações arbitrárias em `StockQuantity`.
    
- Facilita a identificação de divergências de inventário.
    
- Permite realizar ajustes manuais sem apagar o histórico.
    
- Preserva o impacto de compras, vendas e cancelamentos.
    
- Permite distinguir o stock atual do histórico que o originou.
    
- Mantém compras, vendas e stock com responsabilidades separadas.
    
- Facilita futuras auditorias.
    
- Permite construir relatórios de entrada e saída de produtos.
    
- Mantém a quantidade atual disponível para consultas rápidas através de `Product.StockQuantity`.
    

---

## 5. Possíveis Evoluções

A entidade poderá evoluir futuramente caso surjam novas necessidades relacionadas ao controlo de estoque.

Exemplos:

- classificação específica dos motivos de ajuste;
    
- inventários periódicos;
    
- transferência de estoque entre óticas;
    
- reservas de produtos;
    
- estoque mínimo;
    
- alertas de baixo estoque;
    
- referência direta à operação que originou a movimentação;
    
- lotes de produtos;
    
- localização física do produto dentro da ótica;
    
- relatórios avançados de movimentação;
    
- controlo de estoque reservado e disponível.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `StockMovement` representa o histórico das alterações realizadas no estoque dos produtos.

Enquanto `Product.StockQuantity` mantém a quantidade atualmente disponível, `StockMovement` registra como essa quantidade foi modificada ao longo do tempo.

As movimentações podem atualmente ser originadas por:

- cadastro inicial;
    
- compra;
    
- venda;
    
- ajuste manual;
    
- cancelamento de venda.
    

Valores positivos representam entradas no stock e valores negativos representam saídas.

No cancelamento de vendas, apenas produtos efetivamente devolvidos ao stock recebem uma movimentação `SaleCancellation`, preservando corretamente o comportamento de lentes personalizadas que já entraram em produção.

Essa separação mantém o estoque atual simples de consultar sem perder o histórico das operações que o modificaram e garante que as alterações permaneçam rastreáveis e associadas aos fluxos de negócio responsáveis.