# Documentação do Domínio

Esta pasta reúne a documentação do domínio da aplicação G-Otica.

O objetivo é preservar o conhecimento adquirido durante a análise e modelagem do sistema, transformando as regras de negócio, decisões de domínio e comportamentos consolidados durante o desenvolvimento em documentação permanente.

Além de descrever as entidades existentes, esta documentação procura explicar **não apenas o que foi modelado, mas também por que determinadas decisões foram tomadas**.

Dessa forma, os documentos servem como referência para compreender o funcionamento atual do sistema e apoiar futuras evoluções do projeto.

---

# Organização da Documentação

A documentação das entidades está dividida em dois grupos:

- Entidades de Domínio;
    
- Entidades Especializadas.
    

---

## Entidades de Domínio

Representam os principais conceitos e estruturas utilizadas pelo domínio da aplicação.

Essas entidades possuem identidade e responsabilidade próprias dentro do sistema e participam diretamente dos diferentes processos de negócio.

Atualmente são documentadas:

- Ótica (`OpticalStore`);
    
- Utilizador (`User`);
    
- UtilizadorÓtica (`UserOpticalStore`);
    
- Convite (`Invite`);
    
- Refresh Token (`RefreshToken`);
    
- Cliente (`Client`);
    
- Receita (`Prescription`);
    
- Produto (`Product`);
    
- Tratamento (`Treatment`);
    
- Fornecedor (`Supplier`);
    
- Compra (`Purchase`);
    
- Movimentação de Estoque (`StockMovement`);
    
- Venda (`Sale`);
    
- Pagamento (`Payment`).
    

Algumas dessas entidades representam conceitos diretamente relacionados ao funcionamento da ótica, enquanto outras, como `RefreshToken`, oferecem suporte técnico aos processos necessários para a utilização segura da aplicação.

Os respetivos documentos descrevem, conforme aplicável:

- contexto;
    
- responsabilidade;
    
- regras de domínio;
    
- decisões de modelagem;
    
- relacionamentos;
    
- fluxos relevantes;
    
- benefícios;
    
- possíveis evoluções;
    
- conclusão.
    

---

## Entidades Especializadas

Representam informações que existem no contexto de uma entidade ou processo principal.

Atualmente são documentadas:

- Item de Compra (`PurchaseItem`);
    
- Item de Venda (`SaleItem`);
    
- Item Lente (`ItemLens`);
    
- Item Lente Tratamento (`ItemLensTreatment`).
    

Essas entidades possuem responsabilidades específicas e dependem de estruturas principais do domínio.

Por exemplo:

```text
Purchase
   ↓
PurchaseItem
```

```text
Sale
   ↓
SaleItem
   ↓
ItemLens
   ↓
ItemLensTreatment
```

A utilização dessas entidades permite manter os modelos principais mais simples e separar informações que só existem em determinados contextos.

A sua documentação procura concentrar-se nas responsabilidades e regras específicas, evitando repetir informações já explicadas nas entidades principais.

---

# Visão Geral das Entidades

De forma simplificada, o domínio atual pode ser organizado nos seguintes contextos:

## Gestão da Ótica e da Equipa

```text
OpticalStore
User
UserOpticalStore
Invite
```

Essas entidades representam as óticas, os utilizadores, a participação dos utilizadores em cada unidade e o processo de entrada de novos membros.

---

## Autenticação

```text
User
RefreshToken
```

`RefreshToken` oferece suporte à manutenção e renovação das sessões autenticadas da aplicação.

---

## Clientes e Receitas

```text
Client
Prescription
```

Essas entidades representam os clientes da ótica e o histórico das suas prescrições.

---

## Produtos e Tratamentos

```text
Product
Treatment
```

Representam elementos do catálogo disponibilizado pela ótica.

---

## Compras e Estoque

```text
Supplier
Purchase
PurchaseItem
Product
StockMovement
```

Representam a aquisição de produtos junto aos fornecedores e o controlo das alterações realizadas no stock.

---

## Vendas

```text
Sale
SaleItem
ItemLens
ItemLensTreatment
Payment
```

Representam o processo comercial, incluindo produtos vendidos, lentes personalizadas, tratamentos e pagamentos.

---

# Estrutura dos Documentos

