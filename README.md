# WEC Real-time Weather Tracker 🏎️⛅

Um sistema desenvolvido para equipes do World Endurance Championship (WEC) acompanharem em tempo real as condições climáticas de pistas oficiais, permitindo uma melhor tomada de decisão sobre estratégias e troca de pneus. 

Pistas cobertas:
- Imola
- Interlagos
- Circuit de la Sarthe
- Spa-Francorchamps
- Circuit of the Americas
- Fuji Speedway
- Losail International Circuit
- Bahrain International Circuit

Este projeto foi construído como parte de um teste técnico, focado em arquitetura limpa, boas práticas e evolução contínua.

## Tecnologias Utilizadas

- **Front-end:** Vue.js 3 (Composition API)
- **Back-end:** .NET 8 (Clean Architecture)
- **Banco de Dados:** MongoDB
- **Comunicação em Tempo Real:** SignalR
- **Cache / Backplane SignalR:** Redis (Diferencial)
- **Infraestrutura:** Docker e Docker Compose

## Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop/) instalado e rodando.
- [Docker Compose](https://docs.docker.com/compose/install/) instalado.
- Opcional para desenvolvimento: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) e [Node.js](https://nodejs.org/).

## Estrutura de Pastas

A estrutura base para o projeto é a seguinte:
```text
/
├── src/
│   ├── backend/   # Solução .NET 8 com Clean Architecture
│   └── frontend/  # Aplicação Vue.js 3
├── docker-compose.yml
└── README.md
```

## Como rodar o projeto

1. Clone este repositório:
   ```bash
   git clone https://github.com/SEU_USUARIO/wec-realtime-weather-tracker.git
   ```

2. Acesse o diretório do projeto:
   ```bash
   cd wec-realtime-weather-tracker
   ```

3. Suba a infraestrutura via Docker Compose:
   ```bash
   docker-compose up -d
   ```

*(Nota: Nas fases iniciais de desenvolvimento, a subida completa inclui as bases de dados. As aplicações Backend e Frontend serão integradas posteriormente na orquestração final).*

## Variáveis de Ambiente

O projeto utiliza as seguintes conexões padrão na infraestrutura Docker (que serão injetadas nas aplicações):
- **MongoDB:** `mongodb://mongodb:27017`
- **Redis:** `redis:6379`