## 1. Contexto

A entidade `ItemLente` representa as informações necessárias para a fabricação personalizada de uma lente.

Ela complementa um `ItemVenda` quando o produto vendido corresponde a uma lente, armazenando as medidas e características específicas utilizadas durante o processo de produção.

A entidade não representa o produto existente no catálogo nem a prescrição clínica do cliente.

Ela representa especificamente como aquela lente deve ser produzida dentro de uma determinada venda.

---

## 2. Responsabilidade

A responsabilidade da entidade `ItemLente` é armazenar as informações técnicas utilizadas na fabricação personalizada de uma lente.

Atualmente são armazenados:

- Olho correspondente;
    
- DP;
    
- DNP;
    
- Tipo da lente;
    
- Índice de refração;
    
- Material;
    
- Cor;
    
- Diâmetro;
    
- Tratamentos associados.
    

Cada `ItemLente` pertence a um único `ItemVenda`.

---

## 3. Regras de Domínio

#### ItemLente existe apenas para produtos do tipo Lens

Um `ItemLente` somente pode ser criado quando o produto associado ao `ItemVenda` possui:

```text
ProductType.Lens
```

Produtos como:

- armações;
    
- estojos;
    
- flanelas;
    
- acessórios;
    

não podem possuir `ItemLente`.

---

#### Todo ItemVenda de uma lente deve possuir ItemLente

Quando o produto vendido é do tipo `Lens`, as informações de `ItemLente` tornam-se obrigatórias.

Dessa forma:

```text
Produto Lens
→ ItemVenda
   └── ItemLente obrigatório
```

enquanto:

```text
Produto não Lens
→ ItemVenda
   └── ItemLente não permitido
```

Essa regra é validada durante o registo da venda.

---

#### Cada ItemLente representa uma única lente

Um `ItemVenda` correspondente a uma lente deve possuir:

```text
Quantity = 1
```

Cada lente é representada individualmente porque pode possuir características diferentes.

Por exemplo:

```text
ItemVenda
├── Produto: Lente X
└── ItemLente: Olho direito

ItemVenda
├── Produto: Lente X
└── ItemLente: Olho esquerdo
```

Mesmo utilizando o mesmo produto do catálogo, as duas lentes permanecem individualizadas.

---

#### O olho correspondente é obrigatório

Cada lente deve identificar para qual olho será produzida.

Essa informação é representada por `EyeSide`.

Os valores atualmente existentes são:

```text
Right
Left
```

Isso permite diferenciar as características específicas de cada lente dentro da mesma venda.

---

#### DP e DNP são opcionais

As propriedades:

```text
PupillaryDistance
NasoPupillaryDistance
```

são opcionais.

Quando informadas, devem possuir valores maiores que zero.

Essas medidas pertencem ao `ItemLente` porque representam informações utilizadas na fabricação daquela lente específica.

---

#### O tipo da lente é obrigatório

Cada `ItemLente` possui um `LensType`.

Os tipos atualmente suportados são:

```text
SingleVision
Bifocal
Multifocal
IntermediateNear
```

O tipo descreve a configuração funcional da lente que será produzida.

---

#### O índice de refração deve ser maior que zero

`RefractiveIndex` representa o índice de refração utilizado na lente.

O valor é obrigatório e deve ser superior a zero.

---

#### O material da lente é obrigatório

Cada lente possui um `LensMaterial`.

Os materiais atualmente disponíveis são:

```text
Resin
Trivex
Polycarbonate
HighIndexResin
```

O material faz parte da configuração específica da lente produzida.

---

#### A cor é opcional

A propriedade `Color` pode ser utilizada quando a lente possuir uma cor ou característica semelhante que precise ser registada.

Essa informação é opcional e possui limite máximo de 100 caracteres.

---

#### O diâmetro é obrigatório

Cada lente possui um `Diameter`.

O valor deve ser superior a zero.

Essa informação representa uma das características necessárias para a preparação da lente.

---

#### Uma lente pode possuir zero ou vários tratamentos

A aplicação de tratamentos não é obrigatória.

