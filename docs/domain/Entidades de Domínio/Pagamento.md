## 1. Contexto

A entidade `Pagamento` representa uma parcela financeira associada a uma venda.

Ela é utilizada tanto para registrar valores já recebidos pela ótica quanto valores que ainda permanecem pendentes.

O seu objetivo é permitir acompanhar a situação financeira da venda, identificando:

- o valor do pagamento;
    
- o seu estado;
    
- a forma de pagamento utilizada, quando recebido;
    
- quando o valor foi recebido;
    
- qual utilizador registou o recebimento.
    

Uma venda pode possuir mais de um pagamento, permitindo representar o cenário atual de pagamento integral ou pagamento inicial seguido de um valor restante.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos e a implementação do fluxo de venda foram definidas as seguintes regras do negócio.

#### Todo Pagamento pertence a uma Venda

Cada pagamento está associado a uma única venda através de `SaleId`.

O pagamento não possui `OpticalStoreId` diretamente.

O contexto da ótica é determinado através da própria venda:

```text
Pagamento
    ↓
Venda
    ↓
Ótica
```

---

#### Uma Venda pode possuir mais de um Pagamento

A modelagem permite que uma venda possua uma coleção de pagamentos.

Na implementação atual, o fluxo de registo da venda gera:

- um pagamento inicial recebido;
    
- opcionalmente, um pagamento correspondente ao saldo restante.
    

Assim:

```text
Venda totalmente paga no registo
→ 1 Pagamento Received
```

ou:

```text
Venda parcialmente paga
→ 1 Pagamento Received
→ 1 Pagamento Pending
```

O fluxo atual não implementa parcelamentos livres ou a divisão do saldo restante em vários pagamentos independentes.

---

#### Toda Venda exige um pagamento inicial

Durante o registo de uma venda deve ser informado um pagamento inicial.

O valor:

- deve ser superior a zero;
    
- não pode ultrapassar o valor total da venda.
    

Esse pagamento é imediatamente criado com:

```text
Status = Received
```

---

#### O pagamento inicial possui uma forma de pagamento

Como o pagamento inicial representa um valor já recebido, deve ser informada uma forma de pagamento válida.

As formas atualmente suportadas são:

- Dinheiro (`Cash`);
    
- Cartão de Débito (`DebitCard`);
    
- Cartão de Crédito (`CreditCard`);
    
- MB WAY (`MBWay`);
    
- Transferência Bancária (`BankTransfer`).
    

---

#### O saldo restante é representado por um Pagamento pendente

Caso o pagamento inicial seja inferior ao valor total da venda, o sistema cria automaticamente um segundo pagamento.

O seu valor corresponde a:

```text
Venda.TotalAmount
-
Pagamento inicial
```

Esse registo é criado com:

```text
Status = Pending
PaymentMethod = null
ReceivedAt = null
ReceivedByUserId = null
```

Isso representa um valor que ainda deverá ser recebido pela ótica.

---

#### Pagamentos pendentes ainda não possuem forma de pagamento

A forma de pagamento somente é definida quando o valor é efetivamente recebido.

Por esse motivo:

```text
PaymentMethod
```

é uma propriedade opcional na entidade.

Para pagamentos `Pending`, o valor permanece nulo.

Quando o pagamento é recebido, a forma escolhida pelo cliente é registada.

---

#### Um Pagamento pendente pode ser recebido posteriormente

O sistema possui um fluxo específico para receber um pagamento que ainda esteja em estado:

```text
Pending
```

Ao realizar o recebimento:

- o estado passa para `Received`;
    
- é registada a forma de pagamento;
    
- é registada a data do recebimento;
    
- é identificado o utilizador responsável.
    

O valor (`Amount`) não é alterado durante esse processo.

---

#### O pagamento pendente é recebido integralmente

Na implementação atual, o fluxo de recebimento não recebe um novo valor financeiro.

Ele recebe apenas a forma de pagamento.

Isso significa que o pagamento `Pending` existente é liquidado integralmente.

Exemplo:

```text
Venda:           300 €
Entrada:         100 €
Saldo pendente:  200 €
```

Posteriormente:

```text
Pagamento Pending de 200 €
        ↓
Recebimento
        ↓
Pagamento Received de 200 €
```

Não é possível atualmente receber apenas uma parte desses 200 € e manter o restante pendente.

---

#### Apenas determinadas Vendas podem receber um Pagamento pendente

Um pagamento pode passar de `Pending` para `Received` enquanto a venda estiver em:

- `Confirmed`;
    
- `InProduction`;
    
- `Ready`.
    

Pagamentos não podem ser recebidos através desse fluxo quando a venda já estiver:

- `Delivered`;
    
- `Cancelled`.
    

---

#### O Pagamento identifica quem recebeu o valor

