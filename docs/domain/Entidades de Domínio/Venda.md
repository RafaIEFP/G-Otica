## 1. Contexto

A entidade `Venda` representa uma operação comercial realizada entre a ótica e um cliente.

Ela centraliza as principais informações relacionadas ao processo de venda, identificando:

- a ótica onde a operação foi realizada;
    
- o cliente atendido;
    
- o utilizador responsável pelo registo;
    
- a receita utilizada, quando aplicável;
    
- o valor total;
    
- o estado atual da venda.
    

Uma venda é composta por um ou mais `ItemVenda` e pode incluir tanto produtos simples, como armações e acessórios, quanto lentes personalizadas.

A venda também está relacionada aos pagamentos realizados pelo cliente e possui um ciclo de vida que depende da existência ou não de lentes que necessitem de produção.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos e a implementação do processo de venda foram definidas as seguintes regras do negócio.

#### Uma venda pertence a uma única ótica

Toda venda é realizada dentro do contexto de uma ótica específica.

Mesmo que um proprietário possua várias unidades, cada venda pertence exclusivamente à ótica onde foi registada.

Essa associação é representada através de `OpticalStoreId`.

---

#### Uma venda é realizada para um cliente

Cada venda está associada a um único cliente através de `ClientId`.

Ao longo do tempo, um cliente pode possuir diversas vendas.

Para participar de uma nova venda, o cliente deve:

- existir;
    
- pertencer à mesma ótica da operação;
    
- estar ativo.
    

---

#### Uma venda é registada por um utilizador

Toda venda mantém a referência ao utilizador responsável pelo seu registo através de `UserId`.

Essa informação representa a autoria da operação.

O utilizador não determina o pertencimento da venda, que continua sendo definido através da ótica.

---

#### Uma venda deve possuir pelo menos um item

Não é permitido registar uma venda sem produtos.

Cada produto comercializado é representado através de um `ItemVenda`.

---

#### Uma venda pode conter vários produtos

Uma mesma venda pode incluir diferentes produtos.

Exemplos:

- armação;
    
- lentes;
    
- estojo;
    
- flanela;
    
- acessórios.
    

Cada produto é representado por um `ItemVenda`, que preserva informações específicas daquela operação, como:

- quantidade;
    
- preço unitário;
    
- desconto;
    
- valor total;
    
- observações.
    

---

#### Apenas produtos ativos da mesma ótica podem ser vendidos

Todos os produtos incluídos na venda devem:

- existir;
    
- pertencer à ótica onde a venda está sendo realizada;
    
- estar ativos;
    
- possuir stock suficiente.
    

Caso algum produto não possua quantidade disponível suficiente, a venda não pode ser registada.

---

#### O registo da venda reduz o stock dos produtos

Ao registar uma venda, as quantidades comercializadas são removidas do stock dos respetivos produtos.

Para cada produto afetado é criada uma `Movimentação de Estoque` do tipo:

```text
Sale
```

Caso o mesmo produto esteja presente em mais de um item, as quantidades são agrupadas para determinar o impacto total no stock.

A criação da venda, a redução do stock e o registo das movimentações fazem parte da mesma transação.

---

#### Nem toda venda exige uma Receita

A associação entre `Venda` e `Receita` é opcional na modelagem.

Vendas compostas apenas por produtos que não sejam lentes podem ser realizadas sem receita.

Exemplos:

- armações;
    
- estojos;
    
- flanelas;
    
- acessórios.
    

---

#### Uma venda com lentes exige uma Receita válida

Quando pelo menos um dos produtos da venda possui `ProductType.Lens`, uma receita torna-se obrigatória.

A receita deve:

- existir;
    
- pertencer ao mesmo cliente;
    
- pertencer ao contexto da mesma ótica;
    
- estar dentro do seu período de validade.
    

Uma venda contendo lentes não pode ser registada sem uma receita válida.

Caso uma receita seja informada numa venda sem lentes, ela também é validada.

---

#### Produtos do tipo Lens exigem ItemLente

Todo `ItemVenda` cujo produto seja do tipo `Lens` deve possuir um `ItemLente`.

Produtos que não sejam do tipo `Lens` não podem possuir `ItemLente`.

Além disso, um item correspondente a uma lente deve possuir:

```text
Quantity = 1
```

Essa regra permite que cada lente personalizada possua individualmente informações como:

- olho correspondente;
    
- DP;
    
- DNP;
    
- tipo da lente;
    
- índice de refração;
    
- material;
    
- cor;
    
- diâmetro;
    
- tratamentos.
    

---

#### Tratamentos são adicionados ao valor da lente

