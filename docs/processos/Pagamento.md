## Objetivo

Descrever o processo de registo e acompanhamento dos pagamentos associados às vendas realizadas pela ótica.

O processo atual permite:

- registar um pagamento inicial durante a criação da venda;
    
- representar o saldo restante através de um pagamento pendente;
    
- receber posteriormente esse pagamento;
    
- identificar a forma de pagamento utilizada;
    
- identificar quando e por quem o valor foi recebido;
    
- determinar se uma venda está totalmente paga;
    
- cancelar os registos de pagamento quando a venda é cancelada.
    

O sistema atual não implementa parcelamento livre nem um processo financeiro de reembolso.

---

## Participantes

- Cliente;
    
- Utilizador da ótica;
    
- Sistema.
    

---

# 1. Relação entre Venda e Pagamento

Todo `Payment` pertence a uma única `Sale`.

A relação é:

```text
Sale
 │
 │ 1:N
 ↓
Payment
```

Uma venda pode possuir mais de um pagamento.

Entretanto, no fluxo atual, são criados no máximo:

```text
Pagamento inicial
+
Saldo restante
```

---

# 2. Estrutura do Pagamento

Cada pagamento mantém informações como:

- valor;
    
- estado;
    
- forma de pagamento, quando aplicável;
    
- data de recebimento, quando aplicável;
    
- venda à qual pertence;
    
- utilizador responsável pelo recebimento, quando aplicável.
    

---

## Valor

O valor é armazenado em:

```text
Amount
```

e representa o valor financeiro daquele registo.

---

## Estado

Os estados atualmente existentes são:

```text
Pending
Received
Cancelled
```

---

## Forma de Pagamento

A forma utilizada é representada por:

```text
PaymentMethod
```

As formas atualmente suportadas incluem:

- dinheiro;
    
- cartão de débito;
    
- cartão de crédito;
    
- MB WAY;
    
- transferência bancária.
    

`PaymentMethod` é opcional porque um pagamento ainda pendente não possui necessariamente uma forma de pagamento definida.

---

## Dados do Recebimento

Quando o pagamento é efetivamente recebido, são registados:

```text
ReceivedAt
ReceivedByUserId
```

Essas propriedades permanecem nulas enquanto o pagamento estiver pendente.

---

# 3. Estados do Pagamento

## Pending

Representa um valor que ainda deverá ser recebido.

Enquanto estiver pendente:

```text
Status = Pending
PaymentMethod = null
ReceivedAt = null
ReceivedByUserId = null
```

---

## Received

Representa um valor que foi registado como recebido pela ótica.

Nesse caso:

```text
Status = Received
PaymentMethod = forma utilizada
ReceivedAt = momento do recebimento
ReceivedByUserId = utilizador responsável
```

---

## Cancelled

Representa um pagamento associado a uma venda que foi cancelada.

O estado:

```text
Cancelled
```

não significa, por si só, que ocorreu um reembolso financeiro ao cliente.

---

# 4. Pagamento Inicial

Toda nova venda deve possuir um pagamento inicial.

O pagamento é informado juntamente com os restantes dados necessários para o registo da venda.

---

## Dados Informados

São informados:

- valor;
    
- forma de pagamento.
    

---

## Validação do Valor

O pagamento inicial deve possuir:

```text
Amount > 0
```

Além disso:

```text
Amount <= Sale.TotalAmount
```

Não é permitido informar um pagamento inicial superior ao valor total da venda.

---

## Validação da Forma de Pagamento

O pagamento inicial representa um valor já recebido.

Por isso, deve possuir uma forma de pagamento válida.

---

# 5. Criação do Pagamento Inicial

Depois de o valor total da venda ser calculado, o sistema cria o pagamento inicial.

Ele é criado diretamente com:

```text
Status = Received
```

e recebe:

```text
Amount = valor informado
PaymentMethod = forma informada
ReceivedAt = momento da venda
ReceivedByUserId = utilizador que registou a venda
```

---

## Exemplo

```text
Venda:
TotalAmount = 300 €

Pagamento inicial:
Amount = 100 €
PaymentMethod = DebitCard
```

Resultado:

```text
Payment

Amount = 100 €
Status = Received
PaymentMethod = DebitCard
ReceivedAt = data/hora da operação
ReceivedByUserId = utilizador da venda
```

---

# 6. Venda Totalmente Paga no Registo

Caso o pagamento inicial seja igual ao valor total:

```text
Sale.TotalAmount = 300 €
InitialPayment   = 300 €
```

o saldo restante será:

```text
0 €
```

