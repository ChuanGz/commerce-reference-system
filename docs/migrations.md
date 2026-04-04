# Database Migrations

Database migrations should run as a controlled step, not as an automatic side effect of service startup in production.

## Local Usage

General pattern:

```bash
dotnet ef database update \
  --project src/back-end/services/<service>/src/<Service>.Infrastructure \
  --startup-project src/back-end/services/<service>/src/<Service>.API
```

Example for the order service:

```bash
dotnet ef database update \
  --project src/back-end/services/order-service/src/OrderService.Infrastructure \
  --startup-project src/back-end/services/order-service/src/OrderService.API
```

## Delivery Guidance

- run migrations as a separate CI or operator step
- avoid applying schema changes implicitly during production startup
- use migration bundles when deployment environments are locked down

## Script Reference

The repository includes a placeholder script at `scripts/run-migrations.sh`.
