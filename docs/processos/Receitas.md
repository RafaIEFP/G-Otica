## Objetivo

Descrever o processo de registo e consulta das receitas associadas aos clientes de uma ótica.

As receitas representam prescrições clínicas utilizadas no processo de venda de lentes e também compõem o histórico visual do cliente.

O processo atual inclui:

- registo de uma nova receita;
    
- consulta de uma receita específica;
    
- consulta do histórico de receitas de um cliente;
    
- utilização de uma receita durante uma venda.
    

---

## Participantes

- Cliente;
    
- Utilizador da ótica;
    
- Sistema.
    

---

# 1. Acesso à Gestão de Receitas

As operações relacionadas às receitas são realizadas dentro do contexto de:

```text
OpticalStore
    ↓
Client
    ↓
Prescription
```

Para aceder a essas funcionalidades:

- o utilizador deve estar autenticado;
    
- deve possuir uma associação ativa com a ótica.
    

A receita não possui `OpticalStoreId` diretamente.

O contexto da ótica é determinado através do cliente ao qual a receita pertence.

---

# 2. Registo de Receita

Uma nova receita pode ser registada para um cliente ativo.

Cada novo exame ou prescrição deve gerar um novo registo.

Receitas anteriores não são substituídas.

---

## Pré-condições

Para registar uma receita:

- o utilizador deve estar autenticado;
    
- deve possuir acesso à ótica;
    
- o cliente deve existir;
    
- o cliente deve pertencer à ótica;
    
- o cliente deve estar ativo.
    

---

## Dados da Receita

Durante o registo podem ser informados:

- nome do profissional;
    
- número de registo profissional;
    
- data da receita;
    
- data de validade;
    
- esfera do olho direito;
    
- esfera do olho esquerdo;
    
- cilindro do olho direito;
    
- cilindro do olho esquerdo;
    
- eixo do olho direito;
    
- eixo do olho esquerdo;
    
- acuidade visual do olho direito;
    
- acuidade visual do olho esquerdo;
    
- adição;
    
- acuidade visual para perto;
    
- data recomendada para retorno;
    
- observações.
    

---

## Dados Obrigatórios

Atualmente são obrigatórios:

- nome do profissional;
    
- número de registo profissional;
    
- data da receita;
    
- data de validade.
    

Os restantes dados clínicos são opcionais.

---

# 3. Validação da Receita

Antes do registo, o sistema valida os dados recebidos.

---

## Profissional

Os campos:

```text
DoctorName
DoctorRegistration
```

não podem estar vazios.

---

## Data da Receita

A data da receita não pode estar no futuro.

Assim:

```text
PrescriptionDate <= data atual
```

Exemplo válido:

```text
Data atual:         16/09/2026
Data da receita:    10/09/2026
```

Exemplo inválido:

```text
Data atual:         16/09/2026
Data da receita:    20/09/2026
```

---

## Data de Validade

Uma nova receita não pode ser registada já expirada.

No momento do registo:

```text
ExpirationDate >= data atual
```

Uma receita com validade correspondente ao próprio dia ainda é considerada válida.

---

## Acuidade Visual

Quando informadas, as propriedades:

```text
RightEyeVisualAcuity
LeftEyeVisualAcuity
NearVisualAcuity
```

possuem limite máximo de 20 caracteres.

---

## Observações

`Notes` é opcional e possui limite máximo de 1000 caracteres.

---

# 4. Fluxo Principal de Registo

1. O utilizador seleciona um cliente.
    
2. Inicia o registo de uma nova receita.
    
3. Informa os dados da prescrição.
    
4. O sistema normaliza os dados recebidos.
    
5. O sistema valida os campos da receita.
    
6. O sistema verifica se o cliente existe, está ativo e pertence à ótica.
    
7. É criada uma nova `Prescription`.
    
8. A receita recebe o `ClientId` do cliente selecionado.
    
9. O registo é persistido.
    
10. A receita passa a integrar o histórico do cliente.
    

---

## Resultado

Após o registo:

```text
Client
   │
   ├── Prescription A
   ├── Prescription B
   └── Prescription C
```

Cada receita permanece como um registo independente.

---

