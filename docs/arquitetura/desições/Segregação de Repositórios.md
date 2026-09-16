## Contexto

Os casos de uso do G-Otica necessitam consultar e alterar dados persistidos.

Essas operações são realizadas através de repositórios.

Entretanto, nem todos os casos de uso necessitam das mesmas operações.

Por exemplo:

```text
GetProductUseCase
→ apenas leitura
```

enquanto:

```text
RegisterProductUseCase
→ leitura e escrita
```

e:

```text
DeactivateProductUseCase
→ atualização
```

---

## Problema

Uma única interface de repositório contendo todas as operações faria com que cada caso de uso recebesse métodos que não necessita.

Por exemplo:

```csharp
IProductRepository
{
    GetById();
    GetAll();
    Add();
    Update();
    Delete();
}
```

Um caso de uso responsável apenas por consultar produtos teria acesso também a operações de escrita e atualização.

Isso aumentaria a superfície das dependências e tornaria menos explícita a intenção de cada caso de uso.

---

## Alternativas Consideradas

### Uma Interface por Entidade

Criar uma única interface:

```text
IProductRepository
```

contendo todas as operações relacionadas a `Product`.

A abordagem é simples, porém cada consumidor passa a depender de operações que pode não utilizar.

---

### Interfaces Separadas por Tipo de Operação

Separar os contratos de acordo com a responsabilidade necessária.

Por exemplo:

```text
IProductReadOnlyRepository
IProductWriteOnlyRepository
IProductUpdateOnlyRepository
```

Essa foi a estratégia escolhida.

---

## Decisão

Os repositórios do G-Otica são divididos de acordo com o tipo de acesso que disponibilizam.

As categorias mais utilizadas são:

```text
ReadOnly
WriteOnly
UpdateOnly
```

Nem toda entidade precisa obrigatoriamente possuir as três interfaces.

A separação depende das operações realmente existentes no sistema.

---

# ReadOnly

Interfaces `ReadOnly` representam operações de consulta.

Exemplo:

```text
IProductReadOnlyRepository
```

Pode disponibilizar operações como:

```text
GetById
GetByIds
GetAll
Exists
```

As implementações de leitura podem utilizar comportamentos específicos para consulta, como:

```text
AsNoTracking
```

quando o rastreamento das entidades não é necessário.

---

# WriteOnly

Interfaces `WriteOnly` representam a inclusão de novos dados.

Exemplo:

```text
IProductWriteOnlyRepository
```

Pode disponibilizar:

```text
Add
```

O repositório adiciona a entidade ao contexto, enquanto a persistência definitiva é coordenada pela unidade de trabalho.

---

# UpdateOnly

Interfaces `UpdateOnly` representam operações utilizadas para alterar entidades existentes.

Exemplo:

```text
IProductUpdateOnlyRepository
```

São utilizadas em operações como:

- atualização;
    
- ativação;
    
- desativação;
    
- alterações de stock.
    

---

# Unidade de Trabalho

A persistência definitiva é separada das operações individuais dos repositórios através de:

```text
IUnitOfWork
```

Assim, um caso de uso pode executar várias alterações e apenas depois confirmar a operação.

Exemplo:

```text
Purchase criada
+
Produtos atualizados
+
StockMovements criados
        ↓
IUnitOfWork.Commit()
```

Isso é especialmente importante para operações transacionais que modificam várias entidades.

---

## Exemplo

Um caso de uso responsável por registar um produto pode depender de:

```text
IUnitOfWork
IProductReadOnlyRepository
IProductWriteOnlyRepository
```

Enquanto um caso de uso apenas de consulta pode depender somente de:

```text
IProductReadOnlyRepository
```

Dessa forma, as dependências revelam de forma mais clara o que cada caso de uso necessita fazer.

---

## Contratos e Implementações

As interfaces são definidas fora da infraestrutura concreta.

A implementação permanece na camada `Infrastructure`.

Exemplo:

```text
Domain
IProductReadOnlyRepository
        ↑
        │
Infrastructure
ProductRepository
```

Uma mesma classe concreta pode implementar mais de uma interface.

Assim, não é necessário criar uma classe diferente para cada contrato.

Por exemplo:

```text
ProductRepository
├── IProductReadOnlyRepository
├── IProductWriteOnlyRepository
└── IProductUpdateOnlyRepository
```

A separação ocorre principalmente nos **contratos apresentados aos consumidores**, e não necessariamente nas classes concretas.

---

## Benefícios

A segregação oferece:

- dependências menores;
    
- intenção mais explícita nos casos de uso;
    
- menor exposição de operações desnecessárias;
    
- facilidade para identificar se um caso de uso lê ou altera dados;
    
- maior flexibilidade para implementar comportamentos diferentes de leitura e escrita;
    
- melhor organização dos contratos.
    

---

## Consequências

A abordagem aumenta a quantidade de interfaces existentes no projeto.

Em vez de:

```text
IProductRepository
```

podem existir:

```text
IProductReadOnlyRepository
IProductWriteOnlyRepository
IProductUpdateOnlyRepository
```

Esse aumento foi considerado aceitável porque os contratos permanecem pequenos e cada dependência possui uma responsabilidade clara.

---

## Conclusão

Foi adotada a segregação dos repositórios para que cada caso de uso dependa apenas das operações necessárias para executar a sua responsabilidade.

A divisão entre `ReadOnly`, `WriteOnly` e `UpdateOnly` torna as dependências mais explícitas sem exigir uma separação completa dos modelos ou da infraestrutura de persistência.