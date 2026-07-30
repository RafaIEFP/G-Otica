## 1. Contexto
A entidade `Compra` representa o processo de aquisição de produtos pela ótica junto aos seus fornecedores.

Seu objetivo é registrar os pedidos realizados para reposição de estoque, permitindo controlar quais produtos foram adquiridos, de qual fornecedor, por qual utilizador e em qual unidade da empresa.

Assim como uma venda é composta por diversos itens, uma compra também é formada por um ou mais `ItemCompra`.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Uma compra pertence a uma única ótica
Cada compra é realizada por uma unidade específica da empresa.

Mesmo que um proprietário possua várias óticas, cada compra pertence exclusivamente à unidade responsável pela aquisição.

---

#### Uma compra é realizada junto a um fornecedor
Toda compra possui um fornecedor responsável pelo fornecimento dos produtos.

Ao longo do tempo, um mesmo fornecedor pode participar de diversas compras.

---

#### Uma compra é registada por um utilizador
Cada compra possui um utilizador responsável pelo seu registo.

Esse utilizador representa quem efetuou o pedido junto ao fornecedor.

---

#### Uma compra pode conter vários produtos
Uma compra pode incluir diversos produtos em um único pedido.

Exemplos:
- lentes;
- armações;
- estojos;
- flanelas;
- acessórios.

Cada produto adquirido é representado por um `ItemCompra`.

---

#### Uma compra destina-se à reposição de estoque
Os produtos adquiridos por meio de uma compra têm como finalidade abastecer o estoque da ótica.

Esses produtos poderão ser posteriormente comercializados nas vendas aos clientes.

---

## 3. Decisões de Modelagem

#### Compra foi separada da Venda
Inicialmente foi considerada a utilização de uma única entidade para representar compras e vendas.

Essa abordagem foi descartada.

Embora ambas representem operações comerciais, seus objetivos, participantes e regras de negócio são diferentes.

A separação tornou o domínio mais claro e facilitou a evolução futura de cada processo.

---

#### Os produtos da compra serão representados por ItemCompra
A entidade `Compra` não possui relacionamento direto com `Produto`.

Cada produto adquirido é representado por um `ItemCompra`, permitindo registrar informações específicas como:

- quantidade;
- valor unitário;
- valor total.

Essa estrutura é semelhante à utilizada em `Venda`, mantendo consistência entre os dois processos.

---

#### A Compra não possui Receita
Diferentemente das vendas de lentes graduadas, compras realizadas junto aos fornecedores não dependem de receita médica.

Por esse motivo, não existe qualquer relacionamento entre `Compra` e `Receita`.

---

#### A Compra não possui informações específicas de lentes
Durante uma compra, a ótica adquire produtos para seu estoque.

Informações relacionadas à personalização de lentes, como:
- DP;
- DNP;
- Índice;
- Tratamentos;
- Material;

não fazem parte desse processo.

Esses dados pertencem exclusivamente às vendas de lentes realizadas para clientes.

---

#### O pagamento da compra não será controlado na primeira versão
Durante a modelagem foi discutida a criação de uma entidade específica para pagamentos de compras.

Essa ideia foi descartada.

O objetivo da primeira versão é controlar apenas o processo operacional de aquisição de produtos.

Questões financeiras relacionadas ao pagamento dos fornecedores poderão ser incorporadas em versões futuras, caso exista necessidade.

---

## 4. Fluxo da Compra

De forma simplificada, o processo ocorre da seguinte maneira.

1. O utilizador seleciona o fornecedor.
2. É criada uma nova compra.
3. Os produtos são adicionados através de `ItemCompra`.
4. O valor total é calculado.
5. A compra é enviada ao fornecedor.
6. Os produtos são recebidos pela ótica.
7. O estoque é atualizado.

---

## 5. Benefícios
A modelagem adotada oferece diversas vantagens.

- Separa claramente os processos de compra e venda.
- Permite controlar o histórico de aquisições junto aos fornecedores.
- Mantém uma estrutura consistente com a entidade `Venda`.
- Simplifica a gestão do estoque.
- Facilita futuras integrações com processos de reposição e inventário.
- Evita misturar regras comerciais com regras de abastecimento.

---

## 6. Possíveis Evoluções
Dependendo da evolução do sistema, a entidade poderá incorporar novas funcionalidades.

Exemplos:
- Controlo de pagamentos ao fornecedor.
- Prazo previsto para entrega.
- Número da nota fiscal ou documento equivalente.
- Recebimento parcial de mercadorias.
- Controlo de devoluções.
- Integração automática com movimentações de estoque.
- Histórico de alterações de estado.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 7. Conclusão
A entidade `Compra` representa o processo de aquisição de produtos realizado pela ótica junto aos seus fornecedores.

Sua responsabilidade é controlar o abastecimento da unidade, registrando quais produtos foram adquiridos, de quem foram comprados e quem realizou a operação.

A separação entre `Compra` e `Venda` tornou o domínio mais expressivo, permitindo que cada processo evolua de forma independente e mantendo a modelagem alinhada ao funcionamento real da ótica.