# 5. Histórico de Receitas

Um cliente pode possuir várias receitas ao longo do tempo.

O sistema permite consultar esse histórico sem substituir ou eliminar as prescrições anteriores.

Exemplo:

```text
Cliente
├── Receita 2024
├── Receita 2025
└── Receita 2026
```

Isso permite acompanhar as prescrições que foram registadas em diferentes momentos.

---

# 6. Consulta de uma Receita

Uma receita específica pode ser consultada utilizando o contexto:

```text
OpticalStoreId
+
ClientId
+
PrescriptionId
```

O sistema verifica simultaneamente:

- o identificador da receita;
    
- o cliente ao qual ela pertence;
    
- a ótica do cliente.
    

Isso impede que uma receita de outro cliente ou de outra ótica seja obtida através desse fluxo.

---

## Informações Consultadas

A consulta individual disponibiliza os dados completos da receita, incluindo:

- profissional;
    
- registo profissional;
    
- datas;
    
- valores clínicos;
    
- acuidade visual;
    
- recomendação de retorno;
    
- observações.
    

---

# 7. Consulta do Histórico do Cliente

O sistema permite listar as receitas pertencentes a um cliente.

Antes da consulta, é verificado se o cliente existe dentro da ótica.

O cliente não precisa estar ativo para que o seu histórico seja consultado.

Isso permite preservar o acesso às receitas mesmo após a desativação do cliente.

---

## Paginação

A listagem é paginada.

Os parâmetros atualmente utilizados são:

```text
Page
PageSize
```

Os valores padrão são:

```text
Page = 1
PageSize = 20
```

`Page` deve ser maior que zero.

`PageSize` deve estar entre:

```text
1 e 100
```

---

## Ordenação

As receitas são apresentadas da mais recente para a mais antiga, utilizando inicialmente:

```text
PrescriptionDate
```

em ordem decrescente.

Quando duas receitas possuem a mesma data, o identificador é utilizado como segundo critério de ordenação.

---

## Informações Resumidas

Na listagem são apresentados os principais dados necessários para identificar cada receita:

- identificador;
    
- nome do profissional;
    
- número de registo profissional;
    
- data da receita;
    
- data de validade.
    

Os dados clínicos completos permanecem disponíveis na consulta individual.

---

# 8. Cliente Inativo

A desativação do cliente possui comportamentos diferentes para criação e consulta.

---

## Nova Receita

Um cliente inativo não pode receber uma nova receita.

```text
Client.IsActive = false
        ↓
Nova Prescription
        ↓
Não permitido
```

---

## Histórico Existente

As receitas anteriormente registadas continuam disponíveis para consulta.

```text
Client.IsActive = false

Prescription A → preservada
Prescription B → preservada
Prescription C → preservada
```

A desativação do cliente não elimina o histórico clínico.

---

# 9. Utilização da Receita em Vendas

Uma receita pode ser associada a uma venda.

Essa associação é opcional na estrutura da venda, pois nem todos os produtos exigem prescrição.

---

## Venda sem Lentes

Uma venda composta apenas por produtos comuns pode ser realizada sem receita.

Exemplo:

```text
Sale
├── Frame
└── Case

PrescriptionId = null
```

---

## Venda com Lentes

Quando a venda contém pelo menos um produto do tipo `Lens`, uma receita torna-se obrigatória.

A receita deverá:

- existir;
    
- pertencer ao cliente da venda;
    
- pertencer ao contexto da mesma ótica;
    
- não estar expirada.
    

---

## Validação da Validade

No momento da venda, o sistema verifica novamente:

```text
ExpirationDate
```

Caso:

```text
ExpirationDate < data atual
```

a receita não poderá ser utilizada.

Assim, uma receita pode permanecer armazenada no histórico mesmo depois de expirada, mas deixa de poder ser usada numa nova venda.

---

## Reutilização

Uma receita válida pode ser utilizada em mais de uma venda do mesmo cliente.

A sua utilização não altera o seu estado nem a torna automaticamente inválida.

---

# 10. Receita e ItemLens

A receita contém a prescrição clínica.

As informações específicas de fabricação da lente permanecem separadas em:

```text
ItemLens
```

