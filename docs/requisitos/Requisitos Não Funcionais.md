## Objetivo

Este documento reúne os principais requisitos não funcionais do G-Otica.

Enquanto os requisitos funcionais descrevem as funcionalidades disponibilizadas pelo sistema, os requisitos não funcionais definem **características de qualidade, segurança, confiabilidade e manutenção esperadas da aplicação**.

Esses requisitos orientam decisões técnicas relacionadas à arquitetura, segurança, integridade dos dados, desempenho, disponibilidade e evolução do sistema.

---

# Segurança

O sistema deverá:

- exigir autenticação para o acesso às funcionalidades protegidas;
    
- armazenar palavras-passe através de algoritmos seguros de hash;
    
- nunca armazenar palavras-passe em texto simples;
    
- utilizar tokens de acesso com tempo de validade limitado;
    
- utilizar refresh tokens para renovação da autenticação;
    
- invalidar a sessão correspondente quando o utilizador realizar logout;
    
- impedir a utilização das funcionalidades protegidas por utilizadores inativos;
    
- controlar o acesso às óticas de acordo com as associações do utilizador;
    
- impedir que um utilizador aceda a dados de uma ótica à qual não pertence;
    
- aplicar autorizações específicas para operações reservadas ao proprietário;
    
- proteger tokens de convite contra utilização indevida;
    
- evitar a exposição de credenciais e dados de autenticação em respostas da API.
    

---

# Autorização e Isolamento entre Óticas

Os dados pertencentes a diferentes óticas deverão permanecer logicamente isolados.

O sistema deverá:

- utilizar a ótica como contexto das operações sempre que aplicável;
    
- validar a associação do utilizador com a ótica antes de permitir o acesso aos seus recursos;
    
- impedir que identificadores válidos de outra ótica sejam utilizados para aceder ou modificar dados;
    
- respeitar os papéis atribuídos através de `UserOpticalStore`;
    
- restringir operações administrativas ao papel apropriado.
    

O pertencimento de um recurso a uma ótica deverá ser validado no servidor e não depender apenas de dados enviados pelo cliente da API.

---

# Proteção de Dados

O sistema deverá proteger informações pessoais, comerciais e clínicas armazenadas na aplicação.

Deverá ser evitada a exposição desnecessária de:

- palavras-passe;
    
- tokens de autenticação;
    
- tokens de convite;
    
- informações pessoais de clientes;
    
- informações presentes em receitas.
    

O acesso aos dados deverá ser limitado aos utilizadores autorizados dentro do respetivo contexto de ótica.

---

# Integridade dos Dados

O sistema deverá:

- manter consistência entre os relacionamentos do domínio;
    
- utilizar restrições de banco de dados sempre que aplicável;
    
- preservar a unicidade de informações definidas pelo domínio;
    
- impedir que o stock de produtos se torne negativo;
    
- preservar os valores históricos utilizados em compras e vendas;
    
- preservar o histórico de receitas, compras, vendas, pagamentos e movimentações de stock;
    
- utilizar inativação em vez de exclusão definitiva quando os dados precisarem permanecer no histórico;
    
- impedir que operações parcialmente concluídas deixem o sistema em estado inconsistente.
    

---

# Atomicidade das Operações

Operações que alterem múltiplos elementos relacionados deverão ser executadas de forma atómica.

Isso inclui especialmente operações como:

- registo de compras e atualização do stock;
    
- registo de vendas e redução do stock;
    
- criação das movimentações de stock;
    
- criação dos pagamentos associados à venda;
    
- cancelamento de vendas e reposição do stock.
    

Caso uma etapa crítica falhe, as alterações realizadas pela mesma operação deverão ser revertidas.

Não deverá ser possível, por exemplo, que o stock seja alterado sem que a operação comercial correspondente seja corretamente registada.

---

# Preservação do Histórico

Informações históricas não deverão depender exclusivamente do estado atual das entidades de catálogo.

O sistema deverá preservar, quando necessário:

- preço de aquisição dos produtos;
    
- preço de venda dos produtos;
    
- preço dos tratamentos aplicados;
    
- estados financeiros dos pagamentos;
    
- movimentações realizadas no stock;
    
- receitas anteriormente utilizadas.
    

Alterações futuras nos preços atuais dos produtos ou tratamentos não deverão modificar os valores das operações já registadas.

---

# Validação dos Dados

Todos os dados recebidos pela aplicação deverão ser validados antes de serem utilizados em operações de negócio.

O sistema deverá:

- rejeitar valores obrigatórios ausentes;
    
- rejeitar formatos inválidos;
    
- validar identificadores recebidos;
    
- validar limites definidos pelas regras de negócio;
    
- retornar mensagens de erro suficientemente claras para permitir identificar o problema;
    
- impedir que dados inválidos sejam persistidos.
    

A validação realizada pelo cliente da aplicação não deverá substituir as validações realizadas no servidor.

---

# Desempenho

