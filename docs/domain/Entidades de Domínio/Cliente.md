## 1. Contexto
A entidade `Cliente` representa a pessoa atendida pela ótica.

Ela é responsável por armazenar apenas as informações cadastrais necessárias para identificação e comunicação com o cliente.

Embora um cliente possa possuir um histórico de compras e diversas receitas ao longo do tempo, essas informações pertencem a entidades específicas do sistema.

Dessa forma, a responsabilidade da entidade `Cliente` é representar exclusivamente a pessoa que mantém um relacionamento comercial com a ótica.

---

## 2. Regras de Domínio
Durante o levantamento dos requisitos foram identificadas as seguintes regras do negócio.

#### Um cliente pode realizar várias compras
Ao longo do tempo, um cliente pode adquirir diferentes produtos e serviços.

Exemplos:
- Óculos completos;
- Apenas lentes;
- Apenas armações;
- Estojos;
- Flanelas;
- Acessórios.

Todas essas aquisições fazem parte do histórico comercial do cliente.

---

#### Um cliente pode possuir várias receitas
Cada exame realizado gera uma nova receita.

Mesmo quando o grau permanece inalterado, uma nova receita é emitida e passa a fazer parte do histórico clínico do cliente.

---

#### O histórico clínico pertence à Receita
As informações obtidas durante um exame de vista não pertencem ao cadastro do cliente.

Cada receita representa um momento específico da evolução visual do paciente e deve ser preservada para consultas futuras.

---

#### A recomendação para um novo exame pode variar
Embora seja comum recomendar exames anuais para adultos, esse intervalo depende da avaliação do profissional de saúde.

Alguns pacientes podem necessitar de retornos em períodos menores, como:
- três meses;
- seis meses;
- um ano.

Não existe uma periodicidade única aplicável a todos os clientes.

---

## 3. Decisões de Modelagem

#### O Cliente armazenará apenas informações cadastrais
Foi decidido manter a entidade simples, contendo apenas informações necessárias para identificação e contato.

Exemplos:
- Nome;
- Telefone;
- Email;
- Endereço;
- Documento de identificação;
- Estado do cadastro.

Informações clínicas permanecem em entidades específicas.

---

#### Não armazenar DataUltimoExame
Durante a modelagem foi considerada a criação desse atributo, mas essa abordagem foi descartada.

Como cada exame gera uma nova receita, a data do último exame pode ser obtida consultando a receita mais recente do cliente.

Manter essa informação também no cliente criaria redundância.

---

#### Não armazenar ProximoExameRecomendado
Também foi discutida a possibilidade de armazenar diretamente a próxima data recomendada para exame.

Essa ideia foi descartada por dois motivos:
- a recomendação depende da avaliação clínica realizada no exame;
- a informação pode ser obtida a partir da receita mais recente e das regras definidas pela ótica.

Essa decisão mantém uma única fonte da verdade para o histórico clínico.

---

#### O Cliente não armazenará informações clínicas
Dados como:
- Grau;
- Receita;
- DP;
- DNP;
- Histórico de evolução visual.

não pertencem ao cliente.

Essas informações serão mantidas na entidade `Receita` e nas demais entidades responsáveis pelo processo de venda de lentes.

---

#### O Cliente pertence a uma Ótica
Na primeira versão do sistema, cada cliente será associado a uma única ótica.

Essa decisão simplifica a modelagem e atende ao cenário atualmente identificado.

Caso surja a necessidade de compartilhamento de clientes entre diferentes unidades, essa modelagem poderá evoluir futuramente.

---

## 4. Benefícios
A modelagem adotada oferece diversas vantagens.

- Mantém responsabilidades bem definidas.
- Evita redundância de informações.
- Centraliza o histórico clínico na entidade `Receita`.
- Facilita futuras consultas ao histórico do cliente.
- Simplifica a manutenção da base de dados.
- Permite evolução do domínio sem alterar o cadastro do cliente.

---

## 5. Possíveis Evoluções
Dependendo das necessidades futuras do sistema, poderão ser adicionadas novas informações ao cadastro do cliente.

Exemplos:
- Data de nascimento;
- Preferência de contacto;
- Consentimento para comunicações;
- Observações gerais;
- Contacto alternativo;
- Integração entre clientes de diferentes óticas pertencentes ao mesmo proprietário.

Essas funcionalidades foram consideradas fora do escopo da primeira versão.

---

## 6. Conclusão
A entidade `Cliente` representa exclusivamente a pessoa atendida pela ótica.

Sua responsabilidade limita-se ao armazenamento das informações cadastrais necessárias para identificação e comunicação, enquanto os dados clínicos permanecem centralizados na entidade `Receita`.

Essa separação mantém a modelagem simples, evita redundâncias e estabelece responsabilidades claras entre as entidades do domínio.