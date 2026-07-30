## 1. Contexto
A entidade `Tratamento` representa as opções de tratamento que podem ser aplicadas às lentes durante sua fabricação.

Esses tratamentos não fazem parte do cadastro do produto e também não pertencem à receita médica.

Eles representam características adicionais escolhidas durante a venda, de acordo com as necessidades do cliente.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Os tratamentos são definidos durante a venda
A escolha dos tratamentos ocorre no momento do atendimento ao cliente.

O vendedor identifica as necessidades apresentadas e seleciona os tratamentos mais adequados para a lente.

---

#### Uma lente pode possuir vários tratamentos
Uma mesma lente pode receber diferentes tratamentos simultaneamente.

Exemplos:
- Antirreflexo;
- Filtro azul;
- Fotocromático;
- Endurecimento;
- Proteção UV.

Cada tratamento é independente dos demais.

---

#### Um tratamento pode ser utilizado em diversas lentes
O mesmo tratamento pode ser aplicado em diferentes lentes ao longo do tempo.

Não existe vínculo exclusivo entre tratamento e lente.

---

#### Os tratamentos não fazem parte da receita médica
A receita informa a prescrição utilizada para a fabricação da lente.

A escolha dos tratamentos é realizada posteriormente durante a venda.

---

#### Os tratamentos não pertencem ao cadastro do produto
O cadastro do produto representa apenas o item comercializado.

Os tratamentos representam características escolhidas para uma lente específica durante uma venda.

---

## 3. Decisões de Modelagem

#### Foi criada uma entidade específica para Tratamento
Inicialmente foi considerada a criação de atributos fixos em `ItemLente`, como:

- Filtro Azul;
- Antirreflexo;
- Fotocromático.

Essa abordagem foi descartada.

Criar uma entidade específica torna a modelagem mais flexível e evita alterações estruturais sempre que um novo tratamento precisar ser disponibilizado.

---

#### O relacionamento será muitos-para-muitos
Foi identificada a necessidade de uma lente possuir vários tratamentos.

Da mesma forma, um tratamento pode ser utilizado em inúmeras lentes.

Por esse motivo, foi criada a entidade intermediária `ItemLenteTratamento`.

---

#### Os tratamentos serão cadastrados pela ótica
Cada tratamento será previamente registado no sistema.

Durante a venda, o utilizador apenas selecionará os tratamentos desejados para a lente.

Essa abordagem evita duplicação de informação e padroniza os nomes utilizados.

---

#### Não serão utilizados pacotes de tratamentos
Durante a validação com o especialista foi identificado que os tratamentos são definidos individualmente conforme a necessidade do cliente.

Embora laboratórios possam comercializar combinações específicas, essa informação não faz parte do processo de atendimento realizado pela ótica.

Por esse motivo, o sistema armazenará apenas os tratamentos escolhidos para cada lente.

---

## 4. Fluxo dos Tratamentos

De forma simplificada, o processo ocorre da seguinte maneira.

1. O cliente escolhe a lente.
2. O vendedor identifica as necessidades do cliente.
3. São selecionados um ou mais tratamentos.
4. Os tratamentos são associados ao `ItemLente`.
5. Essas informações acompanham o pedido de fabricação da lente.

---

## 5. Benefícios
A modelagem adotada oferece diversas vantagens.

- Permite qualquer combinação de tratamentos.
- Evita criar diversas colunas booleanas.
- Facilita o cadastro de novos tratamentos.
- Mantém o domínio aderente ao funcionamento da ótica.
- Elimina duplicação de informações.
- Mantém o cadastro de tratamentos centralizado.

---

## 6. Possíveis Evoluções
Dependendo da evolução do sistema, poderão ser adicionadas novas funcionalidades.

Exemplos:
- Categoria do tratamento.
- Acréscimo automático no preço.
- Compatibilidade entre tratamentos.
- Ordem de aplicação.
- Código utilizado pelo laboratório.
- Descrição técnica.
- Associação a fabricantes específicos.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 7. Conclusão

A entidade `Tratamento` representa as características adicionais que podem ser aplicadas às lentes durante sua fabricação.

Sua responsabilidade é disponibilizar um catálogo reutilizável de tratamentos, permitindo que cada lente receba apenas as opções selecionadas durante a venda.

A utilização da entidade intermediária `ItemLenteTratamento` tornou a modelagem mais flexível e preparada para futuras evoluções, sem aumentar a complexidade da estrutura principal do sistema.