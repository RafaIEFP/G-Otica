## Objetivo

Este documento reúne os principais requisitos funcionais definidos para a primeira versão do G-Otica.

Os requisitos funcionais descrevem **as funcionalidades que o sistema deve disponibilizar aos seus utilizadores**, servindo como referência para o desenvolvimento, validação e evolução da aplicação.

Os requisitos apresentados refletem o modelo de domínio e os fluxos atualmente definidos para o sistema.

---

# Autenticação e Conta de Utilizador

O sistema deverá permitir:

- registar uma nova conta de utilizador;
    
- autenticar um utilizador através de email e palavra-passe;
    
- disponibilizar tokens de acesso e refresh token após autenticação válida;
    
- renovar a autenticação através de refresh token;
    
- terminar a sessão do utilizador;
    
- consultar os dados do próprio perfil;
    
- atualizar os dados do próprio perfil;
    
- alterar a palavra-passe;
    
- desativar a própria conta;
    
- reativar uma conta previamente desativada.
    

O sistema deverá impedir a desativação da conta enquanto o utilizador for proprietário de uma ótica.

Antes da desativação, a propriedade deverá ser transferida para outro utilizador elegível.

---

# Gestão de Óticas

O sistema deverá permitir:

- registar uma nova ótica;
    
- associar automaticamente o utilizador responsável pela criação como proprietário da ótica;
    
- consultar uma ótica;
    
- consultar as óticas às quais o utilizador possui acesso;
    
- atualizar os dados de uma ótica;
    
- desativar uma ótica;
    
- transferir a propriedade de uma ótica para outro utilizador elegível.
    

A transferência de propriedade deverá atualizar os papéis dos utilizadores envolvidos, tornando o novo utilizador `owner` e o antigo proprietário `manager`.

---

# Gestão de Utilizadores nas Óticas

O sistema deverá permitir:

- consultar os utilizadores associados a uma ótica;
    
- definir o papel de cada utilizador dentro da ótica;
    
- alterar o papel de um utilizador;
    
- desativar a associação de um utilizador com uma ótica;
    
- reativar uma associação anteriormente desativada.
    

Os papéis atualmente suportados são:

- `owner`;
    
- `manager`;
    
- `salesperson`.
    

O papel `owner` deverá ser controlado pelo processo de propriedade da ótica e não poderá ser atribuído através do fluxo comum de alteração de papel.

---

# Gestão de Convites

O sistema deverá permitir ao proprietário da ótica:

- convidar um utilizador através do seu endereço de email;
    
- definir no convite o papel `manager` ou `salesperson`;
    
- enviar ao destinatário um token de convite;
    
- validar um convite através do token recebido;
    
- permitir que o utilizador convidado aceite um convite válido;
    
- criar a associação entre o utilizador e a ótica após a aceitação.
    

O sistema deverá:

- impedir convites para utilizadores que já possuam associação com a ótica, mesmo que essa associação esteja inativa;
    
- impedir múltiplos convites válidos pendentes para o mesmo email na mesma ótica;
    
- impedir a utilização de convites expirados;
    
- associar o convite apenas ao utilizador cujo email corresponda ao destinatário.
    

Utilizadores com associação inativa deverão utilizar o processo de reativação em vez de receber um novo convite.

---

# Gestão de Clientes

O sistema deverá permitir:

- registar clientes;
    
- consultar um cliente;
    
- consultar a lista de clientes de uma ótica;
    
- atualizar os dados de um cliente;
    
- desativar um cliente;
    
- reativar um cliente.
    

Cada cliente deverá pertencer a uma única ótica.

Clientes inativos deverão permanecer no histórico, mas não poderão ser utilizados em novas operações que exijam um cliente ativo.

---

# Gestão de Receitas

O sistema deverá permitir:

- registar uma receita para um cliente;
    
- consultar uma receita;
    
- consultar o histórico de receitas de um cliente;
    
- associar uma receita a uma venda.
    

O sistema deverá preservar receitas anteriormente registadas como parte do histórico clínico do cliente.

Uma receita utilizada numa venda deverá:

- pertencer ao cliente da venda;
    
- pertencer ao contexto da mesma ótica;
    
- estar dentro do seu período de validade.
    

Vendas que contenham produtos do tipo `Lens` deverão obrigatoriamente possuir uma receita válida.

---

# Gestão de Produtos

O sistema deverá permitir:

- registar produtos;
    
- definir a quantidade inicial de stock;
    
- consultar um produto;
    
- consultar os produtos de uma ótica;
    
