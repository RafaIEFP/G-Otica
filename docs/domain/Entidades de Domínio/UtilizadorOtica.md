## 1. Contexto
A entidade `UtilizadorOtica` representa o vínculo entre um utilizador e uma ótica.

Embora um utilizador possua uma identidade única dentro do sistema, sua participação na empresa acontece sempre dentro do contexto de uma unidade.

É nesse vínculo que são definidas informações como o cargo exercido, a data de entrada e o status do colaborador naquela ótica.

Dessa forma, a entidade não existe apenas para conectar duas tabelas, mas para representar a relação profissional entre uma pessoa e uma unidade da empresa.

---


## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um utilizador pode trabalhar em várias óticas
Uma mesma pessoa pode atuar em diferentes unidades da empresa.

Exemplos:
- proprietário de duas óticas;
- gerente em duas unidades;
- vendedor em apenas uma unidade;
- contador com acesso a todas as óticas.

---

#### Uma ótica possui vários utilizadores
Cada unidade possui sua própria equipe de trabalho.

Essa equipe pode ser composta por diferentes tipos de colaboradores.

---

#### O cargo depende da ótica
O cargo exercido por um utilizador não é uma característica da pessoa.
Ele depende da unidade onde ela trabalha.

Exemplo:
João
  - Proprietário da Ótica Centro.
  - Gerente da Ótica Shopping.

O mesmo utilizador exerce funções diferentes em unidades diferentes.

---

#### O vínculo pode ser encerrado
Um colaborador pode deixar de trabalhar em determinada ótica sem perder sua conta no sistema.

Da mesma forma, ele pode continuar ativo em outras unidades.

Por esse motivo, o vínculo possui seu próprio status.

---


## 3. Decisões de Modelagem

#### Foi criada uma entidade intermediária
Em vez de adicionar `OticaId` diretamente em `Utilizador`, foi criada a entidade `UtilizadorOtica`.

Essa abordagem representa corretamente a relação muitos-para-muitos existente entre utilizadores e óticas.

---
#### O cargo pertence ao vínculo
Foi decidido armazenar o cargo em `UtilizadorOtica`.

Essa decisão evita inconsistências quando um Utilizador desempenha funções diferentes em unidades distintas.

Exemplo:
```
João
 ├── Ótica Centro (Administrador)
 ├── Ótica Shopping (Gerente)

Rafael
 ├── Ótica Centro (Vendedor)

Yan
 ├── Ótica Centro (Gerente)
 ├── Ótica Shopping (Gerente)
```

---

#### O vínculo possui informações próprias
Além das chaves estrangeiras, a entidade armazena informações que pertencem exclusivamente à relação entre utilizador e ótica.

Exemplos:
- Cargo
- DataEntrada
- Ativo

Esses dados não fazem sentido nem em `Utilizador` nem em `Otica`, pois descrevem apenas o relacionamento entre ambos.

---

#### As entidades de negócio continuam pertencendo à ótica
Mesmo existindo um vínculo entre utilizador e ótica, as entidades de negócio continuam referenciando apenas `OticaId`.

Exemplos:
- Cliente
- Produto
- Venda
- Compra
- Pagamento

O `UtilizadorId` presente em algumas dessas entidades serve apenas para identificar quem realizou determinada operação.

O pertencimento dos dados continua sendo da ótica.

---


## 4. Benefícios
A modelagem adotada oferece diversas vantagens.

- Permite múltiplas unidades por utilizador.
- Evita duplicação de cadastros.
- Permite cargos diferentes para uma mesma pessoa.
- Mantém o histórico de colaboradores por unidade.
- Facilita o controle de permissões.
- Aproxima a modelagem da estrutura organizacional da empresa.
- Mantém separadas a identidade da pessoa e sua atuação profissional.

---

## 5. Possíveis Evoluções
Caso novas necessidades surjam, a entidade poderá armazenar outras informações relacionadas ao vínculo.

Exemplos:
- Data de saída.
- Motivo do desligamento.
- Jornada de trabalho.
- Comissão.
- Meta de vendas.
- Perfil de acesso específico da unidade.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão
A entidade `UtilizadorOtica` representa o relacionamento profissional entre um utilizador e uma ótica.

Ela permite que uma mesma pessoa atue em diferentes unidades, exercendo funções distintas em cada uma delas, sem duplicação de cadastros.

Além de resolver o relacionamento entre utilizadores e óticas, essa modelagem aproxima o sistema da organização real da empresa e estabelece um ponto central para futuras regras de autorização e gestão de colaboradores.