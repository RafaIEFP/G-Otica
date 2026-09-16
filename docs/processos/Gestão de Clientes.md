## Objetivo

Descrever o processo de registo e gestão dos clientes de uma ótica, incluindo:

- registo;
    
- consulta;
    
- atualização;
    
- desativação;
    
- reativação.
    

O cliente funciona como uma entidade central para outros processos do sistema, principalmente:

- registo de receitas;
    
- realização de vendas.
    

As receitas e vendas possuem processos próprios e não são detalhadas neste documento.

---

## Participantes

- Utilizador da ótica;
    
- Cliente;
    
- Sistema.
    

---

# 1. Acesso à Gestão de Clientes

As operações de gestão de clientes são realizadas dentro do contexto de uma ótica específica.

Para aceder a essas funcionalidades:

- o utilizador deve estar autenticado;
    
- deve possuir acesso ativo à ótica.
    

As operações não estão limitadas ao `owner`.

Um membro autorizado da ótica pode gerir os clientes da unidade.

---

# 2. Registo de Cliente

Um cliente é registado diretamente dentro de uma ótica.

Cada cliente pertence exclusivamente à unidade em que foi criado.

---

## Dados do Cliente

Durante o registo são informados:

- nome;
    
- número de telefone;
    
- email, quando disponível;
    
- data de nascimento, quando disponível.
    

O sistema associa automaticamente:

```text
OpticalStoreId
```

com base na ótica em que a operação está sendo realizada.

---

## Dados Obrigatórios

São obrigatórios:

- nome;
    
- número de telefone.
    

---

## Dados Opcionais

Podem permanecer vazios:

- email;
    
- data de nascimento.
    

Isso permite registar clientes mesmo quando essas informações não forem disponibilizadas.

---

## Validações

Antes de criar o cliente, o sistema deverá validar os dados informados.

### Nome

O nome não pode estar vazio.

---

### Telefone

O número de telefone:

- é obrigatório;
    
- deve possuir um formato válido.
    

---

### Email

Caso seja informado, o email deve possuir um formato válido.

Se não for informado, permanece nulo.

---

### Data de Nascimento

Caso seja informada, a data de nascimento não pode estar no futuro.

Exemplo:

```text
Data atual:       16/09/2026
Data nascimento: 20/09/2026

Resultado:
Operação rejeitada
```

---

## Fluxo Principal

1. O utilizador inicia o registo de um cliente.
    
2. Informa os dados cadastrais.
    
3. O sistema normaliza os dados recebidos.
    
4. O sistema valida:
    
    - nome;
        
    - telefone;
        
    - email, quando informado;
        
    - data de nascimento, quando informada.
        
5. É criado um novo `Client`.
    
6. O cliente é associado à ótica atual.
    
7. O cliente é criado inicialmente com:
    

```text
IsActive = true
```

8. Os dados são persistidos.
    
9. O cliente fica disponível para utilização nos demais processos da ótica.
    

---

## Resultado

Após o registo:

```text
OpticalStore
     │
     │
     ↓
   Client
IsActive = true
```

O cliente passa a poder ser utilizado em operações como:

```text
Prescription
Sale
```

---

# 3. Consulta de Cliente

Um utilizador com acesso à ótica pode consultar um cliente específico.

A consulta é realizada utilizando:

```text
OpticalStoreId
+
ClientId
```

Isso garante que um cliente pertencente a outra ótica não seja obtido através desse fluxo.

---

## Clientes Inativos

A consulta individual não depende de o cliente estar ativo.

Isso permite visualizar clientes anteriormente desativados e preservar o acesso às suas informações históricas.

---

# 4. Consulta da Lista de Clientes

O sistema permite consultar os clientes pertencentes à ótica.

A listagem é paginada.

Os parâmetros atualmente utilizados incluem:

```text
Page
PageSize
IsActive?
```

Os valores padrão são:

```text
Page = 1
PageSize = 20
```

O tamanho da página deve estar entre:

```text
1 e 100
```

---

## Filtro por Estado

`IsActive` é opcional.

