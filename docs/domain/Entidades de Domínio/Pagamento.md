## 1. Contexto
A entidade `Pagamento` representa os valores recebidos pela ótica em decorrência de uma venda.

Seu objetivo é registrar cada recebimento realizado pelo cliente, permitindo controlar o histórico financeiro da venda e acompanhar seu estado de pagamento.

Uma venda pode possuir um ou mais pagamentos, dependendo da forma como o cliente optou por efetuar o pagamento.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Todo pagamento pertence a uma venda
Cada pagamento está associado a uma única venda.

Ele representa um recebimento referente àquela operação comercial.

---

#### Uma venda pode possuir vários pagamentos
O cliente pode optar por dividir o pagamento em diferentes momentos.

No contexto estudado, as formas mais comuns são:
- pagamento integral;
- entrada no momento da encomenda e restante na entrega.

Assim, uma única venda pode possuir mais de um registo de pagamento.

---

#### O pagamento possui uma forma de pagamento
Cada pagamento é realizado utilizando uma única forma de pagamento.

Exemplos identificados durante o levantamento:
- MB WAY;
- Cartão de Débito;
- Cartão de Crédito;
- Dinheiro;
- Transferência Bancária.

---

#### O pagamento possui um estado
Cada registo de pagamento possui um estado que representa sua situação atual.

Exemplos:
- Pendente;
- Recebido;
- Cancelado.

---

#### O pagamento registra apenas valores efetivamente recebidos
Cada registo representa um recebimento realizado ou esperado.

O histórico completo da venda é obtido através da soma de todos os pagamentos associados.

---

## 3. Decisões de Modelagem

#### O pagamento foi separado da Venda
Embora toda venda possua um valor total, foi criada uma entidade específica para representar os pagamentos.

Essa separação permite registrar múltiplos recebimentos para a mesma venda e acompanhar sua evolução financeira.

---

#### O valor total da venda não depende de um único pagamento
A entidade `Venda` armazena o valor total da operação.

Já a entidade `Pagamento` registra apenas os valores recebidos individualmente.

Essa separação permite controlar pagamentos parciais sem alterar a estrutura da venda.

---

#### Não será implementado parcelamento
Durante o levantamento do domínio foi identificado que, no contexto da ótica estudada em Portugal, não existe a necessidade de controlar parcelamentos como ocorre em outros países.

Por esse motivo, a modelagem foi simplificada para representar apenas pagamentos independentes associados à venda.

---

#### Não existirão pagamentos para Compras
Também foi discutida a criação de pagamentos relacionados às compras realizadas junto aos fornecedores.

Essa ideia foi descartada.

Na primeira versão, o objetivo é controlar apenas o processo operacional de aquisição de produtos.

Questões financeiras relacionadas aos fornecedores poderão ser incorporadas futuramente, caso se tornem uma necessidade do negócio.

---

#### As formas de pagamento serão controladas por enum
As formas de pagamento identificadas durante o levantamento serão representadas por um enum.

Essa abordagem simplifica a implementação e atende às necessidades atuais do sistema.

Caso novas formas de pagamento sejam necessárias no futuro, a modelagem poderá evoluir para uma entidade específica.

---

## 4. Fluxo do Pagamento

De forma simplificada, o processo ocorre da seguinte maneira.

1. Uma venda é registada.
2. O valor total da venda é calculado.
3. O cliente escolhe a forma de pagamento.
4. Um ou mais pagamentos são registados.
5. A soma dos pagamentos determina a situação financeira da venda.

---

## 5. Benefícios
A modelagem adotada oferece diversas vantagens.

- Permite pagamentos parciais.
- Mantém separado o processo comercial do processo financeiro.
- Representa corretamente o fluxo observado na ótica.
- Evita dependência de um único pagamento por venda.
- Simplifica futuras integrações financeiras.
- Mantém a modelagem aderente às necessidades da primeira versão.

---

## 6. Possíveis Evoluções
Dependendo da evolução do sistema, poderão ser adicionadas novas funcionalidades.

Exemplos:
- Estorno de pagamentos.
- Comprovantes de pagamento.
- Integração com terminais de pagamento.
- Integração com MB WAY.
- Parcelamentos.
- Pagamentos de compras junto aos fornecedores.
- Conciliação bancária.
- Controlo de caixa.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 7. Conclusão

A entidade `Pagamento` representa os recebimentos efetuados pelos clientes em decorrência de uma venda.

Sua responsabilidade é registrar cada pagamento individualmente, permitindo controlar pagamentos integrais ou parciais sem alterar a estrutura da venda.

A separação entre `Venda` e `Pagamento` tornou o modelo mais flexível, mantendo o processo comercial independente do processo financeiro e refletindo o funcionamento real da ótica estudada.