Quando um pagamento é recebido, o sistema armazena:

```text
ReceivedByUserId
```

Essa referência identifica o utilizador responsável pelo recebimento.

No pagamento inicial, corresponde ao utilizador que registou a venda.

Em um pagamento pendente recebido posteriormente, corresponde ao utilizador que realizou essa operação.

---

#### O Pagamento registra quando o valor foi recebido

Pagamentos com estado `Received` possuem:

```text
ReceivedAt
```

indicando quando o recebimento ocorreu.

Pagamentos pendentes não possuem essa informação até que sejam efetivamente recebidos.

---

#### O Pagamento possui um estado

Os estados atualmente existentes são:

```text
Pending
Received
Cancelled
```

Eles representam:

```text
Pending
→ valor ainda por receber
```

```text
Received
→ valor registado como recebido
```

```text
Cancelled
→ pagamento cancelado em decorrência do cancelamento da venda
```

---

#### O valor recebido da Venda é calculado a partir dos Pagamentos

A entidade `Venda` não armazena diretamente um campo como:

```text
ReceivedAmount
```

O valor recebido é calculado somando os pagamentos cujo estado seja:

```text
Received
```

Assim:

```text
ReceivedAmount
=
Soma dos Payment.Amount
onde Status = Received
```

O saldo restante é obtido através de:

```text
RemainingAmount
=
Sale.TotalAmount - ReceivedAmount
```

Para uma venda cancelada, o sistema apresenta o saldo restante como zero.

---

#### A Venda só pode ser entregue quando estiver totalmente paga

Antes de uma venda passar para `Delivered`, o sistema compara:

```text
ReceivedAmount
```

com:

```text
Sale.TotalAmount
```

Caso ainda exista saldo pendente, a venda não pode ser entregue.

Essa regra é aplicada tanto às vendas comuns quanto às vendas com lentes personalizadas.

---

#### O cancelamento da Venda cancela os Pagamentos

Quando uma venda é cancelada, todos os pagamentos associados que ainda não estejam em `Cancelled` passam para esse estado.

Isso inclui tanto:

- pagamentos pendentes;
    
- pagamentos anteriormente recebidos.
    

Na implementação atual, essa alteração representa o cancelamento do registo financeiro dentro do sistema.

Ela **não representa, por si só, a realização de um reembolso financeiro ao cliente**.

O processo de devolução ou estorno do dinheiro não é atualmente modelado.

---

## 3. Decisões de Modelagem

#### O Pagamento foi separado da Venda

Embora toda venda possua um valor total, foi criada uma entidade específica para representar a sua situação financeira.

Essa separação permite distinguir:

```text
Venda
→ valor total da operação
```

de:

```text
Pagamento
→ valores recebidos ou ainda pendentes
```

Isso evita adicionar diretamente à venda diversas propriedades relacionadas ao processo financeiro.

---

#### Pagamento não representa apenas valores recebidos

A entidade também representa compromissos financeiros ainda pendentes.

Por esse motivo, `PaymentMethod`, `ReceivedAt` e `ReceivedByUserId` são opcionais.

Enquanto o pagamento estiver em `Pending`:

```text
PaymentMethod = null
ReceivedAt = null
ReceivedByUserId = null
```

Essas informações passam a existir quando o pagamento é efetivamente recebido.

---

#### O valor do Pagamento permanece fixo

`Amount` representa o valor associado àquele registo financeiro.

Quando um pagamento pendente é recebido, o seu valor não é alterado.

A operação apenas modifica:

- estado;
    
- forma de pagamento;
    
- data de recebimento;
    
- utilizador responsável.
    

---

#### As formas de pagamento são representadas por enum

As formas atualmente suportadas são representadas através de `PaymentMethod`:

```text
Cash
DebitCard
CreditCard
MBWay
BankTransfer
```

Essa abordagem atende às necessidades atuais sem exigir uma entidade específica para formas de pagamento.

---

#### O estado também é representado por enum

A situação do pagamento é representada por `PaymentStatus`:

```text
Pending
Received
Cancelled
```

Isso permite diferenciar valores que ainda devem ser recebidos daqueles que já foram pagos ou que deixaram de fazer parte de uma venda ativa.

---

#### O fluxo atual não implementa parcelamento livre

Embora uma venda possua uma coleção de pagamentos, a implementação atual não funciona como um sistema genérico de parcelas.

O cenário suportado é:

```text
Pagamento inicial
+
Saldo restante
```

O saldo restante é representado por um único pagamento `Pending`.

Caso seja recebido posteriormente, ele é liquidado integralmente.

Essa modelagem atende ao cenário atual sem introduzir um sistema completo de parcelamento.

---

#### Não existem Pagamentos associados às Compras

`Pagamento` está relacionado apenas ao processo de `Venda`.