Os documentos seguem uma estrutura semelhante, adaptada conforme a responsabilidade e complexidade de cada entidade.

Nem todas as entidades necessitam das mesmas secções.

---

## Contexto

Apresenta o papel da entidade dentro do domínio e explica por que ela existe.

---

## Responsabilidade

Quando necessário, define de forma mais direta quais informações ou comportamentos pertencem à entidade.

Essa secção é especialmente útil para entidades especializadas.

---

## Regras de Domínio

Documenta as regras que determinam o comportamento da entidade dentro dos processos da aplicação.

Exemplos:

- condições para utilização da entidade;
    
- restrições;
    
- estados permitidos;
    
- relações obrigatórias;
    
- comportamentos resultantes de determinadas operações.
    

As regras devem representar o funcionamento atual do domínio e permanecer independentes, sempre que possível, de detalhes específicos da infraestrutura utilizada.

---

## Decisões de Modelagem

Explica como os conceitos e regras foram representados no sistema.

Podem ser documentadas decisões como:

- criação ou separação de entidades;
    
- escolha de relacionamentos;
    
- utilização de entidades intermediárias;
    
- preservação de dados históricos;
    
- utilização de estados;
    
- separação de responsabilidades.
    

Essa secção é importante para preservar o raciocínio utilizado durante a modelagem.

---

## Fluxos

Quando a entidade participa de um comportamento relevante, o documento pode apresentar de forma simplificada como esse processo ocorre.

Essa secção não é obrigatória para todas as entidades.

Os processos completos da aplicação permanecem documentados separadamente na área de **Processos**.

---

## Benefícios

Apresenta as principais vantagens obtidas através da modelagem escolhida.

---

## Possíveis Evoluções

Regista funcionalidades, informações ou comportamentos que foram considerados, mas que não fazem parte do modelo atual.

Essa secção ajuda a preservar decisões e ideias que poderão ser revisitadas futuramente sem confundi-las com funcionalidades já implementadas.

---

## Conclusão

Resume a responsabilidade da entidade e os principais pontos da modelagem adotada.

---

# Relação com os Processos

A documentação das entidades descreve individualmente os conceitos do domínio.

Os documentos de processos descrevem como essas entidades colaboram para executar operações completas do sistema.

Atualmente, os principais processos documentados são:

- Gestão da Ótica;
    
- Gestão da Equipa e Convites;
    
- Gestão de Clientes;
    
- Gestão de Receitas;
    
- Gestão de Stock;
    
- Compra;
    
- Venda;
    
- Gestão de Pagamentos.
    

Por exemplo, o processo de Venda envolve simultaneamente:

```text
Client
Prescription
Product
Sale
SaleItem
ItemLens
ItemLensTreatment
Treatment
Payment
StockMovement
```

A documentação de cada entidade explica a sua responsabilidade individual, enquanto o processo explica como elas interagem durante a operação.

---

# Princípios Adotados

Durante a elaboração da documentação foram seguidos os seguintes princípios:

- separar regras de domínio de decisões de modelagem;
    
- documentar o comportamento atual do sistema;
    
- modelar conceitos do negócio, e não apenas estruturas da base de dados;
    
- manter responsabilidades bem definidas entre as entidades;
    
- especializar entidades apenas quando necessário;
    
- preservar dados históricos quando relevantes;
    
- evitar redundância entre documentos;
    
- separar informações clínicas, comerciais, financeiras e de fabricação;
    
- respeitar o isolamento dos dados entre óticas;
    
- modelar apenas necessidades justificadas para a versão atual;
    
- distinguir funcionalidades atuais de possíveis evoluções;
    
- manter a documentação alinhada com a implementação;
    
- registar decisões importantes para facilitar futuras alterações.
    

---

# Evolução da Documentação

A documentação do domínio deve acompanhar a evolução da aplicação.

Sempre que uma alteração modificar:

- uma entidade;
    
- uma regra de negócio;
    
- um relacionamento;
    
- um estado;
    
- um processo relevante;
    
- uma decisão de modelagem;
    

o respetivo documento deverá ser revisto.

A documentação deve representar o **comportamento atual do G-Otica**, enquanto funcionalidades ainda não implementadas devem permanecer claramente identificadas como possíveis evoluções.