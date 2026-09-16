## 1. Contexto

A entidade `Cliente` representa a pessoa atendida pela ótica.

Ela é responsável por armazenar apenas as informações cadastrais necessárias para identificação e comunicação com o cliente.

Embora um cliente possa possuir um histórico de vendas e diversas receitas ao longo do tempo, essas informações pertencem a entidades específicas do sistema.

Dessa forma, a responsabilidade da entidade `Cliente` é representar exclusivamente a pessoa que mantém um relacionamento comercial com a ótica.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um cliente pode estar associado a várias vendas

Ao longo do tempo, um cliente pode adquirir diferentes produtos e serviços.

Exemplos:

- Óculos completos;
    
- Apenas lentes;
    
- Apenas armações;
    
- Estojos;
    
- Flanelas;
    
- Acessórios.
    

Essas aquisições são registadas através da entidade `Venda` e fazem parte do histórico comercial do cliente.

---

#### Um cliente pode possuir várias receitas

Um cliente pode possuir diversas receitas registadas em momentos diferentes.

Cada receita representa um registo independente e mantém as informações clínicas correspondentes àquele momento, permitindo preservar o histórico do cliente sem substituir receitas anteriores.

---

#### O histórico clínico pertence à Receita

As informações provenientes de uma prescrição oftalmológica não pertencem ao cadastro do cliente.

Cada receita representa um momento específico da condição visual do cliente e deve ser preservada para consultas futuras.

---

#### A recomendação para um novo exame pode variar

Não existe uma periodicidade única aplicável a todos os clientes.

Quando existir uma recomendação de retorno, essa informação pertence à receita correspondente e pode ser registada através da data de retorno recomendada.

---

#### O cliente pode ser desativado e reativado

O cliente possui um estado de cadastro que permite a sua desativação sem eliminar o respetivo histórico.

Um cliente desativado permanece registado no sistema, mas não pode ser utilizado em novas operações que exijam um cliente ativo, como o registo de novas receitas ou vendas.

Caso necessário, o cliente pode ser posteriormente reativado.

---

## 3. Decisões de Modelagem

#### O Cliente armazenará apenas informações cadastrais

Foi decidido manter a entidade simples, contendo apenas informações necessárias para identificação e contacto.

Atualmente são armazenados:

- Nome;
    
- Telefone;
    
- Email;
    
- Data de nascimento;
    
- Estado do cadastro;
    
- Ótica à qual o cliente pertence.
    

O email e a data de nascimento são opcionais.

A data de nascimento, quando informada, não pode ser uma data futura.

Informações clínicas permanecem em entidades específicas.

---

#### Não armazenar DataUltimoExame

Durante a modelagem foi considerada a criação desse atributo, mas essa abordagem foi descartada.

Como as receitas permanecem armazenadas individualmente, a data da receita mais recente pode ser obtida consultando o histórico de receitas do cliente.

Manter essa informação também no cliente criaria redundância.

---

#### Não armazenar ProximoExameRecomendado no Cliente

Também foi discutida a possibilidade de armazenar diretamente no cliente a próxima data recomendada para exame.

Essa informação não pertence ao cadastro do cliente, pois está relacionada à avaliação representada por uma receita específica.

Quando existir uma recomendação de retorno, ela é armazenada na própria entidade `Receita`.

Essa decisão mantém uma única fonte da verdade para o histórico clínico.

---

#### O Cliente não armazenará informações clínicas

Dados como:

- Esfera;
    
- Cilindro;
    
- Eixo;
    
- Acuidade visual;
    
- Adição;
    
- Histórico de evolução visual;
    

não pertencem ao cliente e são mantidos na entidade `Receita`.

Informações relacionadas à configuração de uma lente vendida, como DP e DNP, pertencem ao `ItemLente`, pois representam características da lente personalizada associada a um item de venda.

---

#### O Cliente pertence a uma Ótica

Na versão atual do sistema, cada cliente é associado a uma única ótica.

Essa associação é obrigatória e permite que as operações sobre clientes sejam sempre realizadas dentro do contexto da ótica correspondente.

Essa decisão simplifica a modelagem e atende ao cenário atualmente identificado.

Caso surja a necessidade de compartilhamento de clientes entre diferentes unidades, essa modelagem poderá evoluir futuramente.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Mantém responsabilidades bem definidas.
    
- Evita redundância de informações.
    
- Centraliza o histórico clínico na entidade `Receita`.
    
- Preserva o histórico comercial através das entidades de venda.
    
- Permite desativar clientes sem eliminar os seus dados históricos.
    
- Facilita futuras consultas ao histórico do cliente.
    
- Simplifica a manutenção da base de dados.
    
- Permite evolução do domínio sem sobrecarregar o cadastro do cliente.
    

---

## 5. Possíveis Evoluções

Dependendo das necessidades futuras do sistema, poderão ser adicionadas novas informações ao cadastro do cliente.

Exemplos:

- Endereço;
    
- Documento de identificação;
    
- Preferência de contacto;
    
- Consentimento para comunicações;
    
- Observações gerais;
    
- Contacto alternativo;
    
- Integração entre clientes de diferentes óticas pertencentes ao mesmo proprietário.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 6. Conclusão

A entidade `Cliente` representa exclusivamente a pessoa atendida pela ótica.

Sua responsabilidade limita-se ao armazenamento das informações cadastrais necessárias para identificação e comunicação, enquanto os dados clínicos permanecem centralizados na entidade `Receita` e os dados específicos de lentes personalizadas permanecem associados ao processo de venda.

O cliente também mantém um estado ativo ou inativo, permitindo preservar o seu histórico sem necessidade de exclusão definitiva.

Essa separação mantém a modelagem simples, evita redundâncias e estabelece responsabilidades claras entre as entidades do domínio.