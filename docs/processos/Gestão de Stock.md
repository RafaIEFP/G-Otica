## Objetivo

Descrever o processo de controlo e acompanhamento do stock dos produtos de uma ótica.

O stock atual de cada produto é mantido através de:

```text
Product.StockQuantity
```

Enquanto isso, o histórico das alterações é preservado através de:

```text
StockMovement
```

O processo de gestão de stock abrange:

- stock inicial;
    
- entrada através de compras;
    
- saída através de vendas;
    
- ajustes manuais;
    
- reposição decorrente de cancelamento de vendas;
    
- consulta do histórico de movimentações.
    

---

## Participantes

- Utilizador da ótica;
    
- Sistema.
    

---

# 1. Estrutura do Stock

Cada produto possui uma quantidade atual disponível:

```text
Product.StockQuantity
```

Esse valor representa o estado atual do stock e permite consultas sem a necessidade de recalcular todo o histórico.

As alterações que originaram essa quantidade são registadas separadamente em `StockMovement`.

Assim:

```text
Product.StockQuantity
→ quantidade atual
```

```text
StockMovement
→ histórico das alterações
```

---

# 2. Tipos de Movimentação

As movimentações atualmente existentes são:

```text
InitialStock
Purchase
Sale
ManualAdjustment
SaleCancellation
```

Cada tipo representa a origem da alteração realizada no stock.

---

# 3. Quantidade da Movimentação

`StockMovement.QuantityChange` armazena a variação provocada no stock.

Valores positivos representam entradas:

```text
+10
+5
+1
```

Valores negativos representam saídas:

```text
-1
-2
-5
```

Exemplo:

```text
InitialStock       +10
Purchase            +5
Sale                -3
ManualAdjustment    -1
SaleCancellation    +2
```

---

# 4. Stock Inicial

O primeiro impacto no stock pode ocorrer durante o registo de um produto.

---

## Registo com Stock Superior a Zero

Caso o produto seja criado com:

```text
StockQuantity > 0
```

o sistema cria uma movimentação:

```text
Type = InitialStock
QuantityChange = StockQuantity
```

Exemplo:

```text
Novo produto
Stock inicial: 20
```

gera:

```text
InitialStock +20
```

---

## Registo com Stock Igual a Zero

Caso:

```text
StockQuantity = 0
```

não é criada uma movimentação de stock.

O produto simplesmente inicia com quantidade zero.

---

## Consistência

A criação do produto e da movimentação inicial fazem parte da mesma operação transacional.

Não deve ocorrer:

```text
Produto criado com stock 20
Movimentação inicial inexistente
```

caso a operação tenha sido concluída com sucesso.

---

# 5. Entrada de Stock por Compra

O registo de uma compra aumenta o stock dos produtos adquiridos.

---

## Pré-condições

Para que um produto participe de uma compra:

- deve existir;
    
- deve pertencer à ótica;
    
- deve estar ativo.
    

---

## Fluxo

1. A compra é validada.
    
2. Os seus `PurchaseItem` são criados.
    
3. As quantidades são agrupadas por `ProductId`.
    
4. O stock de cada produto é aumentado.
    
5. É criada uma movimentação do tipo `Purchase`.
    
6. A compra, as alterações de stock e as movimentações são persistidas na mesma transação.
    

---

## Produtos Repetidos na Compra

O mesmo produto pode aparecer em mais de um `PurchaseItem`.

Exemplo:

```text
Produto A
5 unidades × 10 €

Produto A
3 unidades × 9 €
```

Os itens continuam separados na compra:

```text
PurchaseItem 1 → 5
PurchaseItem 2 → 3
```

mas o impacto no stock é agrupado:

```text
Purchase +8
```

Assim:

```text
Stock atual: 10
Compra:      +8
Stock final: 18
```

---

# 6. Saída de Stock por Venda

O registo de uma venda reduz o stock dos produtos vendidos.

---

## Pré-condições

Para que um produto seja vendido:

- deve existir;
    
- deve pertencer à ótica;
    
- deve estar ativo;
    
- deve possuir stock suficiente.
    

---

## Validação do Stock

Antes da operação, o sistema verifica se existe quantidade suficiente para todos os produtos.

Exemplo:

