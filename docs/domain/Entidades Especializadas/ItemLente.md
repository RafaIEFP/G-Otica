## 1. Contexto
A entidade `ItemLente` representa as informações necessárias para a fabricação de uma lente graduada.

Ela complementa um `ItemVenda` quando o produto vendido corresponde a uma lente, armazenando todas as medidas e características específicas utilizadas pelo laboratório durante o processo de produção.

Essa entidade existe apenas para itens de venda que representam lentes graduadas.

---

## 2. Responsabilidade
A responsabilidade da entidade `ItemLente` é armazenar as informações técnicas utilizadas na fabricação personalizada de uma lente.

Essas informações incluem:
- DP;
- DNP;
- Tipo da lente;
- Índice;
- Material;
- Cor;
- Diâmetro;
- Nome do cliente (quando necessário).

Além disso, a entidade pode possuir um ou mais tratamentos associados através de `ItemLenteTratamento`.

---

## 3. Decisões de Modelagem

#### ItemLente especializa um ItemVenda
Foi criada uma entidade específica para armazenar informações exclusivas das lentes graduadas.

Essa abordagem evita adicionar diversos atributos ao `ItemVenda`, que devem existir apenas quando o produto vendido é uma lente.

---

#### DP e DNP pertencem ao ItemLente
Durante o levantamento do domínio foi identificado que essas medidas normalmente são obtidas ou conferidas pela própria ótica durante o atendimento.

Embora possam aparecer na receita, elas são utilizadas efetivamente na fabricação da lente.

Por esse motivo, essas informações pertencem ao `ItemLente` e não à entidade `Receita`.

---

#### ItemLente representa a fabricação da lente
A entidade não descreve o produto comercializado nem a prescrição médica.

Ela representa exclusivamente como aquela lente deverá ser produzida para um cliente específico.

---

#### Os tratamentos foram separados em outra entidade
Uma lente pode possuir diversos tratamentos.

Por esse motivo, foi criada a entidade `ItemLenteTratamento`, evitando limitar a quantidade de tratamentos ou criar diversos atributos booleanos.

---

#### ItemLente existe apenas quando necessário
Nem todo `ItemVenda` possui um `ItemLente`.

Essa entidade é criada apenas quando o produto vendido corresponde a uma lente graduada.

Itens como armações, estojos e acessórios não necessitam dessas informações.

---

## 4. Benefícios
A modelagem adotada permite:

- manter o `ItemVenda` simples;
- representar corretamente a fabricação personalizada das lentes;
- separar informações médicas das informações de produção;
- suportar diferentes tipos de lentes;
- facilitar futuras evoluções da fabricação sem impactar outras entidades.

---

## 5. Conclusão

A entidade `ItemLente` representa as informações técnicas necessárias para fabricar uma lente graduada para um cliente específico.

Sua responsabilidade é complementar o `ItemVenda` apenas quando necessário, mantendo separadas as informações comerciais, médicas e de fabricação.

Essa modelagem tornou o domínio mais organizado, flexível e fiel ao funcionamento real da ótica.