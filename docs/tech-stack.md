# Tech Stack

This document explains the intended technology choices and why they fit the repository.

## Frontend

### Storefront

- React
- TypeScript

Why:

- fast iteration for customer-facing UI
- broad ecosystem for routing, data fetching, and forms

### Backoffice

- Angular
- TypeScript

Why:

- stronger structure for large internal applications
- clear patterns for forms, routing, and service-based API access

## Backend

- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQL Server or PostgreSQL

Why:

- consistent service implementation model
- strong tooling for APIs, background logic, and data access
- suitable for a multi-service commerce backend

## Platform And Delivery

- API Gateway
- Docker
- Docker Compose
- GitHub Actions
- Kubernetes for production-scale deployment

Why:

- route all client traffic through a single boundary
- support local multi-service execution
- keep CI and deployment workflows service-aware

## Observability

- Prometheus
- Grafana
- centralized logging stack

Why:

- service health visibility
- performance monitoring
- faster debugging across multiple services

## Fit For This Repository

- React fits the storefront delivery speed
- Angular fits the backoffice structure and team scale
- .NET fits service consistency and long-term maintainability
- gateway and container tooling fit a multi-service environment
