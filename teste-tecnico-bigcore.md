# Teste Técnico — Processo Seletivo

Olá! Seja bem-vindo(a) ao nosso processo seletivo. Este teste foi elaborado para avaliarmos não apenas seu conhecimento técnico nas tecnologias utilizadas, mas também sua capacidade de organização, arquitetura de software e boas práticas de desenvolvimento.

Leia o documento com atenção antes de começar. Boa sorte!

---

## Tema do Projeto

O **tema é livre**, porém deve ser baseado em algo da **sua vivência pessoal ou profissional**. Queremos ver você resolvendo um problema real que faça sentido para você — pode ser uma ferramenta para seu hobby, algo que facilitaria sua rotina, um sistema inspirado em alguma experiência profissional anterior, ou qualquer outro projeto que demonstre sua criatividade.

Exemplos (apenas para inspiração, não precisa seguir):

- Um chat em tempo real para um grupo que você participa
- Um painel de acompanhamento de treinos, estudos ou finanças pessoais
- Um sistema de notificações para uma comunidade
- Um dashboard com dados ao vivo de algo que você acompanha

Importante: o tema **deve permitir explorar funcionalidades em tempo real** (para justificar o uso do SignalR).

---

## Stack Obrigatória

O projeto deve obrigatoriamente utilizar as seguintes tecnologias:

| Camada | Tecnologia |
|---|---|
| Front-end | **Vue.js** |
| Back-end | **.NET** |
| Banco de dados | **MongoDB** |
| Comunicação em tempo real | **SignalR** |

### Opcional (diferencial)

- **Redis** para cache

O uso de Redis não é obrigatório, mas será considerado um diferencial positivo caso seja implementado de forma coerente com a aplicação.

---

## Requisitos Técnicos

- A aplicação deve rodar em **containers Docker** (recomendado `docker-compose` para orquestrar front, back, MongoDB e, se aplicável, Redis).
- O projeto deve subir com **um único comando** (ex: `docker-compose up`).
- O repositório deve ser **público** no GitHub.
- Todos os serviços necessários devem estar containerizados e configurados.

---

## O que será avaliado

1. **Commits contínuos e explicados**
   Queremos ver a evolução do seu raciocínio ao longo do desenvolvimento. Commits pequenos, frequentes e com mensagens claras são esperados.

2. **Arquitetura do projeto**
   Organização de camadas, separação de responsabilidades, escolhas arquiteturais coerentes e justificáveis.

3. **Qualidade e organização do código**
   Código limpo, legível, nomes significativos, padrões consistentes e boas práticas da linguagem/framework.

4. **README organizado**
   Deve conter, no mínimo:
   - Descrição do projeto e do tema escolhido
   - Tecnologias utilizadas
   - Pré-requisitos
   - Instruções claras para rodar o projeto localmente
   - Como executar via Docker
   - Variáveis de ambiente necessárias (se houver)
   - Estrutura de pastas (opcional, mas bem-vindo)

---

## Critérios de Exclusão

O candidato será **automaticamente desclassificado** caso:

- O container **não inicialize** ou apresente **erro** ao subir a aplicação.
- Forem detectados **poucos commits** ou **um único commit**, indicando uso abusivo de IA ou falta de desenvolvimento incremental.
- **Alguma das tecnologias obrigatórias não for utilizada** (Vue.js, .NET, MongoDB, SignalR).

> O uso de ferramentas de IA como apoio é aceitável, mas queremos ver **seu processo de desenvolvimento**, suas decisões e sua evolução ao longo do projeto.

---

## Entregável

- **Link público do repositório no GitHub** contendo o projeto completo.
- **Prazo final**: **04/05, às 23:59**.

### Envio

O link do repositório deve ser enviado por e-mail para os seguintes endereços, **em cópia (todos juntos)**:

- rafael@bigcore.com.br
- guilherme@bigcore.com.br
- renato@bigcore.com.br

**Assunto sugerido**: `Teste Técnico - [Seu Nome Completo]`

No corpo do e-mail, inclua:

- Seu nome completo
- Link público do repositório no GitHub
- Breve descrição (1 a 2 parágrafos) sobre o tema escolhido e as decisões técnicas principais

---

## Dúvidas

Em caso de dúvidas sobre o teste, entre em contato por Whatsapp com Rafael (11) 9 5056-7731.

Bom desenvolvimento!