Por exemplo:

```text
Prescription
├── Sphere
├── Cylinder
├── Axis
└── Visual Acuity
```

enquanto:

```text
ItemLens
├── EyeSide
├── PupillaryDistance
├── NasoPupillaryDistance
├── LensType
├── RefractiveIndex
├── Material
├── Color
└── Diameter
```

Essa separação evita misturar:

```text
Prescrição clínica
```

com:

```text
Configuração da lente produzida
```

---

# 11. DP e DNP

DP e DNP não são armazenadas na receita.

Essas medidas são registadas no `ItemLens` correspondente à lente produzida.

Assim:

```text
Prescription
→ dados da prescrição
```

e:

```text
ItemLens
→ medidas e características de fabricação
```

---

# 12. Recomendação de Retorno

A receita pode possuir:

```text
RecommendedReturnDate
```

Essa informação é opcional.

Ela representa uma recomendação associada àquela prescrição específica.

A data não é armazenada diretamente no cliente.

Dessa forma, diferentes receitas podem possuir diferentes recomendações ao longo do tempo.

---

# 13. Preservação do Histórico

A implementação atual não possui operações para:

- atualizar uma receita;
    
- eliminar uma receita.
    

Depois de registada, a receita permanece como parte do histórico do cliente.

Caso seja emitida uma nova prescrição, deverá ser criado um novo registo.

Exemplo:

```text
Receita antiga
        ↓
permanece armazenada

Nova prescrição
        ↓
nova Prescription
```

Essa abordagem evita que uma alteração substitua informações clínicas anteriormente registadas.

---

# 14. Fluxos Alternativos

## Cliente não Encontrado

Caso o cliente:

- não exista;
    
- pertença a outra ótica;
    

o registo ou consulta deverá indicar que o cliente não foi encontrado no contexto informado.

---

## Cliente Inativo no Registo

Caso:

```text
Client.IsActive = false
```

uma nova receita não poderá ser criada.

---

## Data da Receita no Futuro

Caso:

```text
PrescriptionDate > data atual
```

o registo deverá ser rejeitado.

---

## Receita já Expirada no Registo

Caso:

```text
ExpirationDate < data atual
```

o registo deverá ser rejeitado.

---

## Receita não Pertence ao Cliente

Ao consultar ou utilizar uma receita numa venda, caso ela pertença a outro cliente:

```text
Prescription.ClientId != ClientId
```

a receita não deverá ser encontrada dentro daquele contexto.

---

## Receita de Outra Ótica

Como a receita pertence à ótica através do cliente, uma receita cujo cliente pertença a outra ótica não poderá ser utilizada.

---

## Receita Expirada na Venda

Uma receita pode continuar no histórico após expirar.

Entretanto:

```text
Prescription
ExpirationDate < data atual
```

não poderá ser associada a uma nova venda.

---

# 15. Funcionalidades Não Implementadas

O processo atual não possui funcionalidades específicas para:

- editar uma receita já registada;
    
- eliminar receitas;
    
- anexar imagem ou PDF da receita original;
    
- registar a clínica responsável;
    
- representar o profissional através de uma entidade própria;
    
- utilizar assinatura eletrónica;
    
- controlar receitas digitais;
    
- enviar lembretes automáticos com base em `RecommendedReturnDate`;
    
- aplicar validações clínicas específicas aos valores de esfera, cilindro, eixo ou adição.
    

Essas funcionalidades poderão ser incorporadas futuramente caso se tornem necessárias.

---

# Resultado

O processo de Gestão de Receitas permite manter o histórico das prescrições associadas a cada cliente.

O fluxo principal é:

```text
Cliente ativo
    ↓
Nova prescrição
    ↓
Validação
    ↓
Prescription registada
    ↓
Histórico do cliente
```

Posteriormente, uma receita válida pode participar do processo de venda:

```text
Prescription
     ↓
   Sale
     ↓
ItemLens
```

A receita preserva os dados clínicos, enquanto as características específicas da fabricação permanecem em `ItemLens`.

Receitas antigas ou expiradas continuam disponíveis para consulta, mas apenas receitas válidas podem ser utilizadas em novas vendas com lentes.