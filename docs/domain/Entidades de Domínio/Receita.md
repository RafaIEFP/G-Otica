## 1. Contexto
A entidade `Receita` representa a prescrição médica apresentada pelo cliente após a realização de um exame de vista.

Ela armazena exclusivamente as informações clínicas necessárias para a produção das lentes, preservando o histórico visual do cliente ao longo do tempo.

Cada receita representa um exame específico e deve ser mantida mesmo quando uma nova receita for emitida posteriormente.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Cada exame gera uma nova receita
Sempre que o cliente realiza um novo exame de vista, uma nova receita é emitida.

Mesmo quando o grau permanece inalterado, uma nova receita passa a fazer parte do histórico clínico do cliente.

---

#### Um cliente pode possuir várias receitas
Ao longo da vida, um cliente poderá acumular diversas receitas.

Esse histórico permite acompanhar a evolução da visão do paciente.

---

#### A receita representa apenas informações médicas
A receita contém exclusivamente os dados fornecidos pelo profissional responsável pelo exame.

Ela não representa informações coletadas pela ótica durante o atendimento.

---

#### DP e DNP normalmente são obtidos pela ótica
Embora algumas receitas possam apresentar essas medidas, na prática elas costumam ser verificadas novamente pelo profissional responsável pelo atendimento.

Essas medidas fazem parte do processo de fabricação da lente e não da prescrição médica propriamente dita.

Por esse motivo, elas pertencem ao pedido da lente e não à receita.

---

#### Uma venda de lentes utiliza uma receita
Quando o cliente adquire lentes graduadas, a produção é realizada com base na receita selecionada durante a venda.

Essa receita passa a representar a prescrição utilizada naquele pedido.

---

#### Nem toda venda exige uma receita
Produtos como:
- armações;
- estojos;
- flanelas;
- acessórios;

podem ser vendidos sem qualquer receita médica.

---

## 3. Decisões de Modelagem

#### A Receita não será dividida em múltiplas tabelas
Durante a modelagem foi considerada a possibilidade de separar os dados da receita em diferentes entidades.

Essa ideia foi descartada.

Apesar da quantidade de atributos, todos pertencem ao mesmo conceito de negócio e são registrados simultaneamente durante o cadastro da receita.

Criar novas tabelas aumentaria a complexidade sem trazer benefícios reais para a primeira versão.

---

#### DP e DNP não pertencem à Receita
Foi decidido não armazenar DP e DNP nesta entidade.

Essas medidas são utilizadas na fabricação personalizada das lentes e frequentemente são obtidas novamente pela ótica.

Por esse motivo, elas passaram a fazer parte da entidade `ItemLente`.

---

#### A Venda referencia a Receita de forma opcional
A entidade `Venda` possui uma referência opcional para `Receita`.

Essa decisão permite atender tanto vendas de lentes graduadas quanto vendas de produtos que não dependem de prescrição médica.

---

#### A Receita é a fonte da verdade do histórico clínico
Informações relacionadas aos exames permanecem exclusivamente na entidade `Receita`.

Dados como:
- Data do último exame;
- Grau atual;
- Histórico de evolução visual;

podem ser obtidos consultando as receitas cadastradas para o cliente.

Essa decisão elimina redundâncias e mantém uma única fonte da verdade.

---

## 4. Benefícios
A modelagem adotada oferece diversas vantagens.

- Preserva todo o histórico clínico do cliente.
- Evita perda de informações entre exames.
- Mantém separadas as informações médicas e as informações de fabricação das lentes.
- Permite reutilizar uma receita em diferentes vendas, quando necessário.
- Evita redundância de dados.

---

## 5. Possíveis Evoluções
Dependendo das necessidades futuras do sistema, a entidade poderá evoluir.

Exemplos:
- Anexar uma imagem ou PDF da receita original.
- Identificar o tipo de profissional emissor da receita.
- Registrar a clínica responsável pelo exame.
- Controlar receitas digitais.
- Assinatura eletrónica da receita.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão

A entidade `Receita` representa a prescrição médica utilizada pela ótica durante o processo de venda de lentes graduadas.

Ela concentra exclusivamente as informações clínicas do exame, preservando o histórico visual do cliente e servindo como única fonte da verdade para os dados médicos.

A separação entre prescrição médica e informações utilizadas na fabricação das lentes permitiu uma modelagem mais fiel ao funcionamento real da ótica, mantendo cada entidade responsável por um único conceito do domínio.