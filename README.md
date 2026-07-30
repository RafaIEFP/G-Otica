## Projeto
**Título**: G-Otica
**Autor**: Rafael Bezerra da Silva
**Data**: 29/07/2026

### Contexto do projeto
Este projeto tem como objetivo desenvolver um sistema de gestão para óticas, centralizando as principais operações do dia a dia em uma única plataforma.
 
A aplicação permitirá o gerenciamento de clientes, receitas oftalmológicas, produtos, vendas, compras, pagamentos e demais processos envolvidos na rotina de uma ótica. O sistema foi concebido para atender empresas que possuem uma ou mais unidades, possibilitando que proprietários e funcionários acessem apenas as informações pertinentes às suas funções e às óticas às quais estão vinculados.
 
A modelagem do domínio está sendo construída em conjunto com um especialista do ramo óptico, garantindo que as regras de negócio reflitam o funcionamento real de uma ótica. Durante o desenvolvimento, busca-se manter um equilíbrio entre simplicidade e escalabilidade, implementando inicialmente apenas as funcionalidades essenciais, mas preparando a arquitetura para futuras evoluções.
 
O principal objetivo do projeto é oferecer uma solução organizada, confiável e de fácil manutenção, capaz de apoiar a gestão operacional das óticas e servir como base para a expansão de funcionalidades conforme novas necessidades do negócio forem surgindo.

---

## Descrição do problema
O gerenciamento de uma ótica envolve uma série de processos específicos que exigem organização, precisão e controle adequado das informações. Diferentemente de outros tipos de comércio, uma ótica não trabalha apenas com produtos e vendas, mas também com informações técnicas relacionadas à saúde visual dos clientes, como receitas oftalmológicas, parâmetros de lentes, tratamentos, medidas ópticas e acompanhamento de serviços realizados. Além disso, existem processos administrativos essenciais, como controle de estoque, compras, fornecedores, vendas, pagamentos e acompanhamento da produção e entrega dos produtos.

Atualmente, muitos estabelecimentos utilizam sistemas de gestão genéricos ou soluções desenvolvidas especificamente para o setor óptico. Embora algumas dessas ferramentas ofereçam uma grande quantidade de funcionalidades, é comum que apresentem dificuldades relacionadas à experiência de utilização, principalmente devido à complexidade dos fluxos, excesso de informações exibidas ao usuário e processos pouco intuitivos para operações realizadas diariamente. Essa situação pode impactar diretamente a produtividade dos funcionários e vendedores, que precisam executar tarefas de maneira rápida durante o atendimento ao cliente, mas acabam enfrentando dificuldades para localizar informações ou concluir operações simples.

Por outro lado, existem soluções mais simples no mercado que possuem interfaces mais acessíveis e fáceis de compreender, porém geralmente apresentam limitações quando é necessário lidar com informações específicas do segmento óptico. Esses sistemas podem não oferecer recursos suficientes para o controle detalhado de receitas, características de lentes, tratamentos aplicados, parâmetros técnicos e demais informações necessárias para uma gestão completa da ótica. Como consequência, gestores e profissionais especializados podem perder capacidade de controle sobre processos importantes do negócio.

Esse cenário evidencia uma necessidade de equilíbrio entre facilidade de uso e profundidade funcional. Um sistema excessivamente simples pode não atender às necessidades técnicas de uma ótica, enquanto uma solução muito complexa pode dificultar a utilização por usuários que não precisam acessar todos os recursos disponíveis. A ausência desse equilíbrio pode gerar aumento no tempo de execução das atividades, dificuldades no treinamento de novos funcionários, registros incorretos de informações e menor eficiência operacional.

Dessa forma, identifica-se a necessidade de desenvolver uma aplicação de gestão voltada especificamente para o segmento óptico, capaz de oferecer uma interface simples e intuitiva para usuários que realizam operações rotineiras, sem comprometer o acesso a funcionalidades avançadas para profissionais que necessitam de maior controle sobre os detalhes técnicos e administrativos da ótica.

