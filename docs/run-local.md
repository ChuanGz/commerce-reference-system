# Run Local

This guide covers the simplest local run options for the repository.

## Prerequisites

- Node.js
- .NET 8 SDK
- Docker and Docker Compose for the full local stack

## Configuration Rule

The repository intentionally keeps only placeholder credentials in checked-in config files.

Before running any service, provide real values through environment variables or local-only config overrides.

## Run Frontend Apps

### Storefront

```bash
cd src/front-end/storefront
npm install
npm start
```

### Backoffice

```bash
cd src/front-end/backoffice
npm install
npm start
```

## Run Backend Services

Example for the order service:

```bash
dotnet run --project src/back-end/services/order-service/src/OrderService.API
```

Other services follow the same shape:

- `src/back-end/services/identity-service/src/IdentityService.API`
- `src/back-end/services/user-service/src/UserService.API`
- `src/back-end/services/customer-service/src/CustomerService.API`
- `src/back-end/services/product-service/src/ProductService.API`
- `src/back-end/services/inventory-service/src/InventoryService.API`
- `src/back-end/services/payment-service/src/PaymentService.API`

## Common Local Variables

Examples:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=<set-me>;User Id=<set-me>;Password=<set-me>;TrustServerCertificate=true;"
export Jwt__Secret="<set-me>"
```

## Run Docker Compose

```bash
cd src
docker compose up --build
```

## Default Local Ports

- storefront: `http://localhost:3000`
- backoffice: `http://localhost:4200`
- identity-service: `http://localhost:5001`
- user-service: `http://localhost:5002`
- customer-service: `http://localhost:5003`
- product-service: `http://localhost:5004`
- inventory-service: `http://localhost:5005`
- order-service: `http://localhost:5006`

## Current Compose Note

The current `src/docker-compose.yaml` does not include `payment-service`, even though the repository contains that service. Keep this mismatch in mind when discussing local orchestration.