Nesse caso, apenas o pagamento inicial é criado.

Exemplo:

```text
Sale
 └── Payment
      Amount = 300 €
      Status = Received
```

Não existe pagamento `Pending`.

---

# 7. Criação do Saldo Pendente

Quando o pagamento inicial não cobre o valor total da venda, o sistema calcula:

```text
RemainingAmount
=
Sale.TotalAmount
-
InitialPayment.Amount
```

Esse valor gera automaticamente um segundo `Payment`.

---

## Estado Inicial

O pagamento restante é criado com:

```text
Status = Pending
```

e:

```text
PaymentMethod = null
ReceivedAt = null
ReceivedByUserId = null
```

---

## Exemplo

```text
Venda:
Total = 300 €

Pagamento inicial:
100 €
```

O sistema cria:

```text
Payment 1
Amount = 100 €
Status = Received
```

e:

```text
Payment 2
Amount = 200 €
Status = Pending
```

---

# 8. O Saldo Pendente é Único

A implementação atual não divide automaticamente o valor restante em várias parcelas.

Assim:

```text
Venda = 500 €
Entrada = 100 €
```

gera:

```text
Received = 100 €
Pending  = 400 €
```

e não:

```text
100 € Received
100 € Pending
100 € Pending
100 € Pending
100 € Pending
```

O modelo atual representa:

```text
Entrada
+
Saldo final
```

---

# 9. Recebimento do Pagamento Pendente

Um pagamento pendente pode ser recebido posteriormente.

Nesse processo, o utilizador não informa novamente o valor.

Ele informa apenas a forma de pagamento utilizada.

---

## Pré-condições

Para que o pagamento possa ser recebido:

- o pagamento deve existir;
    
- deve pertencer à venda informada;
    
- a venda deve pertencer à ótica;
    
- o pagamento deve estar em `Pending`;
    
- a venda deve estar em um estado que permita recebimentos.
    

---

## Estados Permitidos da Venda

O recebimento é permitido quando a venda estiver em:

```text
Confirmed
InProduction
Ready
```

Não é permitido através desse fluxo quando a venda estiver:

```text
Delivered
Cancelled
```

---

# 10. Fluxo de Recebimento

1. O utilizador seleciona o pagamento pendente.
    
2. Informa a forma de pagamento utilizada pelo cliente.
    
3. O sistema valida a forma de pagamento.
    
4. O sistema verifica simultaneamente:
    
    - identificador do pagamento;
        
    - identificador da venda;
        
    - ótica;
        
    - estado `Pending`;
        
    - estado atual da venda.
        
5. O sistema altera:
    

```text
Pending
```

para:

```text
Received
```

6. A forma de pagamento é armazenada.
    
7. É registado o momento do recebimento.
    
8. É registado o utilizador responsável.
    

---

## Resultado

Antes:

```text
Payment

Amount = 200 €
Status = Pending
PaymentMethod = null
ReceivedAt = null
ReceivedByUserId = null
```

Depois:

```text
Payment

Amount = 200 €
Status = Received
PaymentMethod = MBWay
ReceivedAt = momento atual
ReceivedByUserId = utilizador responsável
```

---

# 11. O Valor do Pagamento não é Alterado

Durante o recebimento, `Amount` permanece o mesmo.

Exemplo:

```text
Pending:
Amount = 200 €
```

Após recebimento:

```text
Received:
Amount = 200 €
```

O fluxo não recebe um novo valor financeiro.

Por esse motivo, um pagamento pendente é liquidado integralmente.

---

# 12. Recebimentos Parciais Posteriores

A implementação atual não permite transformar:

```text
Pending = 200 €
```

em:

```text
Received = 100 €
Pending  = 100 €
```

Caso exista um saldo pendente de 200 €, ele deverá ser recebido integralmente.

Um sistema de múltiplos recebimentos posteriores poderá ser implementado futuramente caso se torne necessário.

---

# 13. Cálculo do Valor Recebido

A venda não possui uma propriedade própria para armazenar o total recebido.

Esse valor é calculado através dos pagamentos.

São considerados apenas pagamentos com:

```text
Status = Received
```

Assim:

```text
ReceivedAmount
=
Soma de Payment.Amount
onde Status = Received
```

---

## Exemplo

```text
Payment 1
100 €
Received

Payment 2
200 €
Received
```

Resultado:

```text
ReceivedAmount = 300 €
```

---

# 14. Cálculo do Saldo Restante

Enquanto a venda não estiver cancelada:

```text
RemainingAmount
=
Sale.TotalAmount
-
ReceivedAmount
```

Exemplo:

```text
Sale.TotalAmount = 300 €
ReceivedAmount   = 100 €
```

Resultado:

```text
RemainingAmount = 200 €
```

---

## Venda Cancelada

Quando a venda está em:

```text
Cancelled
```

o sistema apresenta:

```text
RemainingAmount = 0
```

porque a operação comercial deixou de possuir um saldo a ser cobrado dentro do fluxo normal.

---

# 15. Relação com a Entrega da Venda

Uma venda só pode passar para:

```text
Delivered
```

quando estiver totalmente paga.

A condição financeira é:

```text
ReceivedAmount = Sale.TotalAmount
```

---

## Venda sem Lentes

O fluxo é:

```text
Confirmed
    ↓
Pagamento completo
    ↓
Delivered
```

---

## Venda com Lentes

O fluxo é:

```text
Confirmed
    ↓
InProduction
    ↓
Ready
    ↓
Pagamento completo
    ↓
Delivered
```

Mesmo que a produção esteja concluída, uma venda em `Ready` não poderá ser entregue enquanto existir saldo pendente.

---

# 16. Pagamento e Produção são Independentes

O pagamento restante pode ser recebido antes da conclusão da produção.

Por exemplo:

```text
Sale = InProduction
```

pode possuir:

```text
Payment Pending
```

e o cliente pode liquidá-lo nesse momento.

Depois:

```text
Sale = InProduction
Payments = totalmente recebidos
```

A produção continua normalmente até:

```text
Ready
```

e somente então a venda poderá ser entregue.

Assim, pagamento e produção são processos relacionados à mesma venda, mas não precisam ocorrer exatamente ao mesmo tempo.

---

# 17. Cancelamento da Venda

Quando uma venda é cancelada, os pagamentos associados que ainda não estão em `Cancelled` são atualizados.

Assim:

```text
Pending
→ Cancelled
```

e:

```text
Received
→ Cancelled
```

---

## Exemplo

Antes:

```text
Payment 1
100 €
Received

Payment 2
200 €
Pending
```

Após cancelamento da venda:

```text
Payment 1
100 €
Cancelled

Payment 2
200 €
Cancelled
```

---

# 18. Informações de Recebimento após Cancelamento

Quando um pagamento que anteriormente estava `Received` passa para `Cancelled`, as informações relacionadas ao recebimento já realizado podem permanecer registadas.

Por exemplo:

```text
Amount
PaymentMethod
ReceivedAt
ReceivedByUserId
```

Isso permite preservar informações sobre o que ocorreu antes do cancelamento.

Entretanto, o pagamento deixa de ser contabilizado como `Received` para o cálculo financeiro atual da venda.

---

# 19. Cancelamento não Representa Reembolso

A mudança:

```text
Received
→ Cancelled
```

representa o cancelamento do registo financeiro associado à venda dentro do sistema.

Ela não significa que:

```text
dinheiro foi automaticamente devolvido ao cliente
```

A versão atual não possui um processo específico de:

- reembolso;
    
- estorno;
    
- devolução em dinheiro;
    
- devolução por cartão;
    
- transferência de devolução.
    

Essas operações deverão ser modeladas separadamente caso sejam implementadas futuramente.

---

# 20. Consulta dos Pagamentos

Os pagamentos são disponibilizados no contexto da consulta da venda.

A venda apresenta os pagamentos associados e informações como:

- identificador;
    
- valor;
    
- estado;
    
- forma de pagamento;
    
- data de recebimento;
    
- utilizador responsável pelo recebimento.
    

Além disso, a consulta da venda apresenta valores calculados como:

```text
ReceivedAmount
RemainingAmount
```

Não existe atualmente necessidade de uma gestão financeira independente da venda para consultar pagamentos globalmente.

---

# 21. Identificação do Utilizador Responsável

`ReceivedByUserId` identifica especificamente o utilizador que recebeu determinado pagamento.

---

## Pagamento Inicial

No pagamento inicial:

```text
ReceivedByUserId
```

corresponde ao utilizador que registou a venda.

---

## Pagamento Posterior

Quando o pagamento pendente é recebido posteriormente:

```text
ReceivedByUserId
```

corresponde ao utilizador que executou essa operação.

---

## Pagamento Pendente

Enquanto estiver em `Pending`:

```text
ReceivedByUserId = null
```

porque ainda não ocorreu um recebimento.

---

# 22. Isolamento entre Óticas

`Payment` não possui `OpticalStoreId` diretamente.

O contexto da ótica é determinado através da venda:

```text
Payment
   ↓
Sale
   ↓
OpticalStore
```