```text
Stock disponível:      2
Quantidade solicitada: 3
```

Resultado:

```text
Venda rejeitada
```

O stock nunca deve tornar-se negativo através de uma venda.

---

## Produtos Repetidos na Venda

Caso o mesmo produto apareça em mais de um `SaleItem`, as quantidades são agrupadas antes da redução do stock.

Exemplo:

```text
Produto Lente X

Item 1 → 1 unidade
Item 2 → 1 unidade
```

Impacto:

```text
Sale -2
```

Essa situação é importante para lentes em que cada unidade pode possuir um `ItemLens` diferente.

---

## Fluxo

1. Os produtos da venda são validados.
    
2. As quantidades são agrupadas por produto.
    
3. O sistema confirma se existe stock suficiente.
    
4. A venda é criada.
    
5. O stock de cada produto é reduzido.
    
6. É criada uma movimentação do tipo `Sale` para cada produto afetado.
    
7. Todas as alterações são persistidas na mesma transação.
    

---

## Exemplo

```text
Stock atual: 10
Venda:        3
```

Depois:

```text
StockQuantity = 7
```

Movimentação:

```text
Sale -3
```

---

# 7. Ajuste Manual de Stock

O sistema permite alterar manualmente o stock quando existe uma diferença entre a quantidade registada e a quantidade física.

Exemplos:

- erro de contagem;
    
- produto danificado;
    
- produto perdido;
    
- correção de inventário;
    
- correção de cadastro.
    

---

## Tipos de Ajuste

O ajuste pode ser:

```text
Increase
```

ou:

```text
Decrease
```

A quantidade informada pelo utilizador é sempre positiva.

O sistema determina o sinal da movimentação.

---

## Aumento

Exemplo:

```text
Type = Increase
Quantity = 3
```

é transformado em:

```text
QuantityChange = +3
```

---

## Redução

Exemplo:

```text
Type = Decrease
Quantity = 2
```

é transformado em:

```text
QuantityChange = -2
```

---

# 8. Regras do Ajuste Manual

Para realizar um ajuste:

- o produto deve pertencer à ótica;
    
- deve estar disponível para alteração de stock;
    
- o tipo deve ser válido;
    
- a quantidade deve ser superior a zero;
    
- deve ser informada uma justificativa.
    

---

## Justificativa

Todo ajuste manual exige:

```text
Reason
```

A justificativa não pode estar vazia e possui limite máximo de 500 caracteres.

Exemplo:

```text
"Duas armações danificadas durante armazenamento"
```

---

## Redução Superior ao Stock

Quando o ajuste representa uma redução, a quantidade não pode ultrapassar o stock disponível.

Exemplo:

```text
Stock atual: 5
Redução:     7
```

Resultado:

```text
Operação rejeitada
```

---

# 9. Fluxo do Ajuste Manual

1. O utilizador seleciona o produto.
    
2. Escolhe o tipo:
    
    - `Increase`;
        
    - `Decrease`.
        
3. Informa uma quantidade positiva.
    
4. Informa a justificativa.
    
5. O sistema valida os dados.
    
6. O produto é localizado dentro da ótica.
    
7. O sistema transforma a quantidade numa variação positiva ou negativa.
    
8. Caso seja uma redução, é verificado se existe stock suficiente.
    
9. `Product.StockQuantity` é atualizado.
    
10. É criada uma movimentação:
    

```text
Type = ManualAdjustment
```

11. A justificação é armazenada em `Reason`.
    
12. A atualização do stock e a movimentação são persistidas na mesma transação.
    

---

## Exemplo de Aumento

```text
Stock atual: 10

Ajuste:
Increase
Quantity = 2
Reason = "Produtos encontrados durante inventário"
```

Resultado:

```text
Stock final: 12

ManualAdjustment +2
```

---

## Exemplo de Redução

```text
Stock atual: 10

Ajuste:
Decrease
Quantity = 2
Reason = "Duas armações danificadas"
```

Resultado:

```text
Stock final: 8

ManualAdjustment -2
```

---

# 10. Cancelamento de Venda

O cancelamento de uma venda pode provocar a reposição de produtos no stock.

A regra depende do estado em que a venda se encontra.

---

## Cancelamento em Confirmed

Se a venda ainda estiver em:

