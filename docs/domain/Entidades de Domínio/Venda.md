## 1. Contexto
A entidade `Venda` representa uma operação comercial realizada entre a ótica e um cliente.

Ela centraliza todas as informações relacionadas ao processo de venda, identificando o cliente atendido, o utilizador responsável pelo atendimento, a ótica onde a venda foi realizada e, quando aplicável, a receita utilizada para a produção das lentes.

Uma venda é composta por um ou mais itens, podendo incluir produtos simples, como armações e acessórios, ou produtos personalizados, como lentes graduadas.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Uma venda pertence a uma única ótica
Toda venda é realizada por uma unidade específica da empresa.

Mesmo que um proprietário possua várias óticas, cada venda pertence exclusivamente à unidade onde foi efetuada.

---

#### Uma venda é realizada para um cliente
Cada venda está associada a um único cliente.

Ao longo do tempo, um cliente pode realizar diversas compras na mesma ótica.

---

#### Uma venda é registada por um utilizador
Toda venda possui um utilizador responsável pelo atendimento e pelo registo da operação.

O utilizador representa quem executou a venda, mas não determina o pertencimento dos dados.

---

#### Uma venda pode conter vários produtos
Uma mesma venda pode incluir diferentes produtos.

Exemplos:
- armação;
- lentes;
- estojo;
- flanela;
- acessórios.

Cada produto é representado por um `ItemVenda`.

---

#### Nem toda venda utiliza uma receita
Apenas vendas de lentes graduadas necessitam de uma receita médica.

Produtos como armações e acessórios podem ser vendidos sem qualquer prescrição.

Por esse motivo, a associação entre `Venda` e `Receita` é opcional.

---

#### Uma venda pode gerar um ou mais pagamentos
O cliente pode efetuar o pagamento de diferentes formas.

No contexto identificado para Portugal, é comum que o valor seja pago:
- integralmente no momento da compra ou levantamento;
- parcialmente na encomenda e o restante na entrega.

Assim, uma venda pode possuir mais de um registo de pagamento.

---

## 3. Decisões de Modelagem

#### Pedido foi renomeado para Venda
Inicialmente a entidade foi denominada `Pedido`.

Durante a modelagem foi decidido utilizar o nome `Venda`, por representar de forma mais clara o processo comercial realizado pela ótica.

Essa nomenclatura também diferencia naturalmente o processo de compra junto aos fornecedores.

---

#### A Venda referencia a Receita de forma opcional
Foi decidido que `ReceitaId` poderá ser nulo.

Essa decisão permite representar tanto vendas de lentes graduadas quanto vendas de produtos que não exigem prescrição médica.

---

#### Os produtos da venda serão representados por ItemVenda
A entidade `Venda` não possui ligação direta com `Produto`.

Esse relacionamento é realizado através de `ItemVenda`, permitindo que uma única venda contenha diversos produtos.

Essa abordagem também possibilita armazenar informações específicas de cada item, como quantidade, desconto e valor unitário.

---

#### A Venda não armazenará informações específicas das lentes
Informações como:
- DP;
- DNP;
- Índice;
- Material;
- Tratamentos;
- Cor;

não pertencem à venda.

Esses dados fazem parte apenas dos itens que representam lentes e são armazenados em `ItemLente`.

---

#### O utilizador registra a venda, mas a venda pertence à ótica
Embora exista um `UtilizadorId`, a venda pertence sempre à ótica.

O utilizador identifica apenas quem realizou o atendimento.

Essa separação mantém a organização dos dados e facilita o controlo por unidade.

---

## 4. Fluxo da Venda
De forma simplificada, o processo de venda ocorre da seguinte maneira.

1. O cliente é identificado.
2. O utilizador inicia uma nova venda.
3. Caso necessário, uma receita é associada.
4. Os produtos são adicionados através de `ItemVenda`.
5. Se houver lentes graduadas, é criado um `ItemLente` com as informações específicas de fabricação.
6. São associados os tratamentos escolhidos para cada lente.
7. O valor total da venda é calculado.
8. São registados um ou mais pagamentos.
9. A venda é concluída.

---

## 5. Benefícios
A modelagem adotada oferece diversas vantagens.

- Representa corretamente o processo comercial da ótica.
- Permite vendas simples e vendas de lentes graduadas.
- Evita duplicação de informações.
- Mantém separadas as responsabilidades entre as entidades.
- Facilita futuras integrações com faturação e controlo financeiro.
- Permite diferentes formas de pagamento para uma mesma venda.

---

## 6. Possíveis Evoluções
Dependendo da evolução do sistema, a entidade poderá incorporar novas funcionalidades.

Exemplos:
- Reserva de produtos.
- Orçamentos convertidos em vendas.
- Histórico de alterações de estado.
- Cancelamentos com justificativa.
- Descontos promocionais.
- Campanhas comerciais.
- Integração com faturação eletrónica.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 7. Conclusão

A entidade `Venda` representa o núcleo do processo comercial da ótica.

Ela centraliza o relacionamento entre cliente, utilizador, produtos, receita e pagamentos, mantendo cada responsabilidade distribuída para a entidade mais adequada.

Essa modelagem permite representar tanto vendas simples quanto vendas de lentes graduadas, mantendo o domínio organizado, flexível e preparado para futuras evoluções sem aumentar desnecessariamente a complexidade da primeira versão.