- atualizar os dados de um produto;
    
- desativar um produto;
    
- reativar um produto.
    

Cada produto deverá pertencer a uma única ótica.

O código do produto deverá ser único dentro da respetiva ótica.

Produtos inativos deverão permanecer no histórico, mas não poderão ser utilizados em novas compras ou vendas.

---

# Gestão de Stock

O sistema deverá permitir:

- consultar a quantidade atual disponível de cada produto;
    
- realizar ajustes manuais de stock;
    
- aumentar ou diminuir o stock através de um ajuste manual;
    
- exigir uma justificativa para ajustes manuais;
    
- impedir operações que façam o stock tornar-se negativo;
    
- consultar o histórico de movimentações de stock de um produto.
    

O sistema deverá gerar movimentações de stock automaticamente quando ocorrer:

- registo inicial de produto com quantidade superior a zero;
    
- compra de produtos;
    
- venda de produtos;
    
- ajuste manual;
    
- cancelamento de venda com reposição de produtos.
    

As movimentações deverão preservar a quantidade alterada, a origem da alteração, a data e o utilizador responsável.

---

# Gestão de Fornecedores

O sistema deverá permitir:

- registar fornecedores;
    
- consultar um fornecedor;
    
- consultar os fornecedores de uma ótica;
    
- atualizar os dados de um fornecedor;
    
- desativar um fornecedor;
    
- reativar um fornecedor.
    

Cada fornecedor deverá pertencer a uma única ótica.

Fornecedores inativos deverão permanecer no histórico das compras anteriores, mas não poderão ser utilizados em novas compras.

---

# Gestão de Tratamentos

O sistema deverá permitir:

- registar tratamentos;
    
- definir o preço base de cada tratamento;
    
- consultar um tratamento;
    
- consultar os tratamentos disponibilizados por uma ótica;
    
- atualizar os dados e o preço de um tratamento;
    
- desativar um tratamento;
    
- reativar um tratamento.
    

Cada tratamento deverá pertencer a uma única ótica.

Tratamentos inativos deverão permanecer associados às vendas anteriores, mas não poderão ser utilizados em novas vendas.

O preço utilizado numa venda deverá ser preservado independentemente de alterações posteriores no preço base do tratamento.

---

# Gestão de Compras

O sistema deverá permitir:

- registar compras realizadas junto a fornecedores;
    
- selecionar o fornecedor responsável pela compra;
    
- adicionar um ou mais produtos à compra;
    
- informar a quantidade adquirida de cada item;
    
- informar o preço unitário de aquisição de cada item;
    
- calcular o valor total de cada item;
    
- calcular o valor total da compra;
    
- consultar uma compra;
    
- consultar o histórico de compras da ótica.
    

Ao registar uma compra, o sistema deverá:

- validar que o fornecedor está ativo e pertence à ótica;
    
- validar que os produtos estão ativos e pertencem à ótica;
    
- aumentar o stock dos produtos adquiridos;
    
- gerar as respetivas movimentações de stock.
    

O registo da compra, a alteração do stock e a criação das movimentações deverão constituir uma única operação consistente.

A versão atual não possui estados para compras. Uma compra registada representa uma aquisição já incorporada ao stock.

---

# Gestão de Vendas

O sistema deverá permitir:

- registar uma nova venda;
    
- selecionar o cliente da venda;
    
- adicionar um ou mais produtos;
    
- aplicar descontos individualmente aos itens;
    
- registar observações nos itens;
    
- associar uma receita quando aplicável;
    
- registar as informações técnicas de cada lente;
    
- associar tratamentos às lentes;
    
- calcular o valor de cada item;
    
- calcular automaticamente o valor total da venda;
    
- registar o pagamento inicial;
    
- reduzir o stock dos produtos vendidos;
    
- consultar uma venda;
    
- consultar o histórico de vendas da ótica;
    
- iniciar a produção de vendas com lentes;
    
- marcar vendas em produção como prontas;
    
- entregar uma venda;
    
- cancelar uma venda antes da entrega.
    

Toda nova venda deverá iniciar no estado `Confirmed`.

---

## Vendas sem Lentes Personalizadas

Uma venda que não possua `ItemLens` deverá seguir o fluxo:

```text
Confirmed
    ↓
Delivered
```

A entrega somente poderá ocorrer quando a venda estiver totalmente paga.

---

## Vendas com Lentes Personalizadas

Uma venda que possua pelo menos um `ItemLens` deverá seguir o fluxo:

```text
Confirmed
    ↓
InProduction
    ↓
Ready
    ↓
Delivered
```