Uma lente pode possuir zero ou vários tratamentos.

Os tratamentos selecionados devem existir, estar ativos e pertencer à mesma ótica da venda.

O preço utilizado em cada tratamento é preservado em `ItemLenteTratamento`.

Esse valor é acrescentado ao total do respetivo `ItemVenda` e, consequentemente, ao valor total da venda.

---

#### O valor total da Venda é calculado pelos seus itens

`Sale.TotalAmount` não é informado diretamente.

Ele é calculado através da soma dos valores totais dos `ItemVenda`.

De forma simplificada:

```text
Valor do produto
- desconto do item
+ tratamentos da lente, quando existirem
=
ItemVenda.TotalAmount
```

e:

```text
Venda.TotalAmount
=
Soma dos ItemVenda.TotalAmount
```

---

#### A venda exige um pagamento inicial

No momento do registo da venda, deve ser informado um pagamento inicial.

Esse pagamento:

- deve possuir valor superior a zero;
    
- deve possuir uma forma de pagamento válida;
    
- não pode ultrapassar o valor total da venda.
    

O pagamento inicial é registado imediatamente com estado:

```text
Received
```

Caso o pagamento inicial seja igual ao total da venda, não existe saldo pendente.

---

#### O valor restante é registado como pagamento pendente

Quando o pagamento inicial é inferior ao valor total da venda, o sistema cria um segundo registo de pagamento correspondente ao valor restante.

Esse pagamento possui inicialmente:

```text
Status = Pending
PaymentMethod = null
ReceivedAt = null
ReceivedByUserId = null
```

Posteriormente, esse pagamento pode ser recebido através do fluxo específico de pagamentos.

---

#### A venda inicia no estado Confirmed

Toda nova venda é criada com:

```text
Status = Confirmed
```

A partir desse estado, o ciclo de vida depende da existência de lentes personalizadas.

---

#### Vendas sem lentes não passam pelo processo de produção

Uma venda que não possui `ItemLente` não necessita das etapas de produção.

O seu fluxo é:

```text
Confirmed
    ↓
Delivered
```

Para ser entregue, a venda deve estar completamente paga.

---

#### Vendas com lentes possuem processo de produção

Quando a venda contém pelo menos um `ItemLente`, ela deve passar pelo fluxo de produção.

O ciclo é:

```text
Confirmed
    ↓
InProduction
    ↓
Ready
    ↓
Delivered
```

A produção só pode ser iniciada quando:

- a venda está em `Confirmed`;
    
- existe pelo menos um `ItemLente`.
    

Uma venda sem lentes não pode ser colocada em `InProduction`.

---

#### Apenas vendas em produção podem ser marcadas como prontas

A transição:

```text
InProduction
    ↓
Ready
```

representa a conclusão do processo de produção das lentes.

Uma venda que não esteja em `InProduction` não pode ser marcada como `Ready`.

---

#### Uma venda só pode ser entregue quando estiver totalmente paga

Independentemente do tipo da venda, a entrega exige que o valor total recebido seja igual ao valor total da operação.

Caso exista saldo pendente, a venda não pode passar para `Delivered`.

Para vendas sem lentes:

```text
Confirmed
+
Totalmente paga
    ↓
Delivered
```

Para vendas com lentes:

```text
Ready
+
Totalmente paga
    ↓
Delivered
```

Uma venda com lentes não pode ser entregue diretamente a partir de `Confirmed` ou `InProduction`.

---

#### Uma venda pode ser cancelada antes da entrega

O cancelamento é permitido quando a venda está em:

- `Confirmed`;
    
- `InProduction`;
    
- `Ready`.
    

Não é permitido cancelar uma venda que já esteja:

- `Delivered`;
    
- `Cancelled`.
    

Após o cancelamento:

```text
Status = Cancelled
```

---

#### O cancelamento altera os pagamentos associados

Quando uma venda é cancelada, os seus pagamentos que ainda não possuem estado `Cancelled` passam para:

```text
Cancelled
```

Essa alteração representa o cancelamento dos registos financeiros dentro do sistema.

A implementação atual não possui um processo específico de reembolso ou devolução financeira ao cliente.

---

#### O cancelamento pode restaurar o stock

O impacto do cancelamento no stock depende do estado da venda.

Se a venda for cancelada em:

```text
Confirmed
```

todos os itens retornam ao stock.

Se for cancelada em:

```text
InProduction
ou
Ready
```

produtos comuns retornam ao stock, porém itens que possuem `ItemLente` não são restaurados.

Isso ocorre porque essas lentes representam produtos personalizados cuja produção já foi iniciada.