O objetivo é criar uma solução que permita a gestão completa das operações de uma ótica, proporcionando uma experiência de utilização eficiente para diferentes perfis de usuários, desde vendedores e funcionários responsáveis pelo atendimento até gestores e profissionais especializados. Assim, a aplicação busca unir usabilidade, organização e controle detalhado das informações, contribuindo para uma operação mais eficiente e reduzindo as dificuldades encontradas em soluções existentes.

---

## Ferramentas e versões
Todas as ferramentas terão compatibilidade com o a versão da **Stack**

##### Stack
- Backend: Asp.Net Core Web API 
- Frontend: Blazor WASM 
- Versões: .NET 10 

##### Blibliotecas
- Entity Framework
- Scanner
- BCrypt
- FluentValidation
- MudBlazor


##### Documentação da API
- Scalar 

##### Autenticação
- AspNetCore.Identity

Será criado um serviço externo para a autenticação utilizando o mecanismo do Identity, onde seus endpoints serão consumidos

---

## Análise de dados

Esta seção apresenta a modelagem do domínio da aplicação, 
incluindo o Diagrama Entidade-Relacionamento (DER) e a documentação das entidades identificadas durante a análise do negócio.

#### Diagrama completo:
- [G-Otica](./docs/DER/G-Otica.png)

#### Diagramas separados por contexto:
- [Gestores](./docs/DER/G-Otica%20(Gestores).png)
- [Compras e Estoque](./docs/DER/G-Otica%20(Compras%20e%20Estoque).png)
- [Catálogo e Produtos](./docs/DER/G-Otica%20(Catálogo%20e%20Produtos).png)
- [Vendas](./docs/DER/G-Otica%20(Vendas).png)

### Dominio
Sobre:
- [README](./docs/domain/README.md)

#### Entidades de dominio:
- [Otica](./docs/domain/Entidades%20de%20Domínio/Otica.md)

- [Utilizador](./docs/domain/Entidades%20de%20Domínio/Utilizador.md)

- [UtilizadorOtica](./docs/domain/Entidades%20de%20Domínio/UtilizadorOtica.md)

- [Cliente](./docs/domain/Entidades%20de%20Domínio/Cliente.md)

- [Receita](./docs/domain/Entidades%20de%20Domínio/Receita.md)

- [Produto](./docs/domain/Entidades%20de%20Domínio/Produto.md)

- [Venda](./docs/domain/Entidades%20de%20Domínio/Venda.md)

- [Compra](./docs/domain/Entidades%20de%20Domínio/Compra.md)

- [Fornecedor](./docs/domain/Entidades%20de%20Domínio/Fornecedor.md)

- [Pagamento](./docs/domain/Entidades%20de%20Domínio/Pagamento.md)

- [Tratamento](./docs/domain/Entidades%20de%20Domínio/Tratamento.md)

#### Entidades Especializadas
- [ItemVenda](./docs/domain/Entidades%20Especializadas/ItemVenda.md)

- [ItemCompra](./docs/domain/Entidades%20Especializadas/ItemCompra.md)

- [ItemLente](./docs/domain/Entidades%20Especializadas/ItemLente.md)

- [ItemLenteTratamento](./docs/domain/Entidades%20Especializadas/ItemLenteTratamento.md)

---

## Análise de processos
Esta seção descreve como as atividades da ótica acontecem na prática, representando os principais fluxos de negócio identificados durante o levantamento de requisitos.

Sobre:
- [README](./docs/processos/README.md)

Processos:
- [Atendimento](./docs/processos/Atendimento.md)
- [Venda](./docs/processos/Venda.md)
- [Pagamento](./docs/processos/Pagamento.md)
- [Compra](./docs/processos/Compra.md)
- [Receitas](./docs/processos/Receitas.md)

---

## Requisitos:

Esta seção reúne os requisitos identificados para a primeira versão do sistema, separando as funcionalidades esperadas dos atributos de qualidade da aplicação.

- [Requisitos Funcionais](./docs/requisitos/Requisitos%20Funcionais.md)
- [Requisitos Não Funcionais](./docs/requisitos/Requisitos%20Não%20Funcionais.md)
