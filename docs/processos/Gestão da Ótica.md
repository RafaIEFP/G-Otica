## Objetivo

Descrever o processo de criação e gestão de uma ótica no G-Otica, incluindo o registo da unidade, consulta, atualização dos seus dados, transferência de propriedade e desativação.

A gestão dos colaboradores, papéis e convites é tratada separadamente no processo de **Gestão da Equipa e Convites**.

---

## Participantes

- Utilizador autenticado;
    
- Proprietário da ótica;
    
- Sistema.
    

---

# 1. Registo da Ótica

## Pré-condições

Para registar uma nova ótica:

- o utilizador deve possuir uma conta ativa;
    
- o utilizador deve estar autenticado no sistema.
    

Não é necessário que o utilizador já pertença a outra ótica.

---

## Fluxo Principal

1. O utilizador inicia o registo de uma nova ótica.
    
2. São informados:
    
    - nome;
        
    - email;
        
    - número de telefone;
        
    - número fiscal.
        
3. O sistema normaliza e valida os dados recebidos.
    
4. O sistema verifica se já existe uma ótica com o mesmo número fiscal.
    
5. Caso não exista, é criada uma nova `OpticalStore`.
    
6. O sistema cria automaticamente uma associação `UserOpticalStore` entre o utilizador e a nova ótica.
    
7. Essa associação é criada com:
    
    - papel `owner`;
        
    - estado ativo;
        
    - data de entrada correspondente ao momento do registo.
        
8. As duas informações são persistidas.
    
9. A ótica fica disponível para utilização.
    

---

## Resultado

Ao concluir o registo:

```text
Utilizador
    │
    │ owner
    ↓
UserOpticalStore
    │
    ↓
OpticalStore
```

A nova ótica encontra-se ativa e o utilizador responsável pela criação passa a ser o seu proprietário.

---

# 2. Validação dos Dados da Ótica

Durante o registo e atualização, o sistema deverá validar os principais dados da ótica.

## Nome

O nome é obrigatório.

---

## Email

O email:

- é obrigatório;
    
- deve possuir um formato válido.
    

---

## Número de Telefone

O número de telefone é obrigatório e deve possuir um formato válido.

A validação considera o formato internacional utilizado pela aplicação.

---

## Número Fiscal

O número fiscal é obrigatório.

Não podem existir duas óticas registadas com o mesmo número fiscal.

Essa regra também considera óticas inativas, uma vez que a desativação não elimina o registo.

---

# 3. Consulta das Óticas do Utilizador

Um utilizador autenticado pode consultar as óticas às quais possui uma associação ativa.

O sistema utiliza `UserOpticalStore` para identificar essas unidades.

Para cada associação podem ser apresentadas informações como:

- identificador da ótica;
    
- nome;
    
- papel do utilizador;
    
- data de entrada;
    
- estado da ótica.
    

---

## Consulta de uma Ótica Específica

O utilizador pode consultar os dados de uma ótica específica quando possuir uma associação ativa com ela.

A consulta pode disponibilizar:

- nome;
    
- email;
    
- telefone;
    
- número fiscal;
    
- estado;
    
- papel do utilizador;
    
- data de entrada.
    

Caso o utilizador não possua uma associação ativa com a ótica, ela não deverá ser disponibilizada através desse fluxo.

---

# 4. Atualização da Ótica

A atualização dos dados da ótica é uma operação reservada ao proprietário.

## Pré-condições

Para atualizar uma ótica:

- o utilizador deve estar autenticado;
    
- deve possuir uma associação ativa com a ótica;
    
- o seu papel deve ser `owner`.
    

---

## Fluxo Principal

1. O proprietário seleciona a ótica.
    
2. Informa os novos dados.
    
3. O sistema valida:
    
    - nome;
        
    - email;
        
    - telefone;
        
    - número fiscal.
        
4. Caso o número fiscal tenha sido alterado, o sistema verifica se já pertence a outra ótica.
    
5. Os dados da `OpticalStore` são atualizados.
    
6. As alterações são persistidas.
    

---

## Número Fiscal já Utilizado

Caso o novo número fiscal já pertença a outra ótica, a atualização deverá ser rejeitada.

Exemplo:

```text
Ótica A
TaxNumber = 123456789

Ótica B tenta utilizar:
TaxNumber = 123456789

Resultado:
Operação rejeitada
```

---

# 5. Transferência de Propriedade

A propriedade de uma ótica pode ser transferida para outro utilizador.

Essa operação é necessária, por exemplo, quando o proprietário atual deixa de ser responsável pela unidade ou pretende posteriormente desativar a sua própria conta.

---

## Pré-condições

A transferência somente poderá ocorrer quando:

- a ótica estiver ativa;
    
- o utilizador que realiza a operação for o proprietário atual;
    
- o novo proprietário for diferente do proprietário atual;
    
- o novo proprietário possuir uma conta ativa;
    
- o novo proprietário já possuir uma associação ativa com a ótica.
    

A transferência de propriedade **não cria uma nova associação**.

Caso o utilizador ainda não pertença à ótica, deverá primeiro entrar através do processo apropriado de gestão da equipa.

---

## Fluxo Principal

1. O proprietário atual seleciona o utilizador que deverá assumir a propriedade.
    
2. O sistema verifica se a ótica está ativa.
    
3. O sistema verifica se o novo proprietário existe e possui conta ativa.
    
