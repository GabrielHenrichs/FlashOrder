# FlashOrder

API HTTP para cadastro, compra e venda de acoes com foco em regras de negocio, arquitetura em camadas e testabilidade.

## Objetivo

Implementar o desafio tecnico de compra e venda de acoes utilizando .NET, com:

- separacao de responsabilidades por camadas;
- regras de negocio explicitas;
- persistencia em memoria;
- testes unitarios e testes de contrato da API.

## Stack e prerequisitos

- .NET SDK 8.0+
- Sistema operacional com suporte ao .NET 8

## Como executar a aplicacao

1. Restaurar e compilar:

```bash
dotnet build FlashOrder.sln
```

2. Executar a API:

```bash
dotnet run --project src/FlashOrder.API/FlashOrder.API.csproj
```

3. Abrir Swagger (ambiente Development):

- `https://localhost:<porta>/swagger`

## Como executar os testes

Executar todos os testes:

```bash
dotnet test FlashOrder.sln
```

Executar testes da API (contrato):

```bash
dotnet test tests/FlashOrder.Tests/FlashOrder.Tests.csproj --filter "FullyQualifiedName~ApiContractTests"
```

## Arquitetura adotada

Estrutura em 4 camadas:

- `src/FlashOrder.API`
  - Controllers, contratos HTTP (request/response), middleware global de erro e composicao de DI.
- `src/FlashOrder.Application`
  - Use cases, DTOs, contratos de repositorio e excecoes de aplicacao.
- `src/FlashOrder.Domain`
  - Entidades e invariantes de dominio (`Stock`, `Position`, `Transaction`).
- `src/FlashOrder.Infra.Data`
  - Implementacoes de repositorios em memoria.

Projeto de testes:

- `tests/FlashOrder.Tests`

## Principais decisoes tecnicas

- Persistencia em memoria por escopo do desafio.
- Use cases expostos por interfaces para reduzir acoplamento da API.
- Configuracao de DI extraida para classe dedicada em `src/FlashOrder.API/Configuration/DependencyInjection.cs`.
- Tratamento global de excecoes em middleware com mapeamento por `ApplicationErrorCode` (sem comparacao por texto de mensagem).
- Regras de negocio no dominio/application e nao no controller.

## Premissas de escopo

- Preco da acao imutavel apos cadastro (nesta versao).
- Investidor unico implicito (sem autenticacao).
- Sem short sell, sem posicao negativa.
- Sem calculo de preco medio, lucro/prejuizo, impostos e taxas.

## Endpoints principais

- `POST /api/stocks`
- `GET /api/stocks`
- `POST /api/trades/buy`
- `POST /api/trades/sell`
- `GET /api/portfolio/positions`
- `GET /api/portfolio/transactions`

## Codigos HTTP relevantes

- `201 Created` para cadastro de acao com sucesso.
- `200 OK` para consultas e operacoes validas.
- `404 Not Found` para acao inexistente.
- `409 Conflict` para codigo de acao duplicado.
- `422 Unprocessable Entity` para violacao de regra de negocio.
- `500 Internal Server Error` para erro inesperado.