Uma lente pode:

- não possuir tratamento;
    
- possuir um tratamento;
    
- possuir vários tratamentos.
    

Os tratamentos são associados através de `ItemLenteTratamento`.

O mesmo tratamento não pode ser associado mais de uma vez à mesma lente.

---

#### Os tratamentos devem pertencer à mesma Ótica e estar ativos

Durante o registo da venda, todos os tratamentos selecionados devem:

- existir;
    
- pertencer à ótica da venda;
    
- estar ativos.
    

O preço atual de cada tratamento é preservado na associação `ItemLenteTratamento`.

---

#### Uma venda com ItemLente exige uma Receita válida

Como `ItemLente` só pode existir para produtos do tipo `Lens`, uma venda que contenha essa entidade deve possuir uma `Receita`.

A receita deve:

- pertencer ao cliente da venda;
    
- pertencer ao contexto da mesma ótica;
    
- estar dentro do período de validade.
    

Entretanto, `ItemLente` não possui uma referência direta para `Receita`.

A relação ocorre através da própria `Venda`.

---

#### ItemLente identifica uma lente que necessita de produção

Na implementação atual, a existência de `ItemLente` também determina que a venda possui produtos personalizados que precisam passar pelo processo de produção.

Uma venda sem `ItemLente` possui o fluxo:

```text
Confirmed
    ↓
Delivered
```

Uma venda com pelo menos um `ItemLente` possui:

```text
Confirmed
    ↓
InProduction
    ↓
Ready
    ↓
Delivered
```

Assim, `ItemLente` não representa apenas dados técnicos, mas também identifica que aquele item necessita de fabricação personalizada.

---

#### Uma lente que já entrou em produção não retorna ao stock em caso de cancelamento

Quando uma venda ainda está em `Confirmed`, o produto correspondente à lente pode retornar ao stock caso a venda seja cancelada.

Entretanto, se a venda já estiver em:

```text
InProduction
```

ou:

```text
Ready
```

os itens que possuem `ItemLente` não são restaurados ao stock.

Isso ocorre porque representam lentes personalizadas cuja produção já foi iniciada.

---

## 4. Decisões de Modelagem

#### ItemLente especializa um ItemVenda

Foi criada uma entidade específica para armazenar informações exclusivas das lentes.

Essa abordagem evita adicionar diversos atributos ao `ItemVenda` que não possuem significado para produtos como armações, estojos ou acessórios.

A relação pode ser representada como:

```text
ItemVenda
    │
    └── ItemLente
```

Um `ItemVenda` pode possuir zero ou um `ItemLente`.

Cada `ItemLente`, por outro lado, pertence obrigatoriamente a um único `ItemVenda`.

---

#### ItemLente possui uma relação um-para-um com ItemVenda

A ligação é realizada através de:

```text
SaleItemId
```

Cada `ItemLente` corresponde exclusivamente a um único item da venda.

Não existe um `ItemLente` compartilhado entre vários itens.

---

#### DP e DNP pertencem ao ItemLente

Durante o levantamento do domínio foi identificado que essas medidas estão relacionadas à fabricação personalizada da lente.

Embora possam eventualmente aparecer em uma prescrição, no modelo atual elas pertencem ao pedido específico de fabricação.

Por esse motivo, permanecem em `ItemLente` e não em `Receita`.

---

#### ItemLente não armazena o nome do Cliente

A entidade não possui informações cadastrais do cliente.

O cliente já pode ser identificado através da estrutura:

```text
ItemLente
    ↓
ItemVenda
    ↓
Venda
    ↓
Cliente
```

Armazenar novamente o nome do cliente em `ItemLente` criaria redundância desnecessária.

---

#### ItemLente não armazena dados da Receita

Dados como:

- esfera;
    
- cilindro;
    
- eixo;
    
- acuidade visual;
    
- adição;
    

não são duplicados em `ItemLente`.

Essas informações permanecem na `Receita`.

A venda mantém a referência à receita utilizada, enquanto `ItemLente` contém apenas as características específicas da lente que será produzida.

