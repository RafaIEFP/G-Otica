## 1. Contexto

A entidade `Receita` representa a prescrição apresentada pelo cliente após a realização de um exame de vista.

Ela armazena as informações da prescrição necessárias para representar a condição visual do cliente naquele momento, além dos dados de identificação do profissional responsável.

Cada receita representa um registo independente e deve ser preservada mesmo quando uma nova receita for emitida posteriormente.

Dessa forma, as receitas formam o histórico clínico do cliente ao longo do tempo.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Cada exame pode originar uma nova receita

Quando o cliente realiza um novo exame e recebe uma nova prescrição, um novo registo de `Receita` deve ser criado.

Mesmo quando os valores permanecem inalterados, a nova receita representa outro momento do histórico clínico do cliente.

As receitas anteriores não são substituídas.

---

#### Um cliente pode possuir várias receitas

Ao longo do tempo, um cliente pode acumular diversas receitas.

Cada uma permanece associada ao respetivo cliente, permitindo consultar o histórico das prescrições registadas.

---

#### Uma receita pertence a um único cliente

Toda receita possui uma referência obrigatória para `Cliente` através de `ClientId`.

A receita não possui `OpticalStoreId` diretamente.

O seu pertencimento à ótica é determinado através do cliente:

```text
Receita
   ↓
Cliente
   ↓
Ótica
```

Dessa forma, uma receita registada para um cliente de determinada ótica não pode ser utilizada por um cliente pertencente a outra unidade.

---

#### Apenas clientes ativos podem receber novas receitas

Para registar uma nova receita, o cliente deve:

- existir;
    
- pertencer à ótica da operação;
    
- estar ativo.
    

A desativação posterior do cliente não elimina as receitas já existentes, permitindo preservar o seu histórico.

---

#### A receita identifica o profissional responsável

Cada receita armazena:

- nome do profissional;
    
- número de registo profissional.
    

Essas informações são obrigatórias e permitem identificar a origem da prescrição registada.

---

#### A data da receita não pode estar no futuro

`PrescriptionDate` representa a data da prescrição.

No momento do registo, essa data deve ser igual ou anterior à data atual.

---

#### A receita possui uma data de validade

Cada receita possui `ExpirationDate`.

No momento do registo, essa data não pode estar expirada.

Durante uma venda, a receita também é novamente validada.

Uma receita cuja data de validade já tenha terminado não pode ser utilizada numa nova venda.

---

#### A recomendação de retorno pertence à Receita

Quando o profissional recomenda uma nova avaliação em determinada data, essa informação pode ser armazenada através de:

```text
RecommendedReturnDate
```

Essa propriedade é opcional.

A recomendação pertence à prescrição correspondente e não ao cadastro do cliente.

---

#### A Receita armazena os dados clínicos da prescrição

A entidade permite armazenar informações como:

- esfera do olho direito e esquerdo;
    
- cilindro do olho direito e esquerdo;
    
- eixo do olho direito e esquerdo;
    
- acuidade visual de cada olho;
    
- adição;
    
- acuidade visual para perto;
    
- observações.
    

Os campos clínicos são opcionais, permitindo representar diferentes tipos de prescrição.

---

#### DP e DNP não pertencem à Receita

Embora algumas prescrições possam eventualmente apresentar essas medidas, no modelo atual DP e DNP representam dados utilizados na personalização da lente durante a venda.

Por esse motivo, essas informações pertencem ao `ItemLente` e não à `Receita`.

---

#### Uma venda com lentes exige uma Receita

Na implementação atual, sempre que uma venda contém pelo menos um produto com `ProductType.Lens`, uma receita deve ser informada.

Essa receita deve:

- existir;
    
- pertencer ao mesmo cliente da venda;
    
- pertencer ao contexto da mesma ótica;
    
- ainda estar dentro do seu período de validade.
    

Sem uma receita válida, a venda contendo lentes não pode ser registada.

---

#### Nem toda venda exige uma Receita

Vendas compostas apenas por produtos que não sejam lentes podem ser realizadas sem receita.

Exemplos:

- armações;
    
- estojos;
    
- flanelas;
    
- acessórios.
    

A referência da `Venda` para `Receita` é, portanto, opcional.

Caso uma receita seja informada mesmo numa venda sem lentes, ela continua sendo validada quanto ao cliente, ótica e validade.

---

#### Uma Receita pode ser utilizada em mais de uma venda

Enquanto permanecer válida, uma mesma receita pode ser referenciada por diferentes vendas do mesmo cliente.

A utilização da receita numa venda não a invalida nem impede a sua utilização posterior.

---

## 3. Decisões de Modelagem

#### A Receita não foi dividida em múltiplas tabelas

Durante a modelagem foi considerada a possibilidade de separar os dados da receita em diferentes entidades.

Essa ideia foi descartada.

Apesar da quantidade de atributos, todos pertencem ao mesmo conceito de prescrição e são registados como parte do mesmo documento clínico.

