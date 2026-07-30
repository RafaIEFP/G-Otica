## 1. Contexto
A entidade `ItemLenteTratamento` representa a associação entre uma lente e os tratamentos escolhidos durante a venda.

Ela implementa o relacionamento N:N entre `ItemLente` e `Tratamento`, permitindo que uma lente possua diversos tratamentos e que um mesmo tratamento seja utilizado em diferentes lentes.

---

## 2. Responsabilidade
A responsabilidade da entidade `ItemLenteTratamento` é associar um ou mais tratamentos a uma lente específica.

Ela não possui regras próprias de negócio nem informações adicionais, servindo apenas para representar essa associação.

---

## 3. Decisões de Modelagem

#### Foi criada para representar um relacionamento muitos-para-muitos
Durante a modelagem foi identificado que:

- uma lente pode possuir vários tratamentos;
- um tratamento pode ser utilizado em diversas lentes.

Por esse motivo, foi criada uma entidade intermediária para representar essa relação.

---

#### Não possui atributos adicionais
A entidade é composta apenas pelas chaves que identificam:

- a lente;
- o tratamento.

Isso é suficiente para representar o relacionamento identificado no domínio.

Caso surjam necessidades futuras, novos atributos poderão ser adicionados sem alterar a estrutura principal do sistema.

---

#### Evita duplicação de informações
Os tratamentos permanecem centralizados na entidade `Tratamento`.

A entidade `ItemLenteTratamento` apenas estabelece quais tratamentos foram escolhidos para cada lente, evitando duplicação de dados.

---

## 4. Benefícios
A modelagem adotada permite:

- associar qualquer quantidade de tratamentos a uma lente;
- reutilizar os mesmos tratamentos em diferentes vendas;
- evitar múltiplas colunas booleanas na entidade `ItemLente`;
- manter a modelagem alinhada à cardinalidade real do domínio.

---

## 5. Conclusão

A entidade `ItemLenteTratamento` representa a associação entre uma lente e os tratamentos selecionados para sua fabricação.

Sua responsabilidade é implementar o relacionamento entre `ItemLente` e `Tratamento`, mantendo a modelagem simples, flexível e aderente ao funcionamento do negócio.