Ao receber um pagamento, o sistema verifica simultaneamente:

```text
PaymentId
+
SaleId
+
OpticalStoreId
```

Isso impede que um pagamento pertencente a outra venda ou outra ótica seja recebido através do contexto incorreto.

---

# 23. Concorrência no Recebimento

O recebimento verifica diretamente se o pagamento ainda possui:

```text
Status = Pending
```

no momento da atualização.

Isso evita que duas operações concorrentes recebam o mesmo pagamento duas vezes.

Exemplo:

```text
Operação A
        \
         → Payment Pending
        /
Operação B
```

A primeira atualização válida altera:

```text
Pending → Received
```

Quando a segunda tentar realizar a mesma operação, o pagamento já não estará em `Pending`.

Por isso, o segundo recebimento deverá ser rejeitado.

---

# 24. Fluxos Alternativos

## Pagamento Inicial com Valor Inválido

Caso:

```text
Amount <= 0
```

a venda não poderá ser registada.

---

## Pagamento Inicial Superior ao Total

Caso:

```text
InitialPayment.Amount > Sale.TotalAmount
```

a venda deverá ser rejeitada.

Exemplo:

```text
Venda = 200 €
Pagamento inicial = 250 €
```

Resultado:

```text
Operação rejeitada
```

---

## Forma de Pagamento Inválida

Caso seja informada uma forma de pagamento não suportada, a operação deverá ser rejeitada.

Essa regra aplica-se:

- ao pagamento inicial;
    
- ao recebimento posterior.
    

---

## Pagamento não Encontrado

Caso o pagamento:

- não exista;
    
- não pertença à venda;
    
- ou pertença a uma venda de outra ótica;
    

a operação deverá indicar que o pagamento não foi encontrado naquele contexto.

---

## Pagamento já Recebido

Um pagamento com:

```text
Status = Received
```

não poderá ser recebido novamente.

---

## Pagamento Cancelado

Um pagamento com:

```text
Status = Cancelled
```

não poderá ser recebido.

---

## Venda já Entregue

Um pagamento pendente não poderá ser recebido através do fluxo atual se a venda já estiver:

```text
Delivered
```

Na operação normal, essa situação não deverá ocorrer, pois a entrega exige pagamento completo.

---

## Venda Cancelada

Pagamentos pertencentes a uma venda `Cancelled` não podem ser recebidos posteriormente.

---

# 25. Consistência com o Processo de Venda

Durante o registo de uma nova venda, a criação dos pagamentos faz parte da mesma operação que cria:

```text
Sale
+
SaleItems
+
ItemLens
+
ItemLensTreatments
+
Payments
+
alterações de stock
+
StockMovements
```

Caso o registo da venda falhe, os pagamentos também não deverão permanecer persistidos isoladamente.

Isso evita situações como:

```text
Payment existente
Sale inexistente
```

---

# 26. Funcionalidades Não Implementadas

O processo atual não possui funcionalidades específicas para:

- dividir o saldo restante em vários pagamentos;
    
- receber apenas parte de um pagamento pendente;
    
- criar pagamentos adicionais arbitrariamente;
    
- editar o valor de um pagamento existente;
    
- eliminar pagamentos;
    
- realizar reembolsos;
    
- realizar estornos;
    
- registar valor reembolsado;
    
- registar forma de reembolso;
    
- emitir comprovantes;
    
- integrar diretamente com terminais de pagamento;
    
- integrar automaticamente com MB WAY;
    
- realizar conciliação bancária;
    
- gerir caixa;
    
- controlar pagamentos aos fornecedores.
    

Essas funcionalidades poderão ser incorporadas futuramente caso se tornem necessárias.

---

# Resultado

O processo de Gestão de Pagamentos pode ser resumido como:

```text
Venda criada
    ↓
Pagamento inicial
    ↓
Received
```

Caso o pagamento seja integral:

```text
Received = TotalAmount
        ↓
Venda totalmente paga
```

Caso exista saldo:

```text
Pagamento inicial
    ↓
Received

Saldo restante
    ↓
Pending
    ↓
Recebimento posterior
    ↓
Received
```

A venda somente pode ser entregue quando:

```text
ReceivedAmount = Sale.TotalAmount
```

Caso a venda seja cancelada:

```text
Payments
    ↓
Cancelled
```

sem que isso represente automaticamente um reembolso financeiro.

Essa modelagem atende ao fluxo atual de **pagamento inicial + saldo final**, mantendo as informações financeiras separadas da entidade `Sale` e preservando quem realizou cada recebimento e quando ele ocorreu.