O sistema deverá:

- permitir iniciar produção apenas para vendas confirmadas que possuam lentes;
    
- permitir marcar como pronta apenas uma venda em produção;
    
- permitir entregar uma venda com lentes apenas quando estiver `Ready` e totalmente paga.
    

---

## Cancelamento de Vendas

O sistema deverá permitir cancelar vendas nos estados:

- `Confirmed`;
    
- `InProduction`;
    
- `Ready`.
    

Vendas `Delivered` ou já `Cancelled` não poderão ser canceladas.

Quando uma venda em `Confirmed` for cancelada, os seus produtos deverão retornar ao stock.

Quando uma venda em `InProduction` ou `Ready` for cancelada:

- produtos comuns deverão retornar ao stock;
    
- produtos associados a `ItemLens` não deverão retornar ao stock.
    

Os produtos restaurados deverão gerar movimentações do tipo `SaleCancellation`.

---

# Gestão de Lentes

Quando um produto do tipo `Lens` fizer parte de uma venda, o sistema deverá:

- exigir um `ItemLens`;
    
- exigir quantidade igual a uma unidade por item;
    
- identificar o olho correspondente;
    
- permitir informar DP e DNP;
    
- registar o tipo de lente;
    
- registar o índice de refração;
    
- registar o material;
    
- permitir informar a cor;
    
- registar o diâmetro;
    
- permitir associar zero ou vários tratamentos.
    

Produtos que não sejam do tipo `Lens` não poderão possuir `ItemLens`.

---

# Gestão de Pagamentos

O sistema deverá permitir:

- registar um pagamento inicial juntamente com a venda;
    
- permitir que o pagamento inicial cubra total ou parcialmente o valor da venda;
    
- criar automaticamente um pagamento pendente quando existir saldo restante;
    
- consultar os pagamentos no contexto da venda;
    
- receber posteriormente o pagamento pendente;
    
- definir a forma de pagamento no momento do recebimento;
    
- registar a data do recebimento;
    
- registar o utilizador responsável pelo recebimento.
    

A versão atual deverá suportar o seguinte modelo:

```text
Pagamento inicial
+
Saldo restante
```

O saldo restante deverá ser representado por um único pagamento `Pending` e deverá ser recebido integralmente.

A versão atual não necessita suportar parcelamento livre em várias prestações.

Uma venda somente poderá ser entregue quando o valor total dos pagamentos `Received` for igual ao valor total da venda.

Ao cancelar uma venda, os pagamentos associados deverão passar para `Cancelled`.

O cancelamento do pagamento não deverá ser interpretado como um reembolso financeiro efetivamente realizado.

---

# Consultas e Paginação

O sistema deverá permitir consultas das principais coleções do domínio, incluindo:

- clientes;
    
- produtos;
    
- fornecedores;
    
- tratamentos;
    
- compras;
    
- vendas;
    
- receitas;
    
- movimentações de stock.
    

Sempre que aplicável, listas potencialmente extensas deverão ser disponibilizadas de forma paginada.

---

# Funcionalidades Futuras

As funcionalidades abaixo foram identificadas como possíveis evoluções, mas não fazem parte do escopo atual da primeira versão.

O sistema poderá futuramente permitir:

- reativar uma ótica desativada;
    
- realizar reembolsos ou estornos de pagamentos;
    
- receber o saldo de uma venda em vários pagamentos independentes;
    
- controlar stock mínimo;
    
- emitir alertas de stock reduzido;
    
- realizar inventários periódicos;
    
- transferir stock entre óticas;
    
- reservar produtos;
    
- criar orçamentos e convertê-los em vendas;
    
- registar motivos de cancelamento;
    
- manter histórico completo das alterações de estado das vendas;
    
- integrar com sistemas de faturação;
    
- emitir relatórios financeiros;
    
- emitir relatórios de vendas;
    
- disponibilizar dashboards com indicadores da ótica;
    
- enviar lembretes para realização de novos exames;
    
- enviar notificações aos clientes;
    
- integrar o processo de produção com laboratórios;
    
- anexar documentos ou imagens às receitas;
    
- controlar pagamentos realizados a fornecedores.
    

---

# Observações

- Este documento representa os requisitos funcionais atualmente definidos para a primeira versão do G-Otica.
    
- As funcionalidades refletem o comportamento atual do domínio e da API.
    
- As regras de negócio mais detalhadas permanecem documentadas nas respetivas entidades e processos.
    
- Novos requisitos poderão ser incorporados conforme a evolução do projeto.