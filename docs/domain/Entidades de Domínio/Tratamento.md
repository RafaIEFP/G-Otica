## 1. Contexto

A entidade `Tratamento` representa as opções de tratamento que podem ser aplicadas às lentes durante a sua fabricação.

Esses tratamentos não fazem parte do cadastro do produto e também não pertencem à receita.

Eles representam características adicionais que podem ser selecionadas para uma lente específica durante o processo de venda.

Cada tratamento possui um cadastro próprio dentro da ótica, incluindo o seu nome, preço base e estado.

---

## 2. Regras de Domínio

Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um tratamento pertence a uma Ótica

Cada tratamento é cadastrado dentro do contexto de uma única ótica.

Essa associação é representada através de `OpticalStoreId`.

Dessa forma, cada unidade mantém o seu próprio catálogo de tratamentos.

Um tratamento cadastrado em uma ótica não fica automaticamente disponível para outras unidades.

---

#### O nome do tratamento deve ser único dentro da ótica

Não podem existir dois tratamentos com o mesmo nome dentro da mesma ótica.

A comparação é realizada sem diferenciar letras maiúsculas e minúsculas.

Por exemplo:

```text
Antirreflexo
ANTIRREFLEXO
antirreflexo
```

são considerados o mesmo nome.

Essa regra também considera tratamentos inativos, pois a desativação não elimina o respetivo cadastro.

---

#### O tratamento possui um preço base

Cada tratamento possui um `BasePrice`.

Esse valor representa o preço atual do tratamento no catálogo da ótica.

O preço deve ser maior ou igual a zero, permitindo inclusive tratamentos sem custo adicional.

---

#### Os tratamentos são selecionados durante a venda

A associação de tratamentos a uma lente ocorre durante o registo da venda.

O utilizador seleciona, entre os tratamentos ativos disponíveis na ótica, aqueles que devem ser aplicados à lente correspondente.

Os tratamentos não são adicionados diretamente ao produto nem à receita.

---

#### Uma lente pode possuir zero ou vários tratamentos

A aplicação de tratamentos não é obrigatória.

Uma lente pode ser vendida:

- sem tratamentos;
    
- com um único tratamento;
    
- com vários tratamentos.
    

Exemplos:

- Antirreflexo;
    
- Filtro azul;
    
- Fotocromático;
    
- Endurecimento;
    
- Proteção UV.
    

O mesmo tratamento não pode ser associado mais de uma vez à mesma lente.

---

#### Um tratamento pode ser utilizado em diversas lentes

O mesmo tratamento pode ser aplicado em diferentes lentes e em diferentes vendas ao longo do tempo.

Não existe vínculo exclusivo entre um tratamento e uma lente específica.

---

#### Apenas tratamentos ativos podem ser utilizados em novas vendas

O tratamento possui um estado definido por `IsActive`.

Durante o registo de uma venda, todos os tratamentos selecionados devem:

- existir;
    
- pertencer à mesma ótica da venda;
    
- estar ativos.
    

Um tratamento inativo permanece associado às vendas realizadas anteriormente, mas não pode ser selecionado em novas vendas enquanto estiver desativado.

---

#### O tratamento pode ser desativado e reativado

Um tratamento pode deixar de ser disponibilizado para novas vendas sem que o seu cadastro seja eliminado.

A desativação preserva as associações existentes com lentes vendidas anteriormente.

Caso volte a ser oferecido pela ótica, o tratamento pode ser reativado.

---

#### Os tratamentos não fazem parte da Receita

A `Receita` contém as informações relacionadas à prescrição visual do cliente.

Os tratamentos representam características adicionais escolhidas para a lente durante o processo de venda.

Por esse motivo, não existe relacionamento entre `Tratamento` e `Receita`.

---

#### Os tratamentos não pertencem ao cadastro do Produto

O cadastro de `Produto` representa o item comercializado pela ótica.

O tratamento é uma opção adicional aplicada a uma lente específica numa determinada venda.

Por esse motivo, não existe uma associação permanente entre `Produto` e `Tratamento`.

---

#### O preço dos tratamentos é acrescentado ao valor da lente vendida

Quando um tratamento é selecionado para uma lente, o seu preço é acrescentado ao valor do respetivo `ItemVenda`.

O cálculo considera:

```text
Valor do produto
- desconto do item
+ tratamentos selecionados
```

Os preços dos tratamentos também fazem parte do `TotalAmount` da venda.

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

#### O tratamento funciona como um catálogo da Ótica

Cada ótica pode cadastrar os tratamentos que disponibiliza aos seus clientes.

Atualmente, cada tratamento armazena:

- Nome;
    
- Preço base;
    
- Estado do cadastro;
    
- Ótica à qual pertence.
    

Durante a venda, o utilizador seleciona os tratamentos existentes nesse catálogo.

---

#### O relacionamento é muitos-para-muitos

Foi identificada a necessidade de uma lente possuir vários tratamentos.

Da mesma forma, um tratamento pode ser utilizado em inúmeras lentes.