4. O sistema verifica se ele já pertence ativamente à ótica.
    
5. O papel do proprietário atual é alterado de:
    

```text
owner
```

para:

```text
manager
```

6. O papel do novo proprietário é alterado para:
    

```text
owner
```

7. As duas alterações são persistidas na mesma operação.
    

---

## Resultado

Antes da transferência:

```text
Utilizador A → owner
Utilizador B → manager
```

Depois da transferência:

```text
Utilizador A → manager
Utilizador B → owner
```

A associação de ambos com a ótica é preservada.

---

## Transferência para o Próprio Utilizador

O proprietário atual não pode transferir a propriedade para si próprio.

Nesse caso, a operação deverá ser rejeitada.

---

## Novo Proprietário Inativo

Caso a conta do novo proprietário esteja inativa, a transferência não poderá ocorrer.

---

## Novo Proprietário não Pertence à Ótica

Caso o utilizador exista, mas não possua uma associação ativa com a ótica, a transferência também deverá ser rejeitada.

---

# 6. Desativação da Ótica

A desativação é uma operação reservada ao proprietário.

A ótica não é eliminada da base de dados.

Em vez disso:

```text
OpticalStore.IsActive = false
```

---

## Pré-condições

Para desativar a ótica:

- o utilizador deve estar autenticado;
    
- deve ser o proprietário ativo da ótica.
    

---

## Fluxo Principal

1. O proprietário solicita a desativação.
    
2. O sistema altera o estado da `OpticalStore` para inativo.
    
3. Todas as associações `UserOpticalStore` ativas daquela ótica são também desativadas.
    
4. As alterações são realizadas dentro da mesma operação transacional.
    

---

## Efeito nas Associações

Antes da desativação:

```text
OpticalStore
IsActive = true

User A → owner       IsActive = true
User B → manager     IsActive = true
User C → salesperson IsActive = true
```

Depois:

```text
OpticalStore
IsActive = false

User A → owner       IsActive = false
User B → manager     IsActive = false
User C → salesperson IsActive = false
```

Os registos continuam armazenados, mas deixam de representar acesso ativo à ótica.

---

# 7. Preservação dos Dados

A desativação da ótica não deverá eliminar os seus dados históricos.

Devem permanecer preservadas informações relacionadas, como:

- clientes;
    
- receitas;
    
- produtos;
    
- fornecedores;
    
- compras;
    
- vendas;
    
- pagamentos;
    
- movimentações de stock;
    
- tratamentos;
    
- associações de utilizadores;
    
- convites existentes.
    

A desativação representa apenas que a unidade deixou de estar disponível para operações normais.

---

# 8. Reativação da Ótica

A versão atual do sistema **não possui um fluxo de reativação de `OpticalStore`**.

Como a desativação também inativa as associações `UserOpticalStore`, uma eventual funcionalidade futura de reativação deverá definir claramente:

- quem possui autorização para reativar a ótica;
    
- quais associações deverão voltar a ficar ativas;
    
- se todos os antigos utilizadores serão reativados;
    
- ou se apenas o proprietário deverá recuperar inicialmente o acesso.
    

Por esse motivo, a reativação não faz parte do processo atual.

---

# 9. Fluxos Alternativos

## Número Fiscal Duplicado

Caso já exista uma ótica com o mesmo número fiscal:

```text
Registo ou atualização
        ↓
Número fiscal já utilizado
        ↓
Operação rejeitada
```

---

## Utilizador sem Permissão

Operações administrativas como:

- atualização;
    
- transferência de propriedade;
    
- desativação;
    

somente poderão ser realizadas pelo proprietário da ótica.

Caso outro utilizador tente executar essas operações, o acesso deverá ser recusado.

---

## Ótica Inativa

Uma ótica inativa não deverá ser utilizada normalmente em novos processos de negócio.

Como as associações ativas são também desativadas, os utilizadores deixam de possuir acesso ativo à unidade através dos fluxos normais da aplicação.

---

# 10. Consistência das Operações

Algumas operações alteram mais de uma entidade e devem preservar a consistência dos dados.

## Criação

A criação envolve:

```text
OpticalStore
+
UserOpticalStore do owner
```

A nova ótica deverá possuir um proprietário associado.

---

## Transferência de Propriedade

A transferência envolve simultaneamente:

```text
Owner atual → manager
Novo owner  → owner
```

As duas mudanças devem fazer parte da mesma operação.

---

## Desativação

A desativação envolve:

```text
OpticalStore
+
UserOpticalStores ativos
```

A alteração deverá ser atómica para evitar situações em que a ótica esteja inativa, mas continue com associações de acesso ativas.

---

# Resultado

O processo de Gestão da Ótica cobre o ciclo atualmente suportado para uma unidade:

```text
Criação
   ↓
Utilização
   ↓
Atualização
   ↓
Transferência de propriedade, quando necessário
   ↓
Desativação
```

A criação estabelece automaticamente o utilizador responsável como `owner`.

Enquanto estiver ativa, a ótica pode ser consultada pelos seus membros e administrada pelo proprietário.

A propriedade pode ser transferida para outro membro ativo, mantendo o antigo proprietário como `manager`.

Quando desativada, a unidade e as associações dos seus utilizadores permanecem armazenadas para preservação histórica, mas deixam de estar ativas no sistema.