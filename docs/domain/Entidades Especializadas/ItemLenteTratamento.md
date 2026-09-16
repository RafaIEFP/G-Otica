## 1. Contexto

A entidade `ItemLenteTratamento` representa a associação entre uma lente personalizada e os tratamentos escolhidos durante a venda.

Ela implementa o relacionamento N:N entre `ItemLente` e `Tratamento`, permitindo que uma lente possua vários tratamentos e que um mesmo tratamento seja utilizado em diferentes lentes.

Além de representar essa associação, a entidade preserva o preço do tratamento utilizado no momento da venda.

---

## 2. Responsabilidade

A responsabilidade da entidade `ItemLenteTratamento` é associar um tratamento a uma lente específica e armazenar o preço praticado naquela operação.

Atualmente são armazenados:

- `ItemLensId`;
    
- `TreatmentId`;
    
- `UnitPrice`.
    

Dessa forma, a entidade possui duas responsabilidades relacionadas à associação:

- identificar qual tratamento foi aplicado à lente;
    
- preservar o preço daquele tratamento no momento da venda.
    

---

## 3. Regras de Domínio

#### Uma lente pode possuir vários tratamentos

Um `ItemLente` pode estar associado a zero ou vários tratamentos.

Cada tratamento selecionado é representado por um registo de `ItemLenteTratamento`.

Exemplo:

```text
ItemLente
 ├── Antirreflexo
 ├── Filtro azul
 └── Proteção UV
```

---

#### Um tratamento pode ser utilizado em várias lentes

O mesmo `Tratamento` pode ser associado a diferentes lentes e vendas ao longo do tempo.

Não existe exclusividade entre um tratamento e uma determinada lente.

---

#### O mesmo tratamento não pode ser associado duas vezes à mesma lente

Cada combinação de:

```text
ItemLensId + TreatmentId
```

deve ser única.

Dessa forma, uma lente não pode possuir duas associações com o mesmo tratamento.

---

#### O preço do tratamento é preservado no momento da venda

Quando um tratamento é selecionado para uma lente, o seu preço atual:

```text
Treatment.BasePrice
```

é copiado para:

```text
ItemLensTreatment.UnitPrice
```

Esse valor representa o preço efetivamente utilizado naquela venda.

Alterações posteriores no preço base do tratamento não modificam vendas já registadas.

---

#### O preço do tratamento participa do valor do ItemVenda

Os valores armazenados em `ItemLenteTratamento.UnitPrice` são adicionados ao valor do respetivo `ItemVenda`.

De forma simplificada:

```text
Valor do produto
- desconto
+ tratamentos
=
ItemVenda.TotalAmount
```

Assim, `ItemLenteTratamento` não representa apenas uma característica técnica da lente, mas também participa do registo financeiro da operação.

---

## 4. Decisões de Modelagem

#### Foi criada para representar um relacionamento muitos-para-muitos

Durante a modelagem foi identificado que:

- uma lente pode possuir vários tratamentos;
    
- um tratamento pode ser utilizado em diversas lentes.
    

Por esse motivo, foi criada uma entidade intermediária para representar essa relação.

A estrutura é:

```text
ItemLente
    ↓
ItemLenteTratamento
    ↓
Tratamento
```

---

#### A entidade possui uma chave primária composta

`ItemLenteTratamento` não possui um identificador próprio como `Id`.

A sua chave primária é formada por:

```text
ItemLensId + TreatmentId
```

Essa combinação identifica naturalmente a associação e impede que o mesmo tratamento seja associado mais de uma vez à mesma lente.

---

#### A entidade possui o preço histórico do tratamento

Inicialmente, a associação poderia ser representada apenas pelas duas chaves estrangeiras.

Entretanto, o preço do tratamento pode mudar ao longo do tempo.

Por esse motivo, `UnitPrice` é armazenado diretamente em `ItemLenteTratamento`.

Assim:

```text
Treatment.BasePrice
→ preço atual do catálogo
```

enquanto:

```text
ItemLensTreatment.UnitPrice
→ preço utilizado naquela venda
```

Essa decisão preserva corretamente o valor comercial da operação.

---

#### As informações do Tratamento não são duplicadas

Dados como:

- nome do tratamento;
    
- estado do cadastro;
    
- preço base atual;
    
- ótica à qual pertence;
    

continuam centralizados na entidade `Tratamento`.

`ItemLenteTratamento` armazena apenas as informações que pertencem à associação.

Atualmente, o preço é preservado historicamente, mas o nome do tratamento não é copiado.

Por isso, ao consultar uma venda antiga, o sistema utiliza o nome atual existente em `Tratamento` juntamente com o `UnitPrice` histórico armazenado na associação.

---

#### ItemLenteTratamento não pertence diretamente à Ótica

A entidade não possui `OpticalStoreId`.

O contexto da ótica pode ser determinado através das relações existentes:

```text
ItemLenteTratamento
        ↓
ItemLente
        ↓
ItemVenda
        ↓
Venda
        ↓
Ótica
```

e também através do próprio `Tratamento`, que pertence a uma ótica.

Durante o registo da venda, somente tratamentos ativos pertencentes à mesma ótica podem ser associados à lente.

---

#### A associação é criada durante o registo da Venda

`ItemLenteTratamento` não possui um fluxo independente de criação.

As associações são criadas quando uma venda contendo lentes é registada.

Nesse momento:

1. os tratamentos selecionados são validados;
    
2. é criado o `ItemLente`;
    
3. cada tratamento selecionado gera um `ItemLenteTratamento`;
    
4. o preço atual do tratamento é armazenado em `UnitPrice`;
    
5. os valores são adicionados ao total do `ItemVenda`.
    

---

## 5. Benefícios

A modelagem adotada permite:

- associar vários tratamentos a uma lente;
    
- reutilizar os mesmos tratamentos em diferentes lentes e vendas;
    
- impedir a associação duplicada do mesmo tratamento à mesma lente;
    
- evitar múltiplas colunas booleanas em `ItemLente`;
    
- preservar o preço histórico utilizado na venda;
    
- permitir alterações futuras em `Treatment.BasePrice` sem afetar operações anteriores;
    
- manter os dados cadastrais centralizados em `Tratamento`;
    
- manter a modelagem alinhada à cardinalidade real do domínio.
    

---

## 6. Possíveis Evoluções

Caso novas necessidades surjam, a associação poderá armazenar outras informações específicas do momento da venda.

Exemplos:

- nome histórico do tratamento;
    
- desconto específico aplicado ao tratamento;
    
- observações relacionadas à aplicação;
    
- informação técnica fornecida ao laboratório;
    
- estado individual da aplicação do tratamento.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 7. Conclusão

A entidade `ItemLenteTratamento` representa a associação entre uma lente personalizada e um tratamento selecionado durante uma venda.

Ela implementa o relacionamento muitos-para-muitos entre `ItemLente` e `Tratamento` através da chave composta formada por `ItemLensId` e `TreatmentId`.

Além de identificar o tratamento utilizado, a entidade preserva em `UnitPrice` o preço praticado no momento da venda.

Dessa forma, alterações futuras no preço base do tratamento não afetam operações já realizadas.

Essa modelagem mantém o cadastro de tratamentos centralizado, permite combinações flexíveis e preserva corretamente as informações comerciais específicas de cada venda.