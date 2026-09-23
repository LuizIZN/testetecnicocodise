# Anime API

API para cadastro e consulta de animes e diretores, com filtros, paginação e relacionamento entre as entidades.

## Tecnologias

- .NET 10 / ASP.NET Core
- Entity Framework Core
- PostgreSQL
- xUnit
- Swagger e Scalar para documentação da API

## Pré-requisitos

- .NET SDK 10
- PostgreSQL em execução

Crie um banco chamado `testetecnico` e configure o usuário e a senha em `Api/appsettings.json`.

Configuração padrão:

```text
Host=localhost;Port=5432;Database=testetecnico;Username=postgres;Password=root
```

## Executar o projeto

Na raiz do repositório:

```bash
dotnet restore
dotnet ef database update --project Api
dotnet run --project Api
```

A API fica disponível em:

- `http://localhost:5126`
- `https://localhost:7190`

Em ambiente de desenvolvimento, a documentação está disponível em `/swagger` e `/scalar`.

Os dados iniciais são inseridos automaticamente quando a aplicação é iniciada.

## Testes

```bash
dotnet test TesteTecnico.Tests/TesteTecnico.Tests.csproj
```

## Arquitetura

- `Api`: inicialização da aplicação, controllers, configurações, migrations e seeders.
- `Application`: DTOs, interfaces e regras de serviço.
- `Domain`: entidades e regras básicas do domínio.
- `Infraestructure`: `DbContext` e repositories com Entity Framework Core.
- `TesteTecnico.Tests`: testes unitários dos serviços e testes dos repositories com banco InMemory.

Fluxo principal:

```text
Controller -> Service -> Repository -> PostgreSQL
```

## Principais recursos

- CRUD de animes.
- CRUD de diretores.
- Associação de animes a diretores.
- Filtros por nome, ano de lançamento e quantidade de episódios.
- Paginação e ordenação por nome.
- Consulta dos animes de um diretor.
- Endpoint `GET /health-check`.
