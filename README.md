# Template Architect

Template Architect is a full-stack reference project that matches the current
requirements: a customer Storefront (React), an internal Backoffice (Angular),
and .NET microservices for users, products, orders, and inventory.

Use this repo to understand **what the system does**, **where each part lives**,
and **how to run the basics**.

---

## What This Repository Contains

- **Storefront (React)**: customer-facing product browsing and ordering
- **Backoffice (Angular)**: internal CRUD for users/products/orders
- **.NET microservices**: identity, user, customer, product, inventory, order, payment
- **Docs**: requirements and technical notes under `docs/`

See requirements in `docs/requirements/1-requirement.adoc`.

---

## Project Structure

```
template-architect/
├── src/
│   ├── front-end/
│   │   ├── storefront/   # React storefront app
│   │   └── backoffice/   # Angular backoffice app
│   └── back-end/
│       ├── BackEnd.sln   # .NET solution
│       └── services/
│           ├── identity-service/
│           ├── user-service/
│           ├── customer-service/
│           ├── product-service/
│           ├── inventory-service/
│           ├── order-service/
│           └── payment-service/
├── docs/
└── .github/workflows/    # CI/CD workflows per service
```

---

## Service Responsibilities (High Level)

### Frontend
- **Storefront (React)**: product listing, login, order creation, order history
- **Backoffice (Angular)**: CRUD for users/products, order review and status changes

### Backend
- **Identity service**: authentication, token issuance, SSO/MFA (per requirements)
- **User service**: roles and user CRUD
- **Customer service**: customer profile data used by orders
- **Product service**: catalog and pricing
- **Inventory service**: stock, commit, and availability
- **Order service**: order creation and lifecycle
- **Payment service**: extension point for payment flows

---

## Why One Repository (Monorepo)

- **Single source of truth** for requirements, docs, and contracts
- **Cross-service changes** are easier to review in one PR
- **Consistent tooling** (lint/test/build) across all services
- **Shared UI Core** and frontend conventions stay aligned

---

## CI/CD Workflows (Why They Exist)

Workflows in `.github/workflows/*-service.yaml` are **per service** and trigger
on path changes. This keeps builds focused and avoids rebuilding the whole repo
for a small change.

If directories move, update the workflow path filters so CI triggers correctly.

---

## Quick Start (Basic)

### Prerequisites
- Node.js (for frontends)
- .NET 8 SDK (for services)

### Auth Configuration (Required)
Backend services expect:
- `Auth:Authority`
- `Auth:Audience`

Set them in `appsettings.*.json` or environment variables before running APIs.

### Run Storefront
```bash
cd src/front-end/storefront
npm install
npm start
```

### Run Backoffice
```bash
cd src/front-end/backoffice
npm install
npm start
```

### Run a Backend Service (example: Order Service)
```bash
dotnet run --project src/back-end/services/order-service/src/OrderService.API
```

For full orchestration, see `docs/technical-notes/HOW_TO_RUN.md`.

---

## Docs

- Requirements: `docs/requirements/1-requirement.adoc`
- Technical notes: `docs/technical-notes/`