Assim, a consulta pode solicitar:

```text
IsActive = true
→ apenas clientes ativos
```

```text
IsActive = false
→ apenas clientes inativos
```

ou:

```text
IsActive não informado
→ clientes ativos e inativos
```

---

## Ordenação

Os clientes são apresentados por nome.

De forma simplificada:

```text
Ana
Bruno
Carlos
Rafael
```

---

## Informações da Paginação

A resposta inclui informações como:

- página atual;
    
- tamanho da página;
    
- quantidade total de registos;
    
- quantidade total de páginas.
    

---

# 5. Atualização do Cliente

Os dados cadastrais de um cliente ativo podem ser atualizados.

---

## Pré-condições

Para atualizar:

- o utilizador deve possuir acesso à ótica;
    
- o cliente deve existir;
    
- o cliente deve pertencer à mesma ótica;
    
- o cliente deve estar ativo.
    

---

## Dados Atualizáveis

Atualmente podem ser alterados:

- nome;
    
- telefone;
    
- email;
    
- data de nascimento.
    

O cliente não pode ser transferido para outra ótica através dessa operação.

---

## Validações

As mesmas regras utilizadas durante o registo são aplicadas à atualização:

- nome obrigatório;
    
- telefone válido;
    
- email válido quando informado;
    
- data de nascimento não pode estar no futuro.
    

---

## Fluxo Principal

1. O utilizador seleciona o cliente.
    
2. Informa os novos dados.
    
3. O sistema normaliza as informações.
    
4. O sistema valida os dados.
    
5. O sistema verifica se o cliente está ativo e pertence à ótica.
    
6. Os dados são atualizados.
    
7. As alterações são persistidas.
    

---

## Cliente Inativo

Um cliente inativo não pode ser atualizado através do fluxo comum.

Caso seja necessário voltar a utilizá-lo, deverá primeiro ser reativado.

---

# 6. Desativação do Cliente

Clientes não são eliminados quando deixam de ser utilizados pela ótica.

O sistema utiliza desativação lógica:

```text
Client.IsActive = false
```

---

## Pré-condições

Para desativar:

- o cliente deve existir;
    
- deve pertencer à ótica;
    
- deve estar ativo.
    

---

## Fluxo Principal

1. O utilizador seleciona o cliente.
    
2. Solicita a desativação.
    
3. O sistema verifica se o cliente pertence à ótica e está ativo.
    
4. O estado é alterado de:
    

```text
IsActive = true
```

para:

```text
IsActive = false
```

5. O cliente permanece armazenado no sistema.
    

---

# 7. Efeitos da Desativação

A desativação não remove dados relacionados ao cliente.

Permanecem preservados:

- receitas anteriores;
    
- vendas anteriores;
    
- demais informações históricas associadas.
    

Entretanto, um cliente inativo não pode ser utilizado em novas operações que exijam um cliente ativo.

Isso inclui principalmente:

```text
Nova Prescription
Nova Sale
```

---

## Exemplo

```text
Cliente
IsActive = false

Histórico:
├── Receita A
├── Receita B
├── Venda 1
└── Venda 2
```

Esses registos continuam disponíveis.

Porém:

```text
Nova receita
→ não permitido

Nova venda
→ não permitido
```

---

# 8. Reativação do Cliente

Um cliente anteriormente desativado pode voltar a ficar disponível para utilização.

Não é criado um novo registo.

O mesmo `Client` é reativado.

---

## Pré-condições

Para reativar:

- o cliente deve existir;
    
- deve pertencer à ótica;
    
- deve estar inativo.
    

---

## Fluxo Principal

1. O utilizador seleciona um cliente inativo.
    
2. Solicita a reativação.
    
3. O sistema localiza o cliente.
    
4. Altera:
    

```text
IsActive = false
```

para:

```text
IsActive = true
```

5. O cliente volta a ficar disponível para novas operações.
    

---

## Resultado

Após a reativação:

```text
Client.IsActive = true
```

O cliente poderá novamente ser utilizado em:

