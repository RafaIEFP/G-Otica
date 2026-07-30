# Documentação do Domínio

Esta pasta reúne a documentação do domínio da aplicação.

O objetivo é registrar o conhecimento adquirido durante a modelagem do sistema, transformando as regras de negócio validadas com o especialista da ótica em documentação permanente.

Além de descrever as entidades do sistema, esta documentação explica as decisões tomadas durante sua modelagem, permitindo compreender **não apenas o que foi modelado, mas também por que cada decisão foi tomada**.

---

# Organização da documentação

A documentação está dividida em dois grupos.

## Entidades de Domínio

Representam os principais conceitos do negócio.

São entidades que possuem identidade própria dentro do domínio e podem ser compreendidas de forma independente.

Exemplos:

- Ótica
    
- Utilizador
    
- Cliente
    
- Receita
    
- Produto
    
- Venda
    
- Compra
    
- Pagamento
    
- Fornecedor
    
- Tratamento
    

Esses documentos descrevem as regras de negócio, as decisões de modelagem e as possíveis evoluções da entidade.

---

## Entidades Especializadas

Representam especializações ou complementos de outras entidades do domínio.

Essas entidades não existem de forma independente, estando sempre associadas a uma entidade principal.

Exemplos:

- ItemVenda
    
- ItemCompra
    
- ItemLente
    
- ItemLenteTratamento
    

Por esse motivo, sua documentação é mais objetiva, concentrando-se apenas em sua responsabilidade específica e nas decisões particulares da modelagem, 
evitando repetir informações já descritas nos documentos das entidades principais.

---

# Estrutura dos documentos

Os documentos seguem uma estrutura comum, adaptada conforme a responsabilidade de cada entidade.

## 1. Contexto

Apresenta o papel da entidade dentro do domínio e sua responsabilidade no sistema.

---

## 2. Regras de Domínio _(quando aplicável)_

Registra exclusivamente regras do negócio validadas com o especialista.

Essas regras representam o funcionamento real da ótica e independem da tecnologia utilizada.

Entidades especializadas podem não possuir esta seção quando todas as regras já estiverem documentadas na entidade principal.

---

## 3. Decisões de Modelagem

Descreve como as regras de domínio foram representadas no sistema.

Aqui são documentadas decisões relacionadas à modelagem das entidades, relacionamentos e demais escolhas arquiteturais.

---

## 4. Benefícios

Apresenta as vantagens obtidas com a modelagem escolhida.

---

## 5. Possíveis Evoluções _(quando aplicável)_

Registra funcionalidades e ideias discutidas durante a modelagem que não fazem parte da primeira versão, mas que poderão ser incorporadas futuramente.

Essa seção evita a perda de conhecimento e reduz a necessidade de revisitar decisões já analisadas.

---

## 6. Conclusão

Resume a responsabilidade da entidade e os principais motivos que justificam sua modelagem.

---

# Princípios adotados

Durante a elaboração desta documentação foram seguidos os seguintes princípios.

- Separar regras de domínio de decisões de modelagem.
    
- Modelar conceitos do negócio, e não apenas estruturas de banco de dados.
    
- Manter cada entidade responsável por um único conceito do domínio.
    
- Especializar entidades apenas quando necessário.
    
- Evitar redundância entre os documentos.
    
- Modelar apenas necessidades reais da primeira versão.
    
- Registrar decisões importantes para facilitar futuras evoluções.
    

Esses princípios orientam toda a documentação do domínio e servem como referência para a criação de novos documentos e para a evolução do sistema.