```text
Confirmed
```

todos os seus itens retornam ao stock.

Exemplo:

```text
Venda:
- 1 armação
- 2 lentes
```

Cancelamento:

```text
Armação +1
Lentes  +2
```

São criadas movimentações:

```text
SaleCancellation
```

---

## Cancelamento em InProduction ou Ready

Caso a venda já esteja em:

```text
InProduction
```

ou:

```text
Ready
```

a reposição é diferente.

Produtos comuns retornam ao stock.

Itens que possuem `ItemLens` não retornam.

---

## Exemplo

```text
Venda:
- 1 armação
- 1 estojo
- 2 lentes personalizadas
```

Cancelamento em `InProduction`:

```text
Armação → +1
Estojo  → +1
Lentes  → não retornam
```

As movimentações geradas são:

```text
SaleCancellation +1
SaleCancellation +1
```

para os produtos restaurados.

---

## Por que a Lente não Gera Nova Saída

A lente já provocou a saída do stock quando a venda foi registada:

```text
Sale -1
```

Caso já tenha entrado em produção, ela simplesmente não retorna ao stock.

Não deve ser criada outra movimentação negativa, pois isso reduziria o stock pela segunda vez.

---

# 11. Agrupamento no Cancelamento

Caso vários itens restaurados utilizem o mesmo produto, as quantidades também são agrupadas por `ProductId`.

Exemplo:

```text
Produto A
Item 1 → 2
Item 2 → 3
```

Reposição:

```text
SaleCancellation +5
```

---

# 12. Consulta do Histórico de Stock

O sistema permite consultar as movimentações associadas a um determinado produto.

---

## Pré-condições

Para consultar o histórico:

- o produto deve existir;
    
- deve pertencer à ótica.
    

A consulta não exige que o produto esteja ativo.

Isso permite consultar o histórico de produtos que foram posteriormente desativados.

---

## Paginação

A consulta é paginada.

Os parâmetros utilizados são:

```text
Page
PageSize
```

Os valores padrão são:

```text
Page = 1
PageSize = 20
```

`Page` deve ser maior que zero.

`PageSize` deve estar entre:

```text
1 e 100
```

---

## Ordenação

As movimentações são apresentadas da mais recente para a mais antiga.

O primeiro critério é:

```text
CreatedAt DESC
```

e, quando necessário:

```text
Id DESC
```

é utilizado como segundo critério.

---

## Informações Apresentadas

Cada movimentação disponibiliza informações como:

- identificador;
    
- quantidade alterada;
    
- tipo;
    
- justificativa, quando existente;
    
- data;
    
- identificador do utilizador;
    
- nome do utilizador responsável.
    

---

## Exemplo

```text
Produto: Armação XP10

+10  InitialStock
 +5  Purchase
 -2  Sale
 -1  ManualAdjustment
 +1  SaleCancellation
```

Esse histórico permite compreender como a quantidade atual foi alcançada.

---

# 13. Utilizador Responsável

Toda `StockMovement` mantém:

```text
UserId
```

representando o utilizador responsável pela operação que originou a alteração.

Exemplos:

```text
InitialStock
→ utilizador que cadastrou o produto
```

```text
Purchase
→ utilizador que registou a compra
```

```text
Sale
→ utilizador que registou a venda
```

```text
ManualAdjustment
→ utilizador que realizou o ajuste
```

```text
SaleCancellation
→ utilizador que cancelou a venda
```

---

# 14. Movimentações não são Criadas Diretamente

Não existe um processo genérico no qual o utilizador informa livremente:

```text
Type
QuantityChange
```

para criar uma `StockMovement`.

As movimentações surgem sempre como consequência de um fluxo de negócio.

Assim:

```text
RegisterProduct
→ InitialStock
```

```text
RegisterPurchase
→ Purchase
```

```text
RegisterSale
→ Sale
```

```text
AdjustProductStock
→ ManualAdjustment
```

```text
CancelSale
→ SaleCancellation
```

Essa abordagem evita a criação de históricos que não correspondam a uma operação real.

---

# 15. Movimentações são Históricas

Uma movimentação existente representa um evento que já ocorreu.

Ela não deve ser editada para corrigir o stock atual.

Caso seja necessária uma correção, deve ser realizada uma nova operação.