Essa separação mantém:

```text
Receita
→ informação clínica
```

e:

```text
ItemLente
→ informação de fabricação
```

---

#### ItemLente representa a configuração da lente vendida

A entidade não descreve o produto genérico existente no catálogo.

O produto pode representar, por exemplo, uma determinada lente comercializada pela ótica.

Já `ItemLente` descreve como uma unidade desse produto será preparada para aquela venda.

Assim:

```text
Produto
→ o que a ótica comercializa
```

enquanto:

```text
ItemLente
→ como aquela lente será produzida
```

---

#### Os tratamentos foram separados em outra entidade

Uma lente pode possuir diversos tratamentos e o mesmo tratamento pode ser utilizado em várias lentes.

Por esse motivo, foi criada a entidade intermediária:

```text
ItemLenteTratamento
```

A estrutura é:

```text
ItemLente
    ↓
ItemLenteTratamento
    ↓
Tratamento
```

Essa associação também preserva o preço histórico do tratamento utilizado na venda.

---

#### Os tratamentos não são obrigatórios

A coleção de tratamentos pode permanecer vazia.

Isso permite representar uma lente que não necessita de nenhum tratamento adicional sem criar informações artificiais.

---

#### ItemLente participa indiretamente do cálculo financeiro da venda

A entidade não possui um valor financeiro próprio.

Entretanto, os tratamentos associados a ela possuem `UnitPrice`.

Esses valores são somados ao `TotalAmount` do respetivo `ItemVenda`.

Dessa forma, o próprio `ItemLente` permanece focado nas informações técnicas, enquanto os valores dos tratamentos são mantidos na entidade intermediária.

---

## 5. Benefícios

A modelagem adotada permite:

- manter o `ItemVenda` aplicável a qualquer tipo de produto;
    
- representar individualmente cada lente produzida;
    
- distinguir lentes dos olhos direito e esquerdo;
    
- armazenar características específicas de fabricação;
    
- manter DP e DNP separadas da prescrição clínica;
    
- suportar diferentes tipos e materiais de lente;
    
- permitir zero ou vários tratamentos;
    
- preservar o preço histórico dos tratamentos através de `ItemLenteTratamento`;
    
- identificar quais vendas necessitam passar pelo processo de produção;
    
- aplicar corretamente as regras de cancelamento para lentes já produzidas ou em produção;
    
- evitar duplicação de informações do cliente e da receita;
    
- facilitar futuras evoluções relacionadas à fabricação de lentes.
    

---

## 6. Possíveis Evoluções

Dependendo das necessidades futuras do sistema, poderão ser adicionadas novas informações relacionadas à fabricação.

Exemplos:

- laboratório responsável pela produção;
    
- código ou referência enviada ao laboratório;
    
- data de envio para produção;
    
- data prevista para conclusão;
    
- observações técnicas específicas;
    
- parâmetros adicionais de montagem;
    
- rastreamento individual da produção de cada lente;
    
- integração direta com sistemas de laboratórios.
    

Essas funcionalidades não fazem parte do modelo atual.

---

## 7. Conclusão

A entidade `ItemLente` representa a configuração técnica de uma lente personalizada dentro de uma venda.

Ela complementa exclusivamente um `ItemVenda` cujo produto seja do tipo `Lens`.

Cada lente é representada individualmente e identifica o olho correspondente, tipo da lente, índice de refração, material, diâmetro e, quando aplicável, DP, DNP, cor e tratamentos.

As informações clínicas permanecem em `Receita`, enquanto o produto continua representado por `Produto`.

Os tratamentos são associados através de `ItemLenteTratamento`, permitindo múltiplas opções e preservando o respetivo preço histórico.

Além de armazenar os dados técnicos da lente, a existência de `ItemLente` identifica que a venda necessita passar pelo processo de produção e influencia as regras de reposição de stock em caso de cancelamento.

Essa separação mantém distintas as informações comerciais, clínicas e de fabricação, tornando o domínio mais organizado e adequado ao processo de venda de lentes personalizadas.