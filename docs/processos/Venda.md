# Processo de Venda

## Objetivo

Descrever o processo de registo e acompanhamento de uma venda realizada pela ótica, incluindo produtos, lentes personalizadas, tratamentos, pagamentos, controlo de stock, produção, entrega e cancelamento.

---

## Participantes

- Cliente;
    
- Utilizador da ótica;
    
- Sistema.
    

---

## Pré-condições

Para iniciar uma venda:

- o utilizador deve estar autenticado;
    
- a operação deve ocorrer dentro do contexto de uma ótica à qual o utilizador possui acesso;
    
- o cliente deve existir, estar ativo e pertencer à mesma ótica;
    
- os produtos selecionados devem existir, estar ativos e pertencer à mesma ótica;
    
- deve existir stock suficiente para todos os produtos selecionados.
    

---

# 1. Registo da Venda

## Fluxo Principal

1. O utilizador inicia uma nova venda.
    
2. O cliente é selecionado.
    
3. O sistema valida se o cliente está ativo e pertence à ótica.
    
4. São adicionados um ou mais produtos à venda.
    
5. O sistema valida a existência, o estado e o stock disponível de cada produto.
    
6. Para cada item são informados:
    
    - quantidade;
        
    - desconto, quando aplicável;
        
    - observações, quando necessário.
        
7. O sistema utiliza o preço base atual do produto como preço unitário da venda.
    
8. Caso existam produtos do tipo `Lens`, uma receita válida deve ser selecionada.
    
9. Para cada lente são registadas as informações técnicas necessárias à fabricação.
    
10. Quando aplicável, são selecionados os tratamentos da lente.
    
11. O sistema calcula o valor total de cada item.
    
12. O sistema calcula o valor total da venda.
    
13. É registado um pagamento inicial.
    
14. Caso exista saldo restante, o sistema cria um pagamento pendente.
    
15. O sistema cria a venda com estado `Confirmed`.
    
16. O stock dos produtos vendidos é reduzido.
    
17. São criadas as respetivas movimentações de stock do tipo `Sale`.
    
18. A operação é concluída.
    

---

## Resultado do Registo

Ao final do processo:

- a venda encontra-se registada;
    
- o estado inicial é `Confirmed`;
    
- os preços utilizados ficam preservados nos itens;
    
- o stock foi atualizado;
    
- as movimentações de stock foram registadas;
    
- existe pelo menos um pagamento recebido;
    
- caso exista saldo restante, há um pagamento com estado `Pending`.
    

---

# 2. Venda com Lentes

## Regras Específicas

Quando a venda contém um produto do tipo `Lens`:

- uma receita válida é obrigatória;
    
- cada lente deve possuir um `ItemLens`;
    
- cada `ItemLens` representa uma única lente;
    
- o respetivo `SaleItem` deve possuir quantidade igual a `1`.
    

A receita deve:

- pertencer ao cliente da venda;
    
- pertencer ao contexto da mesma ótica;
    
- estar dentro do período de validade.
    

---

## Informações da Lente

Para cada lente são registadas informações como:

- olho correspondente;
    
- DP;
    
- DNP;
    
- tipo da lente;
    
- índice de refração;
    
- material;
    
- cor;
    
- diâmetro.
    

DP, DNP e cor podem ser opcionais conforme o caso.

---

## Tratamentos

Uma lente pode possuir zero ou vários tratamentos.

Os tratamentos selecionados devem:

- existir;
    
- estar ativos;
    
- pertencer à mesma ótica.
    

O preço do tratamento utilizado no momento da venda é preservado em `ItemLensTreatment`.

---

# 3. Cálculo da Venda

## Item sem Tratamentos

O valor de um item é calculado através de:

```text
(UnitPrice × Quantity)
- DiscountAmount
=
SaleItem.TotalAmount
```

---

## Item com Tratamentos

No caso de uma lente:

```text
(UnitPrice × Quantity)
- DiscountAmount
+ Tratamentos
=
SaleItem.TotalAmount
```

O desconto do item é aplicado ao valor do produto e não aos tratamentos.

---

## Total da Venda

O valor total da venda corresponde à soma dos valores de todos os itens:

```text
Sale.TotalAmount
=
Soma dos SaleItem.TotalAmount
```

---

# 4. Pagamento Inicial

Toda venda exige um pagamento inicial.

O pagamento inicial deve:

- possuir valor superior a zero;
    
- possuir uma forma de pagamento;
    
- não ultrapassar o valor total da venda.
    

Esse pagamento é criado com:

```text
Status = Received
```

---

## Venda Totalmente Paga

Quando o pagamento inicial corresponde ao valor total:

```text
Total da venda:       300 €
Pagamento inicial:    300 €

Saldo restante:         0 €
```

É criado apenas um pagamento.

---

## Venda Parcialmente Paga

Quando o pagamento inicial é inferior ao total:

```text
Total da venda:       300 €
Pagamento inicial:    100 €
Saldo restante:       200 €
```

O sistema cria:

```text
Payment 1
100 €
Received
```

e:

```text
Payment 2
200 €
Pending
```

---

# 5. Venda sem Lentes

Quando a venda não possui nenhum `ItemLens`, não existe processo de produção.

O fluxo é:

```text
Confirmed
    ↓
Delivered
```

Para que a venda seja entregue:

- deve permanecer em `Confirmed`;
    
- o valor total deve estar pago.
    

