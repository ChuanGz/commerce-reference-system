# How to Run (Local)

This project is a **monorepo** with:
- **Storefront** (React) and **Backoffice** (Angular) frontends.
- **.NET microservices** (Identity, User, Customer, Product, Inventory, Order, Payment).

See system flows in `ServiceDuty.md` and the stack rationale in `TechStack.md`.

---

## Prerequisites
- Node.js (for frontends)
- .NET 8 SDK (for services)
- Docker + Docker Compose (optional but recommended)

---

## Option A: Run Frontends Locally (Fastest)

### Storefront (React)
```bash
cd src/front-end/storefront
npm install
npm start
```

### Backoffice (Angular)
```bash
cd src/front-end/backoffice
npm install
npm start
```

---

## Option B: Run Backend Services Locally

### Example: Order Service
```bash
dotnet run --project src/back-end/services/order-service/src/OrderService.API
```

### Other services
```text
src/back-end/services/identity-service/...
src/back-end/services/user-service/...
src/back-end/services/customer-service/...
src/back-end/services/product-service/...
src/back-end/services/inventory-service/...
src/back-end/services/payment-service/...
```

---

## Option C: Docker Compose (Local Stack)

Docker compose lives at `src/docker-compose.yaml`.

```bash
cd src
docker-compose up --build
```

Default ports (from `src/docker-compose.yaml`):
- Storefront: http://localhost:3000
- Backoffice: http://localhost:4200
- Identity: http://localhost:5001
- User: http://localhost:5002
- Customer: http://localhost:5003
- Product: http://localhost:5004
- Inventory: http://localhost:5005
- Order: http://localhost:5006

---

## CI/CD Note

Workflows are in `.github/workflows/` and run **per service** based on path
filters. If you move folders, update the workflow path filters accordingly.
