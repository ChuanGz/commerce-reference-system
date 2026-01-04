# Database Migrations

Production environments **must not** run EF Core migrations on service startup.
Migrations should be executed in a controlled step (CI/CD or operator run).

## Local Development

Run migrations for a service:
```bash
dotnet ef database update \
  --project src/back-end/services/<service>/src/<Service>.Infrastructure \
  --startup-project src/back-end/services/<service>/src/<Service>.API
```

Example (Order Service):
```bash
dotnet ef database update \
  --project src/back-end/services/order-service/src/OrderService.Infrastructure \
  --startup-project src/back-end/services/order-service/src/OrderService.API
```

## Production / Staging

Recommended approach:
- Run migrations as a **separate pipeline step** before service deployment.
- Use EF Core migrations bundle if needed for locked-down environments.

This repo includes a placeholder script under `scripts/run-migrations.sh`.
