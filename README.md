# FlashOrder

API HTTP para cadastro, compra e venda de ações.

## Pre-requisitos

Para executar localmente:

- .NET SDK 8.0+
- sistema operacional com suporte ao .NET 8

Para executar com Docker (opcional):

- Docker Engine 24+ (ou Docker Desktop)
- Docker Compose v2

## Como executar a aplicação (sem Docker)

1. Restaurar dependências:

```bash
dotnet restore FlashOrder.sln
```

2. Compilar a solução:

```bash
dotnet build FlashOrder.sln
```

3. Executar a API em porta fixa:

```bash
dotnet run --project src/FlashOrder.API/FlashOrder.API.csproj --urls http://localhost:8080
```

4. Validar endpoints:

- Swagger: `http://localhost:8080/swagger`
- Health check: `http://localhost:8080/health`

## Como executar com Docker

Build da imagem:

```bash
docker build -t flashorder-api .
```

Executar container:

```bash
docker run --rm -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development flashorder-api
```

Ou subir com compose:

```bash
docker compose up --build
```

Endpoints no container:

- Swagger: `http://localhost:8080/swagger`
- Health check: `http://localhost:8080/health`

## Como executar os testes

Executar toda a suíte:

```bash
dotnet test tests/FlashOrder.Tests/FlashOrder.Tests.csproj
```

Executar apenas testes de integração da API:

```bash
dotnet test tests/FlashOrder.Tests/FlashOrder.Tests.csproj --filter "FullyQualifiedName~ApiIntegrationTests"
```

## Arquitetura adotada

Estrutura em 4 camadas (Clean Architecture):

- `src/FlashOrder.API`
  - Controllers, contratos HTTP (request/response), middlewares e composição de DI.
- `src/FlashOrder.Application`
  - Use cases, DTOs, contratos de repositório e exceções de aplicação.
- `src/FlashOrder.Domain`
  - Entidades e invariantes de domínio (`Stock`, `Position`, `Transaction`).
- `src/FlashOrder.Infra.Data`
  - Implementações de repositórios em memória.

Projeto de testes:

- `tests/FlashOrder.Tests`

## Principais decisões técnicas

- Persistência em memória por escopo do desafio.
- Versionamento de API por URL segment (`/api/v1/...`).
- Configuração de DI extraída para classe dedicada em `src/FlashOrder.API/Configuration/DependencyInjection.cs`.
- Tratamento global de exceções em middleware com mapeamento por `ApplicationErrorCode`.
- Logging estruturado em middleware para requests (`Method`, `Path`, `StatusCode`, `ElapsedMs`, `TraceId`) e exceções (`ErrorCode`, `TraceId`).
- Health check dedicado em `GET /health`.
- Regras de negócio no domínio/application e não no controller.

## Premissas de escopo

- Preço da ação imutável após cadastro (nesta versão).
- Investidor único implícito (sem autenticação).
- Sem short sell, sem posição negativa.
- Sem cálculo de preço médio, lucro/prejuízo, impostos e taxas.

## Endpoints principais

- `GET /health`
- `POST /api/v1/stocks`
- `GET /api/v1/stocks`
- `GET /api/v1/stocks/{code}`
- `POST /api/v1/trades/buy`
- `POST /api/v1/trades/sell`
- `GET /api/v1/portfolio/positions`
- `GET /api/v1/portfolio/positions/{stockCode}`
- `GET /api/v1/portfolio/transactions`

## Códigos HTTP relevantes

- `201 Created` para cadastro de ação com sucesso.
- `200 OK` para consultas e operações válidas.
- `404 Not Found` para ação inexistente.
- `409 Conflict` para código de ação duplicado.
- `422 Unprocessable Entity` para violação de regra de negócio.
- `500 Internal Server Error` para erro inesperado.
