# GoldCSharp

A modular ASP.NET Core Web API for a gold trading platform. The project models the core workflows of a digital gold and coin marketplace: OTP-based onboarding, JWT authentication, live market pricing, product catalog management, price quotes, idempotent quote confirmation, wallet operations, payment flows, and operational health monitoring.

The codebase is organized as a layered solution to keep business rules independent from HTTP, persistence, and external integrations.

## What this project demonstrates

- Designing a multi-project **Clean Architecture** solution in .NET
- Building feature-oriented application workflows with **CQRS and MediatR**
- Modeling domain rules with entities, value objects, domain services, and explicit error types
- Implementing **JWT access tokens** and rotated, hashed refresh tokens
- Building OTP-based registration and authentication flows
- Integrating live market prices through a typed `HttpClient` and a hosted refresh worker
- Handling quote confirmation with an `Idempotency-Key` to protect state-changing requests
- Applying validation, logging, and performance behaviors through MediatR pipeline behaviors
- Adding health endpoints for liveness, readiness, dependency checks, memory, and disk health
- Separating infrastructure concerns behind application abstractions for repositories, payments, caching, SMS, and pricing

## Core capabilities

### Authentication and identity

- Send and verify OTP codes
- Complete user registration
- Issue JWT access tokens
- Rotate and revoke refresh tokens
- Enforce refresh-token replay defense and active-token limits
- Read the current authenticated user

### Catalog and pricing

- Create, update, activate, deactivate, and delete gold/coin products
- Manage stock, weight, karat, images, descriptions, and minting fees
- Paginate and filter catalog results
- Read the latest market price and historical price ranges
- Refresh market prices from an external provider in the background
- Calculate product pricing through a dedicated pricing engine

### Trading workflow

- Create buy/sell quotes
- Confirm quotes with an idempotency key
- Keep quote, order, wallet, and ledger concerns behind repository abstractions
- Support payment creation and verification boundaries
- Provide a mock payment gateway for local development

## Architecture

```text
API
├── Controllers, contracts, filters, middleware, and health endpoints
│
Application
├── Use cases, CQRS commands/queries, validators, behaviors, and abstractions
│
Domain
└── Entities, value objects, domain events, errors, and business rules
│
Infrastructure
└── EF Core persistence, repositories, security, caching, payments, SMS, pricing, and integrations
```

### Solution projects

| Project | Responsibility |
| --- | --- |
| `API` | HTTP endpoints, authentication middleware, Swagger, CORS, health checks, and error handling |
| `Application` | Use cases, MediatR handlers, FluentValidation, pipeline behaviors, and ports |
| `Domain` | Business entities, value objects, domain services, events, and domain errors |
| `Infrastructure` | EF Core, SQL Server, repositories, migrations, token services, cache, payment, SMS, and external price APIs |

## Technology stack

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core 8** with SQL Server and in-memory development storage
- **MediatR** for CQRS and application pipelines
- **FluentValidation** for request validation
- **JWT Bearer authentication** with refresh-token rotation
- **Swagger / OpenAPI** for API exploration
- **HealthChecks UI** and custom operational checks
- **Serilog** integration in the infrastructure layer
- **Mapster** for object mapping
- **Ardalis.Specification** for query and repository specifications

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server for non-development environments
- An API key for the configured market-price provider when live prices are enabled

### 1. Clone and restore

```bash
git clone https://github.com/hoce1n/goldCSharp.git
cd goldCSharp
dotnet restore GOLD.sln
```

### 2. Configure local settings

Development uses an in-memory `GoldDb` database by default. Copy the safe development settings if needed and provide secrets through environment variables or user-secrets rather than committing them:

```bash
dotnet user-secrets init --project API
dotnet user-secrets set "Auth:JwtSecret" "replace-with-a-long-local-development-secret" --project API
dotnet user-secrets set "BrsApi:Key" "your-market-price-api-key" --project API
```

For production, provide a SQL Server connection string through `Gold_Connection` and configure the JWT secret, issuer, audience, and external integration credentials in the deployment secret store.

### 3. Run the API

```bash
dotnet run --project API
```

When running in Development, Swagger is available at:

```text
https://localhost:<port>/swagger
```

## API and operations

The main route groups are:

- `POST /api/auth/send-otp`
- `POST /api/auth/verify-otp`
- `POST /api/auth/complete-registration`
- `POST /api/auth/refresh-token`
- `GET /api/auth/me`
- `GET /api/coins`
- `GET /api/coins/{id}`
- `GET /api/marketprice/latest`
- `GET /api/marketprice/history`
- `POST /api/quote`
- `POST /api/quote/{quoteId}/confirm`

Health endpoints include `/health`, `/health/live`, `/health/ready`, `/health/details`, and `/health-ui`.

Quote confirmation requires an `Idempotency-Key` header so clients can safely retry a request without creating duplicate state changes.

## Database migrations

The EF Core migrations are stored under `Infrastructure/Migrations`.

```bash
dotnet ef database update --project Infrastructure --startup-project API
```

To create a new migration after changing the data model:

```bash
dotnet ef migrations add DescribeYourChange \
  --project Infrastructure \
  --startup-project API
```

## Configuration reference

| Setting | Purpose |
| --- | --- |
| `Gold_Connection` | Production SQL Server connection string |
| `Auth:JwtSecret` | JWT signing secret; use a secret manager outside local development |
| `Auth:Issuer` | JWT issuer |
| `Auth:Audience` | JWT audience |
| `BrsApi:BaseUrl` | Market-price provider endpoint |
| `BrsApi:Key` | Market-price provider credential |
| `SmsIr:ApiKey` | SMS provider credential |
| `Zarinpal:MerchantId` | Payment provider merchant identifier |

Do not commit real connection strings, API keys, JWT secrets, SMS credentials, or payment credentials.

## Project status

This repository is a backend portfolio project and an evolving implementation of a gold marketplace domain. Some integrations are intentionally represented by development-safe adapters, such as the mock payment gateway, so the application can be explored without external payment credentials.

## License

No license has been specified yet. Add a license before distributing or reusing the project publicly.