Os produtos efetivamente restaurados geram uma `Movimentação de Estoque` do tipo:

```text
SaleCancellation
```

---

## 3. Decisões de Modelagem

#### Pedido foi renomeado para Venda

Inicialmente a entidade foi denominada `Pedido`.

Durante a modelagem foi decidido utilizar o nome `Venda`, por representar de forma mais clara a operação comercial realizada pela ótica para o cliente.

Essa nomenclatura também diferencia naturalmente a venda do processo de `Compra` realizado junto aos fornecedores.

---

#### A Venda mantém o seu próprio estado

A entidade possui `SaleStatus`, atualmente composto por:

```text
Confirmed
InProduction
Ready
Delivered
Cancelled
```

O estado representa a situação atual da operação.

A implementação atual não mantém um histórico separado de todas as mudanças de estado.

---

#### Existem dois fluxos principais de Venda

Foi decidido diferenciar o fluxo conforme a necessidade de produção.

Venda sem lente personalizada:

```text
Confirmed
    ↓
Delivered
```

Venda com lente personalizada:

```text
Confirmed
    ↓
InProduction
    ↓
Ready
    ↓
Delivered
```

O estado `Cancelled` pode ser alcançado a partir de:

```text
Confirmed
InProduction
Ready
```

Essa separação evita obrigar vendas comuns a passarem por etapas de produção que não fazem sentido para esses produtos.

---

#### A Venda referencia a Receita de forma opcional

Foi decidido que `PrescriptionId` pode ser nulo.

Essa decisão permite representar vendas que não necessitam de prescrição.

Entretanto, quando existe pelo menos um produto do tipo `Lens`, a regra de negócio torna a receita obrigatória.

Assim:

```text
Modelagem:
PrescriptionId é opcional
```

mas:

```text
Venda com Lens:
PrescriptionId é obrigatório
```

---

#### Os produtos da venda são representados por ItemVenda

A entidade `Venda` não mantém diretamente as informações comerciais de cada produto.

Essa responsabilidade pertence a `ItemVenda`.

Cada item preserva informações específicas da operação, como:

- produto;
    
- quantidade;
    
- preço unitário;
    
- desconto;
    
- valor total;
    
- observações.
    

Essa estrutura permite manter o histórico mesmo que o cadastro do produto seja alterado posteriormente.

---

#### A Venda não armazena informações específicas das lentes

Informações como:

- olho correspondente;
    
- DP;
    
- DNP;
    
- tipo de lente;
    
- índice de refração;
    
- material;
    
- tratamentos;
    
- cor;
    
- diâmetro;
    

não pertencem diretamente à entidade `Venda`.

Esses dados pertencem ao `ItemLente` associado ao respetivo `ItemVenda`.

Essa separação permite que uma mesma venda contenha simultaneamente produtos comuns e lentes personalizadas.

---

#### O valor da Venda é derivado dos seus itens

`TotalAmount` representa o valor total da operação.

Esse valor é calculado no momento do registo com base nos itens da venda.

O cálculo considera:

- preço atual do produto, preservado posteriormente em `ItemVenda.UnitPrice`;
    
- quantidade;
    
- desconto do item;
    
- preços dos tratamentos associados às lentes.
    

Dessa forma, os valores utilizados na operação ficam preservados nas entidades especializadas correspondentes.

---

#### A Venda coordena o impacto no stock

O registo da venda não representa apenas uma informação comercial.

A operação também reduz o stock dos produtos e cria o respetivo histórico através de `StockMovement`.

Essa responsabilidade é coordenada pelo fluxo de registo da venda, mantendo sincronizados:

```text
Venda
+
Stock dos produtos
+
Movimentações de estoque
```

---

#### A Venda possui pagamentos separados

Os valores recebidos do cliente não são armazenados diretamente em propriedades como:

```text
PaidAmount
RemainingAmount
```

dentro da entidade `Venda`.

Os pagamentos são entidades próprias relacionadas à venda.

O valor recebido é calculado através dos pagamentos com estado `Received`.

O saldo restante pode ser derivado através de:

```text
TotalAmount - ReceivedAmount
```

Essa separação permite preservar as informações específicas de cada pagamento.

---

#### O utilizador regista a venda, mas a venda pertence à ótica

Embora exista `UserId`, a venda pertence sempre à ótica indicada por `OpticalStoreId`.

O utilizador representa quem registou a operação.

Essa separação permite preservar a autoria sem associar a propriedade dos dados à conta individual do colaborador.

---

#### As transições de estado são controladas por operações específicas

O estado da venda não é alterado através de uma atualização genérica.

Existem operações específicas para:

- iniciar produção;
    