O sistema deverá apresentar desempenho adequado para utilização durante o atendimento ao cliente e demais operações da ótica.

Para reduzir o impacto do crescimento do volume de dados, o sistema deverá:

- utilizar paginação em consultas que possam retornar grandes quantidades de registos;
    
- evitar carregar dados desnecessários durante consultas;
    
- utilizar índices de banco de dados nas consultas mais relevantes quando aplicável;
    
- permitir a utilização simultânea por múltiplos utilizadores sem comprometer a consistência dos dados.
    

A primeira versão não estabelece um SLA específico de tempo máximo de resposta.

---

# Concorrência

O sistema deverá manter a consistência quando diferentes utilizadores executarem operações simultaneamente.

Operações dependentes do estado atual de uma entidade deverão validar esse estado antes de efetuar alterações.

Transições de estado de vendas não deverão ser realizadas com base em informações desatualizadas.

---

# Disponibilidade e Confiabilidade

O sistema deverá estar disponível durante os períodos em que a ótica necessite utilizar a aplicação.

Falhas temporárias não deverão resultar em gravações parciais ou perda de consistência dos dados.

A aplicação deverá permitir que falhas sejam tratadas de forma controlada, retornando uma resposta de erro em vez de deixar a operação em estado indefinido.

A primeira versão não define um nível formal de disponibilidade ou SLA.

---

# Tratamento de Erros

A API deverá apresentar comportamento consistente em caso de erro.

O sistema deverá:

- utilizar códigos HTTP adequados para representar o resultado das operações;
    
- apresentar erros de validação de forma estruturada;
    
- diferenciar situações como recurso inexistente, conflito, acesso não autorizado e dados inválidos;
    
- evitar a exposição de detalhes internos da aplicação ao cliente;
    
- tratar exceções de forma centralizada.
    

---

# Usabilidade da API

A API deverá:

- possuir rotas consistentes e previsíveis;
    
- utilizar nomes e estruturas coerentes entre os diferentes recursos;
    
- apresentar respostas de forma clara;
    
- documentar os endpoints disponíveis;
    
- facilitar a integração com o frontend da aplicação e outros clientes autorizados.
    

Quando uma interface gráfica for disponibilizada, deverá:

- apresentar informações de forma clara;
    
- reduzir passos desnecessários nas operações mais frequentes;
    
- apresentar mensagens de erro compreensíveis;
    
- manter consistência visual entre as diferentes funcionalidades.
    

---

# Escalabilidade

A estrutura da aplicação deverá permitir o crescimento do volume de:

- óticas;
    
- utilizadores;
    
- clientes;
    
- produtos;
    
- receitas;
    
- compras;
    
- vendas;
    
- pagamentos;
    
- movimentações de stock.
    

O crescimento desses dados não deverá exigir alterações fundamentais no modelo de domínio.

A arquitetura deverá permitir a inclusão de novos módulos e funcionalidades sem exigir reestruturação completa das funcionalidades existentes.

---

# Manutenibilidade

O sistema deverá possuir uma estrutura que facilite manutenção e evolução.

Para isso, deverá:

- manter responsabilidades separadas entre as diferentes camadas da aplicação;
    
- manter as regras de negócio fora da camada de apresentação;
    
- utilizar abstrações para reduzir o acoplamento entre componentes;
    
- centralizar configurações e dependências;
    
- permitir a evolução da infraestrutura sem alterar desnecessariamente as regras de negócio;
    
- utilizar migrations para controlar alterações na estrutura da base de dados;
    
- manter nomenclatura consistente no código;
    
- manter a documentação atualizada conforme a evolução do projeto.
    

---

# Persistência

As operações de persistência deverão manter os dados de forma consistente e durável.

O sistema deverá:

- utilizar uma base de dados relacional para informações estruturadas do domínio;
    
- utilizar chaves e relacionamentos para garantir integridade referencial;
    
- utilizar transações quando uma operação envolver múltiplas alterações dependentes;
    
- permitir que alterações no esquema sejam versionadas e reproduzidas nos diferentes ambientes da aplicação.
    

---

# Compatibilidade e Integração

O backend deverá disponibilizar as funcionalidades através de uma API HTTP, permitindo que a lógica da aplicação permaneça independente da implementação específica do frontend.

Essa separação deverá permitir:

- utilização por uma aplicação web;
    
- evolução independente do backend e frontend;
    
- futura integração com outros clientes ou serviços autorizados.
    

Os contratos expostos pela API deverão permanecer consistentes sempre que possível para evitar alterações desnecessárias nos clientes consumidores.

---

# Observações

- Os requisitos não funcionais representam objetivos de qualidade e restrições técnicas do sistema.
    
- Nem todos os requisitos possuem métricas quantitativas definidas na primeira versão.
    
- Sempre que requisitos de desempenho, disponibilidade ou capacidade se tornarem críticos, deverão ser estabelecidos valores mensuráveis.
    
- Estes requisitos complementam os requisitos funcionais e devem ser considerados durante a evolução do projeto.