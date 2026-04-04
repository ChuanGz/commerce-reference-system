# Configuration Baseline

This document defines the minimum configuration rules for local development and shared environments.

## Principles

- Do not commit real passwords, signing keys, tokens, or private credentials.
- Use placeholders in checked-in configuration files.
- Provide real values through local environment variables, local-only config files, or secret managers.
- Keep development configuration readable, but never treat sample values as deployable defaults.

## Connection Strings

Checked-in `appsettings*.json` and Docker examples should use placeholders such as:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=<set-me>;User Id=<set-me>;Password=<set-me>;TrustServerCertificate=true;"
  }
}
```

## JWT Settings

Checked-in JWT settings should use placeholders such as:

```json
{
  "Jwt": {
    "Issuer": "IdentityService",
    "Audience": "ServiceClient",
    "Secret": "<set-me>"
  }
}
```

## Docker Configuration

Docker Compose examples may document the required environment variables, but should not contain real passwords or signing keys.

Example:

```yaml
environment:
  - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=<set-me>;User Id=sa;Password=<set-me>
  - Jwt__Secret=<set-me>
```

## Local Overrides

Recommended local options:

- shell environment variables
- `.env.local` files kept out of version control
- local-only `appsettings.Local.json` files

## Review Rule

Before making a repository public, scan it for:

- connection strings with real credentials
- hardcoded JWT signing keys
- cloud storage keys and signed URLs
- copied production tokens or passwords in docs and scripts