Criar várias entidades aumentaria a complexidade sem trazer benefícios necessários ao modelo atual.

---

#### Os dados dos dois olhos permanecem na mesma entidade

As informações dos olhos direito e esquerdo são armazenadas diretamente em `Receita`.

Exemplos:

```text
RightEyeSphere
LeftEyeSphere

RightEyeCylinder
LeftEyeCylinder

RightEyeAxis
LeftEyeAxis
```

Não foram criadas entidades separadas para cada olho.

Essa decisão mantém a estrutura simples e adequada ao conjunto atual de informações da prescrição.

---

#### DP e DNP não pertencem à Receita

Foi decidido não armazenar DP e DNP nesta entidade.

Essas medidas estão relacionadas à configuração da lente personalizada produzida para determinada venda.

Por esse motivo, são armazenadas em `ItemLente`.

Essa separação distingue:

```text
Receita
→ prescrição clínica
```

de:

```text
ItemLente
→ características da lente fabricada para aquela venda
```

---

#### A Venda referencia a Receita de forma opcional

A entidade `Venda` possui:

```text
PrescriptionId?
```

Essa referência é opcional porque nem todas as vendas necessitam de prescrição.

Entretanto, quando a venda contém um produto do tipo `Lens`, a regra de negócio torna a receita obrigatória.

Assim, existe uma diferença entre:

```text
Modelagem:
PrescriptionId é nullable
```

e:

```text
Regra de negócio:
Venda com Lens exige PrescriptionId
```

---

#### A receita é validada dentro do contexto do cliente

Ao selecionar uma receita para uma venda, o sistema não verifica apenas o seu identificador.

A consulta considera simultaneamente:

```text
PrescriptionId
+
ClientId
+
OpticalStoreId
```

Isso impede que uma receita pertencente a outro cliente ou outra ótica seja utilizada na venda.

---

#### A Receita preserva o histórico clínico

A implementação atual possui operações para:

- registar receitas;
    
- consultar uma receita;
    
- listar as receitas de um cliente.
    

Não existe atualmente um fluxo para editar ou eliminar uma receita já registada.

Essa abordagem está alinhada à ideia de preservar cada prescrição como um registo histórico independente.

Caso uma nova prescrição seja emitida, deve ser criado um novo registo.

---

#### As receitas são consultadas por cliente

A listagem das receitas é realizada dentro do contexto de um cliente específico.

Os resultados são apresentados de forma paginada e ordenados pela data da prescrição, das mais recentes para as mais antigas.

Isso facilita a consulta ao histórico visual do cliente.

---

#### A Receita é a fonte da verdade do histórico clínico

Informações relacionadas às prescrições permanecem na entidade `Receita`.

Dados como:

- data da prescrição;
    
- grau registado;
    
- acuidade visual;
    
- profissional responsável;
    
- validade;
    
- recomendação de retorno;
    

não precisam ser duplicados no cadastro do cliente.

Essa decisão reduz redundâncias e mantém o histórico organizado através das próprias receitas.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Preserva o histórico das prescrições do cliente.
    
- Evita substituir dados de receitas anteriores.
    
- Mantém separadas as informações clínicas e as informações de fabricação das lentes.
    
- Permite associar a receita ao cliente sem duplicar a referência da ótica.
    
- Impede a utilização de receitas de outros clientes ou unidades.
    
- Controla a validade das receitas utilizadas nas vendas.
    
- Permite reutilizar uma receita válida em diferentes vendas.
    
- Mantém a recomendação de retorno associada à prescrição que a originou.
    
- Evita redundância de informações no cadastro do cliente.
    
- Facilita a consulta cronológica do histórico clínico.
    

---

## 5. Possíveis Evoluções

Dependendo das necessidades futuras do sistema, a entidade poderá evoluir.

Exemplos:

- anexar uma imagem ou PDF da receita original;
    
- identificar de forma estruturada o tipo de profissional emissor;
    
- registar a clínica responsável pelo exame;
    
- controlar receitas digitais;
    
- assinatura eletrónica da receita;
    
- validações clínicas mais específicas para os valores informados;
    
- identificação do documento original da prescrição.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `Receita` representa uma prescrição associada ao histórico clínico de um cliente.

Ela mantém informações sobre o profissional responsável, data da prescrição, validade e os dados clínicos necessários para representar a condição visual registada naquele momento.

Cada receita pertence a um cliente e, através dele, ao contexto de uma ótica.

As informações relacionadas à fabricação personalizada da lente, como DP e DNP, permanecem separadas em `ItemLente`.

Durante uma venda, produtos do tipo `Lens` exigem uma receita válida pertencente ao mesmo cliente e à mesma ótica.

As receitas anteriores são preservadas e podem ser consultadas posteriormente, mantendo o histórico clínico separado do cadastro do cliente e das características específicas de cada lente fabricada.