Exemplo:

```text
Movimentação original:
Purchase +10

Quantidade correta deveria ter sido:
+8
```

A correção pode ser feita através de:

```text
ManualAdjustment -2
```

O histórico passa a mostrar:

```text
Purchase          +10
ManualAdjustment   -2
```

e o resultado final permanece correto sem eliminar o registo anterior.

---

# 16. Isolamento entre Óticas

O `StockMovement` não possui `OpticalStoreId` diretamente.

O contexto da ótica é determinado através do produto:

```text
StockMovement
      ↓
   Product
      ↓
OpticalStore
```

As operações de stock utilizam simultaneamente:

```text
ProductId
+
OpticalStoreId
```

para impedir alterações em produtos pertencentes a outra unidade.

---

# 17. Consistência das Operações

A atualização do stock e a criação da movimentação correspondente devem ocorrer de forma atómica.

---

## Stock Inicial

```text
Product
+
InitialStock
```

---

## Compra

```text
Purchase
+
PurchaseItems
+
Product.StockQuantity
+
StockMovement.Purchase
```

---

## Venda

```text
Sale
+
Product.StockQuantity
+
StockMovement.Sale
```

---

## Ajuste Manual

```text
Product.StockQuantity
+
StockMovement.ManualAdjustment
```

---

## Cancelamento

```text
Sale.Status
+
Payments
+
Product.StockQuantity
+
StockMovement.SaleCancellation
```

Caso uma etapa crítica falhe, a operação deverá ser revertida.

Isso evita situações como:

```text
Stock atualizado
Movimentação ausente
```

ou:

```text
Movimentação criada
Stock não atualizado
```

---

# 18. Fluxos Alternativos

## Produto não Encontrado

Caso o produto:

- não exista;
    
- ou pertença a outra ótica;
    

a operação não deverá alterar o stock.

---

## Stock Insuficiente em Venda

Caso:

```text
Quantidade solicitada > StockQuantity
```

a venda não deverá ser registada.

---

## Stock Insuficiente em Ajuste

Caso um ajuste `Decrease` tente remover uma quantidade superior ao stock disponível, a operação deverá ser rejeitada.

---

## Quantidade de Ajuste Inválida

Caso:

```text
Quantity <= 0
```

o ajuste deverá ser rejeitado.

A direção da movimentação é determinada por `Increase` ou `Decrease`, e não por uma quantidade negativa enviada pelo utilizador.

---

## Justificativa Ausente

Um ajuste manual sem `Reason` deverá ser rejeitado.

---

## Produto Inativo

Produtos inativos permanecem com o seu stock e histórico preservados.

Eles não podem participar de novas compras ou vendas.

A alteração manual do stock também não deve ser realizada enquanto o produto estiver inativo.

O produto poderá ser reativado caso volte a ser utilizado pela ótica.

---

# 19. Funcionalidades Não Implementadas

O processo atual não possui funcionalidades específicas para:

- reserva de stock;
    
- controlo de stock reservado e disponível;
    
- stock mínimo;
    
- alertas automáticos de stock reduzido;
    
- inventários formais;
    
- transferência de stock entre óticas;
    
- localização física dos produtos;
    
- lotes;
    
- números de série;
    
- identificação direta da compra ou venda na `StockMovement`;
    
- categorias estruturadas para motivos de ajuste;
    
- edição ou eliminação de movimentações históricas.
    

Essas funcionalidades poderão ser incorporadas futuramente caso se tornem necessárias.

---

# Resultado

O processo de Gestão de Stock centraliza as regras responsáveis pela quantidade disponível dos produtos e pelo histórico das suas alterações.

As principais origens das movimentações são:

```text
Produto registado
      ↓
InitialStock

Compra
      ↓
Purchase

Venda
      ↓
Sale

Ajuste manual
      ↓
ManualAdjustment

Cancelamento de venda
      ↓
SaleCancellation
```

Enquanto:

```text
Product.StockQuantity
```

representa a quantidade disponível atualmente,

```text
StockMovement
```

preserva como essa quantidade foi alcançada.

Essa separação permite consultas rápidas do stock atual sem perder a rastreabilidade das entradas, saídas, ajustes e reposições realizadas ao longo do tempo.