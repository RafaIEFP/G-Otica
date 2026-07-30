## 1. Contexto
A entidade `Utilizador` representa uma pessoa que possui acesso ao sistema.

Ela é responsável exclusivamente pela identidade do utilizador, armazenando as informações necessárias para autenticação e identificação.

Um utilizador pode trabalhar em uma ou mais óticas, exercendo funções diferentes em cada uma delas.

Por esse motivo, a entidade `Utilizador` não representa um funcionário de uma ótica específica, mas sim uma pessoa que pode estar vinculada a diferentes unidades.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um utilizador pode trabalhar em várias óticas
Uma mesma pessoa pode estar vinculada a diversas unidades.

Exemplos:
- Proprietário de duas óticas.
- Gerente em uma unidade e vendedor em outra.
- Contabilista com acesso a todas as unidades.

---

#### Uma ótica possui vários utilizadores
Cada unidade pode possuir diversos colaboradores.

Entre eles:
- proprietário;
- gerente;
- vendedor;
- administrador.

---

#### Os cargos pertencem à relação com a ótica
O cargo exercido por um utilizador depende da unidade onde ele atua.

Uma mesma pessoa pode exercer funções diferentes em óticas distintas.

Exemplo:

João
- Proprietário da Ótica Centro.
- Gerente da Ótica Shopping.

Por esse motivo, o cargo não pertence ao utilizador.

Essa regra será representada pela entidade `UtilizadorOtica`.

---

#### O utilizador regista operações no sistema
As ações realizadas dentro da aplicação ficam associadas ao utilizador responsável.

Exemplos:
- criação de vendas;
- cadastro de produtos;
- registo de compras;
- recebimento de pagamentos.

Isso permite identificar quem executou cada operação.

---

## 3. Decisões de Modelagem

#### Utilizador representa identidade, não vínculo
Foi decidido manter a entidade `Utilizador` responsável apenas pela identidade da pessoa.

Ela armazena informações como:
- nome;
- e-mail;
- senha (hash);
- situação da conta.

Nenhuma informação relacionada à ótica ou ao cargo será armazenada diretamente nessa entidade.

---

#### Não existe `OticaId` em `Utilizador`
Inicialmente poderia parecer natural adicionar uma chave estrangeira para a ótica.

Essa abordagem foi descartada.

Ela impediria que um mesmo utilizador trabalhasse em mais de uma unidade e obrigaria a duplicação de cadastros.

Para representar corretamente o domínio foi criada a entidade intermediária `UtilizadorOtica`.

---

#### O cargo pertence ao vínculo
Foi decidido armazenar o cargo em `UtilizadorOtica`.

Essa decisão evita inconsistências quando um utilizador desempenha funções diferentes em unidades distintas.

Exemplo:
```text
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

#### O utilizador não representa permissões
A entidade `Utilizador` não será responsável por armazenar funções ou permissões.

Essas informações pertencem ao vínculo entre o utilizador e a ótica.

Essa decisão aumenta a flexibilidade do sistema e evita inconsistências quando um utilizador atua em diferentes unidades.

---

#### O utilizador regista a autoria das operações
Entidades como `Venda` e `Compra` armazenam `UtilizadorId`.

Esse relacionamento indica quem realizou a operação, mas não altera o pertencimento dos dados.

Os registos continuam pertencendo à ótica onde foram criados.

---

## 4. Benefícios
A modelagem adotada oferece diversas vantagens.

- Evita duplicação de utilizadores.
- Permite que uma pessoa trabalhe em várias óticas.
- Permite funções diferentes para a mesma pessoa em unidades distintas.
- Mantém a responsabilidade da entidade bem definida.
- Facilita futuras integrações com mecanismos de autenticação, como o ASP.NET Identity.
- Mantém separadas a identidade do utilizador e sua relação com a empresa.

---

## 5. Possíveis Evoluções

A entidade poderá evoluir futuramente com informações adicionais, como:

- foto de perfil;
- último acesso;
- autenticação em dois fatores;
- idioma preferencial;
- preferências pessoais;

Essas funcionalidades não fazem parte da primeira versão do sistema.

---

## 6. Conclusão
A entidade `Utilizador` representa exclusivamente a identidade de uma pessoa dentro do sistema.

Ela não possui conhecimento sobre as óticas em que o utilizador trabalha nem sobre as funções que exerce.

Essa separação permite reutilizar o mesmo utilizador em diferentes unidades, evita duplicação de cadastros e mantém a modelagem alinhada ao funcionamento real da empresa.

As informações relacionadas ao vínculo entre utilizador e ótica são tratadas pela entidade `UtilizadorOtica`, responsável por representar o contexto em que cada utilizador atua.