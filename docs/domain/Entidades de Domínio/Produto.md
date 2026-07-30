## 1. Contexto
A entidade `Produto` representa os itens comercializados pela ótica.

Ela é utilizada tanto no processo de venda para os clientes quanto no processo de compra junto aos fornecedores.

Seu objetivo é manter o catálogo de produtos disponíveis para comercialização, independentemente da forma como esses produtos serão utilizados durante uma venda.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um produto pode ser vendido diversas vezes
Um mesmo produto pode participar de inúmeras vendas ao longo do tempo.

Cada venda registra apenas uma referência ao produto comercializado.

---

#### Um produto pode ser comprado diversas vezes
Além das vendas, um produto também pode aparecer em diversos pedidos de compra realizados aos fornecedores para reposição de estoque.

---

#### Existem diferentes tipos de produtos
A ótica comercializa diferentes categorias de produtos.

Exemplos:
- Lentes;
- Armações;
- Estojos;
- Flanelas;
- Acessórios.

Embora possuam características distintas, todos são tratados como produtos dentro do sistema.

---

#### Uma lente pode exigir informações adicionais
Quando o produto vendido é uma lente graduada, são necessárias informações específicas para sua fabricação.

Esses dados não pertencem ao produto em si, mas ao item da venda.

Por esse motivo, essas informações são armazenadas na entidade `ItemLente`.

---

#### O código do produto depende do fabricante
Cada produto pode possuir um código próprio definido pelo fabricante.

Esse código pode representar diferentes conceitos conforme o tipo do produto.

Exemplos:
- código da lente;
- referência da armação;
- código de catálogo.

Por esse motivo, o sistema trata essa informação de forma genérica através do atributo `CodigoProduto`.

---

## 3. Decisões de Modelagem

#### Foi criada uma única entidade para representar os produtos
Inicialmente foi considerada a criação de entidades específicas para lentes, armações e demais itens.

Essa abordagem foi descartada.

Todos os itens comercializados compartilham o mesmo ciclo de compra, venda e controlo de stock, tornando desnecessária sua separação na primeira versão.

---

#### O tipo do produto será identificado por um enum
Foi adotado o atributo `TipoProduto`.

Essa abordagem simplifica a modelagem e permite identificar rapidamente o comportamento esperado para cada produto.

Exemplos:
- Lente;
- Armação;
- Estojo;
- Flanela;
- Acessório.

---

#### O Produto não armazenará características da fabricação
Embora uma lente possua informações como:
- DP;
- DNP;
- Índice;
- Material;
- Tratamentos;
- Cor;

esses dados não fazem parte do cadastro do produto.

Eles pertencem exclusivamente ao pedido realizado para um cliente específico.

Essa decisão levou à criação da entidade `ItemLente`.

---

#### Não serão criadas entidades específicas para cada tipo de produto
Durante a modelagem também foi considerada a criação de entidades como:

- Lente;
- Armação;
- CategoriaProduto.

Essas estruturas foram descartadas por aumentarem a complexidade da modelagem sem atender a uma necessidade real da primeira versão.

Caso o domínio evolua, essa decisão poderá ser revisitada.

---

#### Algumas informações permanecerão simples na primeira versão
Também foram discutidas possíveis entidades para representar:

- Material;
- Cor;
- Categoria.

Após análise, foi decidido manter essas informações como enums ou valores simples, priorizando a simplicidade da primeira versão.

---

## 4. Benefícios
A modelagem adotada oferece diversas vantagens.

- Simplifica o cadastro de produtos.
- Evita excesso de entidades.
- Permite utilizar o mesmo catálogo em compras e vendas.
- Mantém separadas as informações do produto e da fabricação das lentes.
- Facilita futuras evoluções da modelagem.
- Reduz a complexidade da primeira versão.

---

## 5. Possíveis Evoluções
Dependendo da evolução do sistema, a entidade poderá ser expandida.

Exemplos:
- Categoria de produtos.
- Fabricante.
- Marca.
- Material como entidade própria.
- Cor como entidade própria.
- Código de barras.
- Imagens do produto.
- Controlo por lotes.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão

A entidade `Produto` representa todos os itens comercializados pela ótica, independentemente de sua categoria.

Sua responsabilidade limita-se ao cadastro e identificação dos produtos disponíveis para compra e venda, enquanto informações específicas da fabricação de lentes permanecem na entidade `ItemLente`.

Essa separação mantém a modelagem simples, evita duplicação de informações e permite que o sistema evolua gradualmente conforme novas necessidades do domínio sejam identificadas.