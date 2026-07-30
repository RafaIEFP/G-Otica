## 1. Contexto
A entidade `Fornecedor` representa as empresas responsáveis pelo fornecimento dos produtos comercializados pela ótica.

Seu objetivo é identificar a origem dos produtos adquiridos e manter o relacionamento entre as compras realizadas e seus respectivos fornecedores.

Na primeira versão do sistema, o fornecedor possui uma responsabilidade exclusivamente cadastral.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um fornecedor pode fornecer diversos produtos
Um mesmo fornecedor pode comercializar diferentes tipos de produtos.

Exemplos:
- lentes;
- armações;
- estojos;
- flanelas;
- acessórios.

---

#### Um fornecedor pode participar de várias compras
Ao longo do tempo, diversas compras podem ser realizadas para o mesmo fornecedor.

Essas compras compõem o histórico comercial da ótica.

---

#### Cada compra possui um único fornecedor
Uma compra é sempre realizada junto a um único fornecedor.

Caso seja necessário comprar produtos de empresas diferentes, serão registadas compras distintas.

---

## 3. Decisões de Modelagem

#### O fornecedor será mantido simples
Foi decidido que a entidade possuirá apenas informações básicas de identificação.

Exemplos:
- Nome;
- Telefone;
- Email;
- Estado do cadastro.

Essa estrutura atende às necessidades da primeira versão.

---

#### Não serão armazenadas informações comerciais
Dados como:
- condições de pagamento;
- prazos de entrega;
- contratos;
- representantes comerciais;
- tabelas de preços;
- limite de crédito;

não serão controlados pelo sistema nesta primeira versão.

Essas informações não são necessárias para os objetivos atuais do projeto.

---

#### O fornecedor representa apenas a origem da compra
A responsabilidade da entidade é identificar de quem a ótica adquiriu determinado produto.

Toda a lógica operacional da compra permanece concentrada na entidade `Compra`.

---

#### Não haverá gestão completa de fornecedores
Embora seja possível evoluir para um módulo completo de gestão de fornecedores, essa necessidade não foi identificada durante o levantamento dos requisitos.

Foi priorizada uma modelagem simples e suficiente para o domínio atual.

---

## 4. Benefícios
A modelagem adotada oferece diversas vantagens.

- Simplifica o cadastro de fornecedores.
- Permite identificar facilmente a origem das compras.
- Evita complexidade desnecessária.
- Mantém responsabilidades bem definidas.
- Facilita futuras expansões caso novas necessidades surjam.

---

## 5. Possíveis Evoluções
Dependendo da evolução do sistema, poderão ser adicionadas novas funcionalidades.

Exemplos:
- Pessoa de contacto.
- Morada.
- Número de identificação fiscal.
- Condições de pagamento.
- Prazo médio de entrega.
- Avaliação do fornecedor.
- Catálogo de produtos fornecidos.
- Histórico de negociações.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão

A entidade `Fornecedor` representa as empresas responsáveis pelo abastecimento da ótica.

Sua responsabilidade limita-se à identificação dos fornecedores utilizados nas compras, permitindo manter o histórico de aquisições sem introduzir complexidade desnecessária.

Essa decisão mantém a modelagem alinhada às necessidades da primeira versão, preservando a possibilidade de expansão futura caso o domínio evolua.