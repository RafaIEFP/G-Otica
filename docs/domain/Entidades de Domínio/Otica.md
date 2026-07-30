## 1. Contexto
A Ótica representa a unidade de negócio do sistema.

É dentro de uma ótica que todas as operações comerciais acontecem, como cadastro de clientes, realização de vendas, emissão de pedidos de lentes, controlo de produtos e compras junto aos fornecedores.

Embora o sistema seja utilizado por proprietários e funcionários, todas as informações de negócio pertencem à ótica e não aos utilizadores que as registaram.

Por esse motivo, a ótica é considerada uma das entidades centrais do domínio.

---

## 2. Regras de Domínio
Durante o levantamento de requisitos foram identificadas as seguintes regras do negócio.

#### Uma empresa pode possuir várias óticas
O sistema deve permitir que um mesmo proprietário administre diversas unidades.

Exemplo:
- Ótica Centro
- Ótica Shopping
- Ótica Norte

Cada uma representa uma unidade independente.

---

#### Cada ótica possui os seus próprios dados
Clientes, produtos, vendas, compras e demais registos pertencem à unidade onde foram cadastrados.

Não existe partilha automática de dados entre diferentes óticas.

Por exemplo, um cliente cadastrado na Ótica Centro não pertence automaticamente à Ótica Shopping.

---

#### Funcionários trabalham em uma ou mais óticas
Uma ótica pode possuir diversos colaboradores.

Da mesma forma, um colaborador pode trabalhar em mais de uma unidade.

Essa regra será representada através da entidade `UtilizadorOtica`.

---

#### A ótica é responsável pelas operações do negócio
Toda operação realizada pelo sistema acontece dentro do contexto de uma ótica.

Entre elas:
- cadastro de clientes;
- cadastro de produtos;
- vendas;
- compras;
- pagamentos;
- pedidos de lentes.

---

## 3. Decisões de Modelagem

#### A Ótica será uma entidade própria
Foi criada uma entidade específica para representar cada unidade da empresa.

Ela não será apenas um atributo em outra tabela.

Isso permite que cada unidade possua sua própria identidade e seus próprios dados.

---

#### As entidades de negócio referenciam a Ótica
As principais entidades do sistema armazenam uma referência para a ótica à qual pertencem.

Exemplos:

- Cliente
- Produto
- Venda
- Compra
- Pagamento
- Utilizador (através de `UtilizadorOtica`)

Essa abordagem garante que todas as informações permaneçam organizadas por unidade.

---

#### A relação entre Utilizador e Ótica será N:N
Não foi adicionada uma chave estrangeira `OticaId` diretamente em `Utilizador`.

Em vez disso, foi criada uma entidade intermediária chamada `UtilizadorOtica`.

Essa decisão permite representar corretamente cenários em que um mesmo utilizador atua em diferentes unidades.

A modelagem dessa relação é detalhada no documento [[UtilizadorOtica]].

---

#### A Ótica será o limite de pertencimento dos dados
Foi definido que os registos do sistema pertencem à ótica e não ao utilizador.

O utilizador representa apenas quem executou determinada operação.

Já a ótica representa quem é proprietária daquela informação.

---

## 4. Benefícios

A modelagem adotada oferece diversas vantagens.

- Permite que um mesmo sistema seja utilizado por empresas com uma ou várias unidades.
- Mantém os dados organizados por ótica.
- Facilita o controlo de permissões entre utilizadores e unidades.
- Evita duplicação de cadastros.
- Aproxima a modelagem da estrutura organizacional encontrada em empresas do ramo óptico.
- Facilita futuras implementações de relatórios por unidade.

---

## 5. Possíveis Evoluções
Embora a V1 mantenha um cadastro simples de óticas, a entidade poderá evoluir futuramente.

Exemplos:
- Endereço completo.
- Horário de funcionamento.
- Logotipo.
- Configurações específicas da unidade.
- Dados fiscais adicionais.
- Informações de contacto secundárias.
- Configurações de emissão de documentos.
- Preferências de notificações.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão
A Ótica representa a unidade organizacional do sistema e constitui um dos principais conceitos do domínio.

Todas as operações comerciais acontecem dentro do contexto de uma ótica, tornando essa entidade a responsável pelo pertencimento dos dados de negócio.

A adoção de uma entidade própria facilita o controlo de acesso e prepara a aplicação para empresas que administram uma ou diversas óticas, mantendo a simplicidade necessária para a primeira versão do projeto.