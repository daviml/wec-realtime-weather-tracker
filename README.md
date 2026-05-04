# WEC Real-time Weather Tracker 🏎️⛅

Um sistema de monitoramento em tempo real desenvolvido para equipes do World Endurance Championship (WEC). O sistema permite acompanhar as condições climáticas das pistas oficiais (Interlagos, Le Mans, Spa, etc.) para tomada de decisão estratégica sobre pneus e paradas nos boxes.

## 🏁 Funcionalidades Principais

- **Dashboard Real-time**: Visualização instantânea do clima em todos os circuitos do calendário.
- **Integração SignalR**: Atualizações automáticas via WebSockets sem necessidade de refresh.
- **Histórico de Dados**: Consulta aos últimos snapshots climáticos de cada pista.
- **Estética Premium**: Interface inspirada em painéis de telemetria de automobilismo com tema dark.

## 🛠️ Tecnologias Utilizadas

- **Front-end**: Vue.js 3 (Composition API), Pinia (State Management) e SignalR Client.
- **Back-end**: .NET 10 (Clean Architecture).
- **Banco de Dados**: MongoDB (Persistência de dados climáticos).
- **Cache & Real-time**: Redis (Utilizado como Cache e Backplane do SignalR).
- **Infraestrutura**: Docker & Docker Compose para orquestração completa.

## 📋 Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução.

## 🚀 Como rodar o projeto

O projeto está totalmente conteinerizado e configurado para subir com um único comando.

1.  **Clone o repositório**:
    ```bash
    git clone https://github.com/SEU_USUARIO/wec-realtime-weather-tracker.git
    cd wec-realtime-weather-tracker
    ```

2.  **Inicie a aplicação via Docker Compose**:
    ```bash
    docker-compose up --build -d
    ```
    *Este comando irá construir as imagens do Frontend e Backend e subir os serviços de MongoDB e Redis.*

3.  **Acesse as interfaces**:
    - **Frontend (Dashboard)**: [http://localhost:8080](http://localhost:8080)
    - **Backend (API/Swagger)**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

## 🏗️ Estrutura do Projeto

```text
/
├── src/
│   ├── backend/   # Solução .NET 10 com Clean Architecture (Domain, Application, Infra, API)
│   └── frontend/  # Aplicação Vue 3 com Vite e Tailwind-inspired CSS
├── docker-compose.yml # Orquestração de todos os serviços
└── README.md
```

## ⚙️ Variáveis de Ambiente

As configurações de conexão são injetadas automaticamente via Docker Compose:
- **MongoDB**: `mongodb://mongodb:27017`
- **Redis**: `redis:6379`

---
*Este projeto foi desenvolvido como um teste técnico, focando em qualidade de código, escalabilidade e experiência do usuário.*