As compras realizadas junto aos fornecedores não possuem atualmente um controlo financeiro equivalente.

Questões como:

- pagamento ao fornecedor;
    
- vencimentos;
    
- contas a pagar;
    

não fazem parte do modelo atual.

---

#### O Pagamento não pertence diretamente à Ótica

Não existe `OpticalStoreId` em `Payment`.

O pertencimento é determinado através da venda.

Isso evita armazenar uma informação que já pode ser obtida através da relação existente.

---

#### O utilizador é armazenado apenas quando existe recebimento

`ReceivedByUserId` não representa quem criou o registo financeiro.

Ele representa especificamente quem recebeu o pagamento.

Por esse motivo, permanece nulo enquanto o pagamento estiver pendente.

Essa distinção evita atribuir uma autoria de recebimento antes que ele realmente aconteça.

---

#### O cancelamento não representa um Reembolso

Na implementação atual, alterar um pagamento de `Received` para `Cancelled` não significa que o dinheiro tenha sido devolvido ao cliente.

Não existem atualmente informações como:

```text
RefundAmount
RefundedAt
RefundedByUserId
RefundMethod
```

nem uma entidade específica para representar estornos.

Por esse motivo, cancelamento de pagamento e reembolso financeiro devem ser tratados como conceitos distintos.

---

## 4. Fluxo do Pagamento

### Venda paga integralmente no registo

```text
Venda: 200 €

Pagamento inicial:
200 €
Cash
Received

Saldo:
0 €
```

É criado apenas um `Pagamento`.

---

### Venda com pagamento inicial parcial

```text
Venda: 300 €

Pagamento inicial:
100 €
DebitCard
Received

Pagamento restante:
200 €
Pending
```

São criados dois pagamentos.

---

### Recebimento posterior

O pagamento restante pode posteriormente ser recebido:

```text
200 €
Pending
    ↓
MBWay
    ↓
200 €
Received
```

Nesse momento também são registados:

- `ReceivedAt`;
    
- `ReceivedByUserId`.
    

---

### Entrega

A venda somente pode ser entregue quando:

```text
Soma dos pagamentos Received
=
Venda.TotalAmount
```

---

### Cancelamento

Caso a venda seja cancelada:

```text
Pending
→ Cancelled
```

e:

```text
Received
→ Cancelled
```

Essa alteração encerra os registos financeiros da venda dentro do sistema, mas não executa automaticamente um reembolso real.

---

## 5. Benefícios

A modelagem adotada oferece diversas vantagens.

- Permite representar pagamento integral ou pagamento inicial com saldo restante.
    
- Distingue claramente valores recebidos e valores pendentes.
    
- Mantém separado o processo comercial do processo financeiro.
    
- Identifica quando e por quem cada valor foi recebido.
    
- Permite utilizar formas de pagamento diferentes entre o pagamento inicial e o saldo final.
    
- Permite calcular o valor recebido e o saldo restante da venda.
    
- Impede a entrega de vendas com valores ainda pendentes.
    
- Mantém o histórico financeiro associado à venda.
    
- Evita adicionar informações financeiras diretamente à entidade `Venda`.
    
- Permite futura evolução para modelos financeiros mais completos.
    

---

## 6. Possíveis Evoluções

Dependendo da evolução do sistema, poderão ser adicionadas novas funcionalidades.

Exemplos:

- reembolso ou estorno de pagamentos;
    
- registo do valor efetivamente reembolsado;
    
- recebimentos parciais do saldo pendente;
    
- múltiplos pagamentos posteriores;
    
- parcelamentos;
    
- comprovativos de pagamento;
    
- integração com terminais de pagamento;
    
- integração direta com MB WAY;
    
- conciliação bancária;
    
- controlo de caixa;
    
- histórico de alterações do pagamento;
    
- pagamentos relacionados às compras junto aos fornecedores.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 7. Conclusão

A entidade `Pagamento` representa os valores financeiros associados a uma venda, incluindo tanto valores já recebidos quanto valores ainda pendentes.

No registo da venda, é criado um pagamento inicial com estado `Received`.

Caso o valor inicial não cubra o total da operação, é criado um segundo pagamento com estado `Pending`, representando o saldo restante.

Esse pagamento pode posteriormente ser recebido integralmente, momento em que são registados a forma de pagamento, a data e o utilizador responsável.

A soma dos pagamentos com estado `Received` determina o valor efetivamente recebido e é utilizada para impedir que uma venda seja entregue enquanto ainda possuir saldo em aberto.

Quando a venda é cancelada, os seus pagamentos passam para `Cancelled`, embora o sistema atual ainda não modele o processo real de reembolso ou estorno financeiro.

Essa separação mantém o processo financeiro organizado e independente da estrutura principal da venda, ao mesmo tempo em que atende ao fluxo de pagamento atualmente implementado no sistema.