```text
Prescription
Sale
```

---

# 9. Relação com Receitas

Um cliente pode possuir várias receitas ao longo do tempo.

A relação é:

```text
Client
   │
   │ 1:N
   ↓
Prescription
```

Para registar uma nova receita:

- o cliente deve existir;
    
- pertencer à ótica;
    
- estar ativo.
    

A desativação do cliente não elimina as receitas existentes.

O processo completo é descrito separadamente em **Processo de Receitas**.

---

# 10. Relação com Vendas

Uma venda é sempre realizada para um cliente.

A relação é:

```text
Client
   │
   │ 1:N
   ↓
Sale
```

Para iniciar uma nova venda, o cliente deve:

- existir;
    
- pertencer à mesma ótica;
    
- estar ativo.
    

A desativação posterior não altera vendas já realizadas.

O processo completo encontra-se documentado em **Processo de Venda**.

---

# 11. Isolamento entre Óticas

Um cliente pertence a apenas uma ótica.

Por exemplo:

```text
Ótica A
   └── Cliente X
```

O mesmo registo não pode ser utilizado diretamente pela:

```text
Ótica B
```

As operações utilizam simultaneamente:

```text
ClientId
+
OpticalStoreId
```

para garantir o contexto correto.

Assim, mesmo que um utilizador conheça o identificador de um cliente pertencente a outra ótica, esse registo não deverá ser tratado como pertencente à unidade atual.

---

# 12. Fluxos Alternativos

## Dados Inválidos

Caso algum dado não cumpra as regras de validação, o cliente não deverá ser criado ou atualizado.

Exemplos:

- nome vazio;
    
- telefone inválido;
    
- email inválido;
    
- data de nascimento futura.
    

---

## Cliente não Encontrado

Caso o cliente informado:

- não exista;
    
- ou pertença a outra ótica;
    

a operação deverá indicar que o cliente não foi encontrado dentro daquele contexto.

---

## Atualização de Cliente Inativo

Caso seja realizada uma tentativa de atualizar um cliente inativo:

```text
Client.IsActive = false
```

a atualização não deverá ocorrer.

O cliente deverá ser reativado primeiro.

---

## Desativação de Cliente já Inativo

O fluxo de desativação atua apenas sobre clientes ativos.

Caso o cliente já esteja inativo, a operação não realiza uma nova alteração.

---

## Reativação de Cliente já Ativo

O fluxo de reativação atua apenas sobre clientes inativos.

Caso o cliente já esteja ativo, a operação não realiza uma nova alteração.

---

# 13. Preservação do Histórico

A utilização de `IsActive` permite remover o cliente das operações normais sem eliminar o seu histórico.

Assim, evita-se que a desativação provoque a perda de relacionamentos como:

```text
Client
├── Prescriptions
└── Sales
```

Esse comportamento é importante porque vendas e receitas representam eventos históricos que devem permanecer disponíveis mesmo quando o cliente deixa de frequentar a ótica.

---

# 14. Funcionalidades Não Implementadas

O processo atual não possui funcionalidades específicas para:

- eliminar definitivamente um cliente;
    
- transferir um cliente entre óticas;
    
- fundir registos duplicados de clientes;
    
- manter histórico das alterações cadastrais;
    
- manter histórico das ativações e desativações;
    
- agendar atendimentos;
    
- gerir consultas ou marcações.
    

Essas funcionalidades poderão ser incorporadas futuramente caso se tornem necessárias.

---

# Resultado

O processo de Gestão de Clientes representa o ciclo atualmente suportado:

```text
Registo
   ↓
Cliente ativo
   ├── Consulta
   ├── Atualização
   ├── Receita
   └── Venda
   ↓
Desativação
   ↓
Cliente inativo
   ↓
Reativação
   ↓
Cliente ativo
```

Cada cliente permanece associado a uma única ótica e os seus dados históricos são preservados mesmo após a desativação.

A separação entre o estado do cliente e os seus registos históricos permite impedir novas operações com clientes inativos sem eliminar receitas ou vendas realizadas anteriormente.