Por esse motivo, foi criada a entidade intermediária `ItemLenteTratamento`.

A estrutura é:

```text
ItemLente
    ↓
ItemLenteTratamento
    ↓
Tratamento
```

---

#### ItemLenteTratamento preserva o preço praticado na venda

`Treatment.BasePrice` representa o preço atual do tratamento no catálogo.

Quando uma venda é registada, esse valor é copiado para:

```text
ItemLensTreatment.UnitPrice
```

Dessa forma:

```text
Treatment.BasePrice
→ preço atual do tratamento
```

enquanto:

```text
ItemLensTreatment.UnitPrice
→ preço histórico utilizado naquela venda
```

Uma alteração posterior no `BasePrice` do tratamento não modifica o valor das vendas já realizadas.

---

#### O preço do tratamento participa do total da venda

Os valores armazenados em `ItemLenteTratamento` são somados ao valor do respetivo `ItemVenda`.

Assim, os tratamentos fazem parte do cálculo financeiro da venda e não funcionam apenas como informações descritivas da lente.

---

#### O nome do tratamento permanece centralizado no cadastro

O relacionamento `ItemLenteTratamento` preserva atualmente o preço histórico utilizado na venda, mas não mantém uma cópia do nome do tratamento.

O nome continua sendo obtido através da entidade `Tratamento`.

Dessa forma, alterações posteriores no nome do tratamento não alteram o preço histórico armazenado, embora o sistema passe a apresentar o nome atual do tratamento nas consultas.

---

#### Os tratamentos podem ser atualizados sem alterar vendas anteriores

O nome e o preço base de um tratamento ativo podem ser atualizados.

A alteração do preço afeta apenas futuras vendas.

Vendas existentes continuam utilizando o `UnitPrice` armazenado em `ItemLenteTratamento`.

---

#### Não serão utilizados pacotes de tratamentos

Durante a validação do domínio foi identificado que os tratamentos são selecionados individualmente conforme a necessidade da lente.

Embora laboratórios possam comercializar combinações específicas, essa informação não faz parte do processo atual.

Por esse motivo, o sistema mantém tratamentos individuais e permite combiná-los livremente durante a venda.

---

## 4. Fluxo dos Tratamentos

De forma simplificada, o processo ocorre da seguinte maneira.

1. O cliente escolhe uma lente.
    
2. São selecionados zero ou mais tratamentos disponíveis.
    
3. O sistema verifica se os tratamentos existem, estão ativos e pertencem à ótica.
    
4. Cada tratamento é associado ao `ItemLente` através de `ItemLenteTratamento`.
    
5. O preço atual de cada tratamento é armazenado como `UnitPrice`.
    
6. Os valores dos tratamentos são adicionados ao total do `ItemVenda`.
    
7. As informações ficam associadas à lente personalizada daquela venda.
    

---

## 5. Benefícios

A modelagem adotada oferece diversas vantagens.

- Permite qualquer combinação de tratamentos.
    
- Permite lentes sem tratamentos adicionais.
    
- Evita criar diversas colunas booleanas.
    
- Facilita o cadastro de novos tratamentos.
    
- Mantém um catálogo próprio por ótica.
    
- Permite ativar e desativar tratamentos sem eliminar o histórico.
    
- Centraliza os nomes e preços atuais dos tratamentos.
    
- Preserva o preço histórico utilizado nas vendas.
    
- Permite alterar preços sem afetar operações anteriores.
    
- Mantém os tratamentos separados do cadastro do produto e da receita.
    
- Permite que os tratamentos participem diretamente do cálculo do valor da venda.
    

---

## 6. Possíveis Evoluções

Dependendo da evolução do sistema, poderão ser adicionadas novas funcionalidades.

Exemplos:

- categoria do tratamento;
    
- compatibilidade entre tratamentos;
    
- incompatibilidade entre determinadas combinações;
    
- ordem de aplicação;
    
- código utilizado pelo laboratório;
    
- descrição técnica;
    
- associação a fabricantes específicos;
    
- pacotes de tratamentos;
    
- histórico de alterações de preço;
    
- preservação do nome histórico do tratamento utilizado na venda.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 7. Conclusão

A entidade `Tratamento` representa as opções adicionais que podem ser aplicadas às lentes durante a sua fabricação.

Cada tratamento pertence a uma ótica e possui nome, preço base e estado próprio.

Durante uma venda, uma lente pode receber zero ou vários tratamentos ativos disponíveis naquela unidade.

A associação é realizada através de `ItemLenteTratamento`, que também preserva o preço utilizado naquele momento.

Dessa forma, `Treatment.BasePrice` representa o preço atual do catálogo, enquanto `ItemLensTreatment.UnitPrice` preserva o valor histórico da venda.

Essa modelagem permite manter um catálogo flexível e reutilizável de tratamentos, preservando os valores comerciais das operações realizadas e evitando acoplar características específicas das lentes ao cadastro de produtos ou às receitas.