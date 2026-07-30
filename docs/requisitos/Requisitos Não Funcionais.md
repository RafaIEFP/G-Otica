# Requisitos Não Funcionais

## Objetivo

Este documento reúne os principais requisitos não funcionais da aplicação.

Enquanto os requisitos funcionais descrevem as funcionalidades do sistema, os requisitos não funcionais definem as características de qualidade esperadas para a aplicação.

Esses requisitos orientam decisões relacionadas à segurança, desempenho, disponibilidade, usabilidade e manutenção do sistema.

---

# Segurança

O sistema deverá:

- autenticar todos os utilizadores antes do acesso às funcionalidades protegidas;
- armazenar palavras-passe utilizando algoritmos seguros de hash;
- permitir que cada utilizador visualize apenas as óticas às quais possui acesso;
- controlar as permissões de acordo com o papel exercido pelo utilizador em cada ótica.

---

# Integridade dos Dados

O sistema deverá:

- preservar o histórico das vendas, compras e pagamentos;
- manter consistência entre os relacionamentos do domínio;
- utilizar inativação sempre que possível em vez de exclusão definitiva dos registros.

---

# Desempenho

O sistema deverá:

- apresentar tempo de resposta adequado durante o atendimento ao cliente;
- suportar múltiplos utilizadores utilizando a aplicação simultaneamente sem comprometer a experiência de uso.

---

# Disponibilidade

O sistema deverá:

- permanecer disponível durante o horário de funcionamento da ótica;
- garantir que problemas em uma unidade não comprometam os dados das demais.

---

# Usabilidade

O sistema deverá:

- possuir uma interface simples e intuitiva;
- reduzir a quantidade de passos necessários para executar as principais operações;
- apresentar informações de forma clara durante o atendimento ao cliente.

---

# Escalabilidade

O sistema deverá:

- permitir o crescimento do número de óticas, clientes, produtos e vendas sem necessidade de alterações significativas na modelagem;
- possibilitar a inclusão de novas funcionalidades preservando a estrutura existente sempre que possível.

---

# Manutenibilidade

O sistema deverá:

- possuir código organizado e de fácil manutenção;
- facilitar futuras evoluções do domínio;
- manter a documentação atualizada conforme a evolução do projeto.

---

# Observações

- Os requisitos não funcionais representam objetivos de qualidade da aplicação.
    
- Eles complementam os requisitos funcionais e orientam decisões técnicas durante o desenvolvimento.
    
- Novos requisitos poderão ser adicionados conforme novas necessidades forem identificadas.