- marcar como pronta;
    
- entregar;
    
- cancelar.
    

Cada operação valida o estado atual antes de realizar a transição.

Essa abordagem impede transições que não façam sentido para o processo comercial.

---

#### As mudanças de estado utilizam o estado atual como condição

As alterações de estado são executadas verificando simultaneamente o estado esperado da venda.

Por exemplo:

```text
InProduction → Ready
```

só ocorre se a venda ainda estiver efetivamente em `InProduction`.

Essa abordagem evita que operações concorrentes modifiquem a venda a partir de um estado que já tenha sido alterado por outra ação.

---

## 4. Fluxo da Venda

De forma simplificada, o registo de uma nova venda ocorre da seguinte maneira.

1. O cliente é identificado.
    
2. O sistema valida se o cliente está ativo e pertence à ótica.
    
3. São informados os produtos da venda.
    
4. O sistema valida a existência, estado e stock dos produtos.
    
5. Caso existam produtos do tipo `Lens`, uma receita válida torna-se obrigatória.
    
6. Cada lente recebe o respetivo `ItemLente`.
    
7. São selecionados os tratamentos desejados, quando aplicável.
    
8. Os valores dos `ItemVenda` são calculados.
    
9. O valor total da venda é calculado.
    
10. É registado o pagamento inicial.
    
11. Caso exista saldo restante, é criado um pagamento pendente.
    
12. A venda é criada com estado `Confirmed`.
    
13. O stock dos produtos é reduzido.
    
14. São criadas movimentações de stock do tipo `Sale`.
    

Depois do registo, existem dois fluxos possíveis.

### Venda sem lentes

```text
Confirmed
    ↓
Pagamento completo
    ↓
Delivered
```

### Venda com lentes

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

Enquanto a venda estiver em `Confirmed`, `InProduction` ou `Ready`, também poderá ser cancelada conforme as regras definidas para o stock.

---

## 5. Benefícios

A modelagem adotada oferece diversas vantagens.

- Representa corretamente o processo comercial da ótica.
    
- Permite vendas simples e vendas com lentes personalizadas.
    
- Possui um ciclo de produção apenas quando necessário.
    
- Mantém separadas as responsabilidades entre venda, itens, lentes, tratamentos e pagamentos.
    
- Preserva os preços utilizados no momento da operação.
    
- Permite pagamentos iniciais e valores pendentes.
    
- Impede a entrega de vendas com saldo em aberto.
    
- Integra o registo da venda ao controlo de stock.
    
- Preserva o histórico das saídas e devoluções de produtos.
    
- Permite cancelamentos com regras diferentes conforme o estágio da produção.
    
- Evita devolver ao stock lentes personalizadas que já entraram em produção.
    
- Mantém a autoria da operação separada do pertencimento dos dados.
    
- Permite evoluir o processo comercial sem concentrar todas as informações em uma única entidade.
    

---

## 6. Possíveis Evoluções

Dependendo da evolução do sistema, o processo de venda poderá incorporar novas funcionalidades.

Exemplos:

- reserva de produtos;
    
- orçamentos convertidos em vendas;
    
- histórico completo das alterações de estado;
    
- motivo do cancelamento;
    
- registo do utilizador responsável por cada mudança de estado;
    
- processo específico de reembolso ou estorno;
    
- descontos gerais sobre a venda;
    
- descontos promocionais;
    
- campanhas comerciais;
    
- integração com faturação eletrónica;
    
- comprovativos ou documentos associados à venda;
    
- notificações ao cliente quando a venda estiver pronta.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 7. Conclusão

A entidade `Venda` representa o núcleo do processo comercial da ótica.

Ela relaciona a ótica, o cliente, o utilizador responsável e, quando aplicável, a receita utilizada na produção das lentes.

Os produtos são representados através de `ItemVenda`, enquanto informações específicas de lentes permanecem em `ItemLente` e os tratamentos em `ItemLenteTratamento`.

Os pagamentos são mantidos separadamente e permitem determinar os valores recebidos e pendentes.

Toda nova venda começa em `Confirmed`.

Vendas comuns podem ser entregues diretamente a partir desse estado quando estiverem totalmente pagas, enquanto vendas com lentes personalizadas passam pelas etapas `InProduction` e `Ready` antes da entrega.

O cancelamento é permitido antes da entrega e possui regras específicas para pagamentos e reposição de stock, evitando que lentes personalizadas já em produção retornem ao inventário.

Essa separação mantém o domínio organizado e permite representar tanto vendas simples quanto operações que envolvem produção personalizada, pagamentos, controlo de stock e diferentes etapas do atendimento.