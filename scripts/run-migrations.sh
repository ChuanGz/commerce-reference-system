#!/usr/bin/env bash
set -euo pipefail

# Placeholder: run migrations in CI/CD before deploying services.
# Update <service> and <Service> per target.

dotnet ef database update \
  --project "src/back-end/services/<service>/src/<Service>.Infrastructure" \
  --startup-project "src/back-end/services/<service>/src/<Service>.API"
