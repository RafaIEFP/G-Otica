Esta pasta reúne a documentação técnica e de negócio do **G-Otica**.

O objetivo é centralizar as regras do domínio, processos de negócio, requisitos, diagramas e principais decisões tomadas durante o desenvolvimento, facilitando a compreensão e a evolução do sistema.

A documentação procura refletir o comportamento atual da aplicação. Funcionalidades ainda não implementadas são identificadas separadamente como possíveis evoluções.

---

## Domínio

A pasta [`domain`](./domain/) contém a documentação das entidades utilizadas pela aplicação.

Os documentos descrevem:

- contexto e responsabilidade das entidades;
- regras de domínio;
- decisões de modelagem;
- relacionamentos;
- possíveis evoluções.

A documentação está dividida entre **entidades de domínio** e **entidades especializadas**.

Entre os principais conceitos documentados encontram-se:

- óticas e utilizadores;
- clientes e receitas;
- produtos e tratamentos;
- fornecedores e compras;
- stock;
- vendas;
- lentes;
- pagamentos;
- autenticação e convites.

Para uma visão geral da organização das entidades, consulte o [`README` do domínio](./domain/README.md).

---

## Processos

A pasta [`processos`](./processos/) documenta como diferentes entidades e regras colaboram durante as principais operações do sistema.

Atualmente são documentados os seguintes processos:

- Gestão da Ótica;
- Gestão da Equipa e Convites;
- Gestão de Clientes;
- Gestão de Receitas;
- Gestão de Stock;
- Compra;
- Venda;
- Gestão de Pagamentos.

Enquanto a documentação de domínio explica cada conceito individualmente, os processos mostram **como esses conceitos interagem durante uma operação completa**.

---

## Requisitos

A documentação de requisitos descreve o comportamento esperado do sistema e as características que orientam a sua implementação.

Inclui:

### Requisitos Funcionais

Definem as funcionalidades disponibilizadas pelo G-Otica, como:

- autenticação;
- gestão de óticas e utilizadores;
- clientes e receitas;
- produtos e stock;
- fornecedores e compras;
- vendas;
- lentes e tratamentos;
- pagamentos.

### Requisitos Não Funcionais

Definem características relacionadas à qualidade e funcionamento da aplicação, como:

- segurança;
- autorização;
- isolamento dos dados entre óticas;
- integridade;
- consistência transacional;
- desempenho;
- tratamento de erros;
- manutenção e evolução.

---

## Diagramas

A pasta [`DER`](./DER/) contém os **Diagramas Entidade-Relacionamento** utilizados para representar a estrutura dos dados e os relacionamentos entre as entidades.

Os diagramas estão organizados por contexto para facilitar a visualização das diferentes áreas do sistema.

Entre os contextos representados encontram-se:

- Gestão de Utilizadores e Óticas;
- Clientes e Receitas;
- Compras e Stock;
- Vendas.

Também existe uma visão geral reunindo as entidades do sistema.

---

## Decisões Arquiteturais

A pasta [`decisions`](./decisions/) regista decisões consideradas relevantes para a estrutura e evolução do projeto.

Atualmente são documentadas:

- **Arquitetura em Camadas**;
- **Estratégia de Autenticação**;
- **Segregação de Repositórios**;
- **Separação entre Backend e Frontend**.

Esses documentos procuram explicar não apenas **qual solução foi utilizada**, mas também o problema que motivou a decisão, as alternativas consideradas e as consequências da escolha.

---

# Organização

De forma simplificada, a documentação está organizada da seguinte maneira:

```text
docs/
├── README.md
│
├── domain/
│   ├── README.md
│   └── ...
│
├── processos/
│   └── ...
│
├── requirements/
│   └── ...
│
├── DER/
│   └── ...
│
└── decisions/
    └── ...
```

---

# Como Navegar pela Documentação

Para compreender o projeto de forma progressiva, uma possível ordem de leitura é:

```text
Requisitos
    ↓
Domínio
    ↓
Processos
    ↓
Diagramas
    ↓
Decisões Arquiteturais
```

- **Requisitos** apresentam o que o sistema deve fazer.
- **Domínio** apresenta os conceitos e regras utilizados para representar o negócio.
- **Processos** mostram como esses conceitos colaboram durante as operações.
- **Diagramas** permitem visualizar os relacionamentos entre os dados.
- **Decisões Arquiteturais** explicam algumas das principais escolhas técnicas realizadas durante o desenvolvimento.

Não é necessário seguir essa ordem para consultas específicas.

---

# Manutenção da Documentação

A documentação deve acompanhar a evolução do projeto.

Sempre que uma alteração modificar significativamente:

- uma regra de domínio;
- uma entidade;
- um relacionamento;
- um processo de negócio;
- um requisito;
- uma decisão arquitetural;

o respetivo documento deverá ser revisto.

O objetivo é manter esta pasta como uma representação confiável do funcionamento atual do **G-Otica**, evitando que a documentação e a implementação evoluam de forma independente.