Caso exista pagamento pendente, ele deverá ser recebido antes da entrega.

---

# 6. Venda com Lentes

Quando existe pelo menos um `ItemLens`, a venda deve passar pelo processo de produção.

O fluxo é:

```text
Confirmed
    ↓
InProduction
    ↓
Ready
    ↓
Delivered
```

---

## Iniciar Produção

Uma venda pode passar para `InProduction` quando:

- está em `Confirmed`;
    
- possui pelo menos um `ItemLens`.
    

Vendas sem lentes não podem entrar em produção.

---

## Marcar como Pronta

Quando a produção das lentes estiver concluída:

```text
InProduction
    ↓
Ready
```

A operação somente pode ser realizada se a venda estiver em `InProduction`.

---

## Entrega

Uma venda com lentes pode ser entregue quando:

- está em `Ready`;
    
- o valor total da venda foi recebido.
    

Então:

```text
Ready
    ↓
Delivered
```

---

# 7. Recebimento do Pagamento Restante

Caso exista um pagamento com estado `Pending`, ele pode ser recebido posteriormente.

O utilizador informa a forma de pagamento utilizada.

O sistema então:

1. verifica se o pagamento está pendente;
    
2. verifica se a venda permite o recebimento;
    
3. regista a forma de pagamento;
    
4. regista a data do recebimento;
    
5. identifica o utilizador responsável;
    
6. altera o estado para `Received`.
    

O valor do pagamento pendente é recebido integralmente.

O fluxo atual não permite dividir posteriormente esse saldo em múltiplos pagamentos.

---

# 8. Cancelamento da Venda

Uma venda pode ser cancelada quando estiver em:

- `Confirmed`;
    
- `InProduction`;
    
- `Ready`.
    

Não é permitido cancelar vendas:

- `Delivered`;
    
- `Cancelled`.
    

---

## Cancelamento em Confirmed

Quando a venda ainda está em `Confirmed`, a produção das lentes ainda não foi iniciada.

Por esse motivo, todos os produtos da venda retornam ao stock.

Para cada produto restaurado é criada uma movimentação:

```text
SaleCancellation
```

---

## Cancelamento em InProduction ou Ready

Quando a venda já entrou em produção:

- produtos comuns retornam ao stock;
    
- produtos associados a `ItemLens` não retornam ao stock.
    

Isso ocorre porque as lentes já representam produtos personalizados cuja produção foi iniciada.

Exemplo:

```text
Venda:
- 1 armação
- 2 lentes personalizadas
```

Cancelamento em `InProduction`:

```text
Armação
→ retorna ao stock

Lentes
→ não retornam ao stock
```

---

## Pagamentos no Cancelamento

Ao cancelar a venda, os pagamentos associados passam para:

```text
Cancelled
```

Isso representa o cancelamento dos registos financeiros dentro do sistema.

A versão atual não possui um processo específico de reembolso ou estorno do valor já recebido.

---

# 9. Fluxos Alternativos

## Cliente inválido

Se o cliente:

- não existir;
    
- estiver inativo;
    
- pertencer a outra ótica;
    

a venda não poderá ser registada.

---

## Produto inválido

Se algum produto:

- não existir;
    
- estiver inativo;
    
- pertencer a outra ótica;
    

a venda não poderá ser registada.

---

## Stock insuficiente

Caso a quantidade solicitada seja superior ao stock disponível:

```text
Stock disponível: 2
Quantidade solicitada: 3
```

a venda deverá ser rejeitada.

---

## Lente sem Receita

Caso exista um produto do tipo `Lens` e nenhuma receita seja informada, a venda não poderá ser registada.

---

## Receita inválida

A venda deverá ser rejeitada caso a receita:

- não exista;
    
- pertença a outro cliente;
    
- pertença a outra ótica;
    
- esteja expirada.
    

---

## Lente sem ItemLens

Um produto do tipo `Lens` sem as informações correspondentes de `ItemLens` deverá impedir o registo da venda.

---

## ItemLens em Produto Comum

Produtos que não sejam do tipo `Lens` não poderão possuir `ItemLens`.

---

## Tratamento inválido

Um tratamento não poderá ser utilizado caso:

- não exista;
    
- esteja inativo;
    
- pertença a outra ótica.
    

---

## Pagamento Inicial Inválido

A venda deverá ser rejeitada caso o pagamento inicial:

- seja igual ou inferior a zero;
    
- ultrapasse o valor total da venda;
    
- não possua uma forma de pagamento válida.
    

---

## Entrega com Saldo Pendente

Uma venda não poderá ser entregue enquanto:

```text
ReceivedAmount < Sale.TotalAmount
```

---

# 10. Consistência da Operação

O registo da venda envolve várias alterações relacionadas:

```text
Venda
+
Itens
+
Lentes
+
Tratamentos
+
Pagamentos
+
Stock
+
Movimentações de estoque
```

Essas alterações devem fazer parte da mesma operação transacional.

Caso alguma etapa crítica falhe, nenhuma alteração parcial deverá permanecer persistida.

---

# Resultado

O processo de venda permite representar desde uma venda simples de produtos até uma venda de lentes personalizadas com produção, tratamentos e pagamento posterior.

Ao final do processo, a venda poderá assumir um dos seguintes estados:

```text
Confirmed
InProduction
Ready
Delivered
Cancelled
```

O fluxo utilizado depende da existência de lentes personalizadas e do estado financeiro da venda.