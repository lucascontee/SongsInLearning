# 🎵 SongsInLearning

Uma aplicação desktop para transformar o estudo de músicas em uma experiência completa de aprendizado, prática e experimentação.

SongsInLearning é uma aplicação desenvolvida em **C#** e **Avalonia UI** com o objetivo de centralizar o processo de aprendizado de músicas.

A ideia é simples: o usuário cadastra uma música, adiciona informações importantes para o estudo e utiliza um ambiente dedicado para fazer anotações, praticar e tocar junto com uma backing track, utilizando sua interface de áudio e plugins VST para criar uma experiência semelhante a um pequeno estúdio de prática.

## 🎯 Objetivo

O projeto nasceu da ideia de criar uma ferramenta voltada para músicos que desejam organizar melhor seus estudos.

Em vez de apenas armazenar o nome de uma música, o SongsInLearning permite reunir informações técnicas e pessoais sobre cada música e transformar esse conteúdo em um ambiente de estudo.

Cada música pode possuir informações como:

- 🎵 Nome da música
- 🎤 Artista
- 📅 Ano
- 🎸 Instrumento
- 🎚️ Afinação
- 📊 Dificuldade
- ⏱️ BPM
- 📈 Progresso de aprendizado
- 🤖 Informações geradas por IA
- 📝 Anotações pessoais

## ✨ Funcionalidades

### 🎵 Biblioteca de músicas

A página principal apresenta as músicas cadastradas em formato de Cards, permitindo visualizar e acessar rapidamente cada música.

Ao selecionar uma música, o usuário é direcionado para o seu ambiente de edição.

### 📝 Editor da música

Cada música possui um editor próprio onde é possível visualizar e editar suas informações.

Além dos dados técnicos, o usuário pode adicionar anotações gerais sobre o estudo da música.

Essas anotações podem ser utilizadas para registrar:

- Técnicas que precisam ser praticadas
- Partes difíceis da música
- Observações pessoais
- Informações importantes para o estudo
- Objetivos de aprendizado

### 🎛️ Estúdio

Uma das principais funcionalidades do projeto é o Estúdio.

O Estúdio transforma o SongsInLearning em um ambiente de prática musical, permitindo que o usuário utilize sua entrada de áudio para tocar seu instrumento enquanto pratica.

#### 🎙️ Entrada de áudio

O usuário pode utilizar uma interface de áudio para capturar o sinal do instrumento.

Isso permite utilizar o SongsInLearning não apenas como um gerenciador de músicas, mas também como uma ferramenta de prática.

#### 🎶 Backing Tracks

É possível carregar e reproduzir uma backing track enquanto o usuário toca.

A ideia é permitir uma experiência semelhante a tocar junto com uma banda, mantendo todo o estudo concentrado dentro da aplicação.

#### 🎚️ Plugins VST

O Estúdio também possui suporte à utilização de plugins VST, permitindo adicionar efeitos ao sinal de áudio.

Por exemplo:

```
🎸 Guitarra
    ↓
Interface de Áudio
    ↓
SongsInLearning
    ↓
VST Plugins
    ↓
🔊 Áudio processado
```

Isso possibilita utilizar efeitos como:

- Distortion
- Overdrive
- Delay
- Reverb
- Chorus
- Equalização
- Amplificadores virtuais

## 🧠 Informações com IA

O modelo de uma música também possui um campo destinado a informações geradas por IA.

A proposta é utilizar inteligência artificial para enriquecer o processo de aprendizado, adicionando informações técnicas e contextuais sobre a música.

Essas informações podem complementar os dados cadastrados pelo usuário e servir como material de apoio durante o estudo.

## 🏗️ Arquitetura

O projeto utiliza uma organização baseada na separação de responsabilidades entre modelos, serviços, ViewModels e Views.

```
SongsInLearning
│
├── Assets
│
├── Context
│
├── Messages
│
├── Migrations
│
├── Models
│
├── Services
│
├── Validators
│
├── ViewModels
│
└── View
```

### 📁 Assets

Recursos utilizados pela aplicação, como imagens, ícones e outros arquivos necessários para a interface.

### 📁 Context

Responsável pelo contexto de persistência e acesso aos dados da aplicação.

### 📁 Messages

Estruturas utilizadas para comunicação entre diferentes partes da aplicação.

### 📁 Migrations

Migrações responsáveis pela evolução da estrutura do banco de dados.

### 📁 Models

Contém os modelos que representam as entidades da aplicação.

Por exemplo, uma `Song` possui informações como nome, artista, ano, dificuldade, afinação, progresso, instrumento, BPM e anotações.

### 📁 Services

Centraliza regras e operações da aplicação, evitando concentrar responsabilidades diretamente nas Views ou ViewModels.

### 📁 Validators

Responsável pelas validações dos dados utilizados pela aplicação.

### 📁 ViewModels

Camada responsável pela lógica de apresentação e comunicação entre a interface e os serviços.

### 📁 View

Contém as interfaces gráficas desenvolvidas utilizando Avalonia UI.

## 🛠️ Tecnologias

**Backend / Aplicação**

- C#
- .NET 10
- Programação Orientada a Objetos
- Entity Framework Core

**Desktop UI**

- Avalonia UI
- MVVM

**Banco de dados**

- SQLite
- Entity Framework Core
- Migrations

**Áudio**

- Interface de áudio
- Backing Tracks
- Plugins VST
- Processamento de áudio

**Outros**

- Git
- GitHub
- IA generativa

## 🗺️ Fluxo da aplicação

```
                    ┌───────────────────┐
                    │  SongsInLearning  │
                    └─────────┬─────────┘
                              │
                              ▼
                    ┌───────────────────┐
                    │ Biblioteca        │
                    │ de músicas        │
                    └─────────┬─────────┘
                              │
                              ▼
                    ┌───────────────────┐
                    │ Editor da música  │
                    └───────┬─────┬─────┘
                            │     │
                 ┌──────────┘     └──────────┐
                 ▼                           ▼
        ┌─────────────────┐          ┌─────────────────┐
        │ Anotações       │          │     Estúdio     │
        │ e informações   │          │                 │
        └─────────────────┘          └────────┬────────┘
                                              │
                         ┌────────────────────┼────────────────────┐
                         ▼                    ▼                    ▼
                   🎙️ Entrada          🎶 Backing Track       🎚️ VST
                     de áudio
```

## 🚀 Roadmap

Algumas funcionalidades planejadas para evolução do projeto:

- Melhorias no sistema de gravação
- Gerenciamento avançado de plugins VST
- Cadeia de efeitos para instrumentos
- Controles de volume e mixagem
- Melhorias no editor de músicas
- Evolução das funcionalidades de IA
- Métricas de progresso de aprendizado
- Melhorias na organização da biblioteca
- Sistema de playlists / sessões de estudo
- Melhorias na experiência do Estúdio

## 📌 Status

🚧 **Em desenvolvimento**

O SongsInLearning é um projeto em desenvolvimento contínuo, utilizado também como projeto de estudo e experimentação com C#, .NET, Avalonia, arquitetura de aplicações desktop e processamento de áudio.
