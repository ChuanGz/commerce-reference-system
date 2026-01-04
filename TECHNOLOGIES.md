# Tools and Technologies

This document explains the **chosen tech stack**, why it fits the current
requirements, and provides small proof-of-evidence examples.

---

## 1. Architecture Context (From Requirements)

- **Storefront**: React SPA for customers
- **Backoffice**: Angular SPA for internal operations
- **Backend**: .NET microservices behind an **API Gateway**
- **Identity**: Microsoft SSO + MFA
- **Core flows**: Products, Orders, Users, Inventory

---

## 2. Backend Stack (Why It Fits)

### .NET 8 (Latest LTS)
**Why**:
- Meets requirement: “latest .NET version”
- Strong performance and tooling for microservices
- Native support for modern auth and DI

**Problems solved**:
- Stable long-term support for enterprise workloads
- Consistent API patterns across services

### Entity Framework Core
**Why**:
- Fast delivery for CRUD-heavy services
- Migrations for consistent schema evolution

**Problems solved**:
- Reduces manual SQL boilerplate
- Safer refactors with tracked migrations

### PostgreSQL / SQL Server
**Why**:
- Both are enterprise-grade RDBMS
- Works well with EF Core and relational models

**Problems solved**:
- ACID consistency for orders, inventory, and user data

### API Gateway (Routing + Auth)
**Why**:
- Requirement: “API Gateway must route all requests”
- Centralizes auth, rate limiting, and cross-cutting concerns

**Problems solved**:
- Prevents duplicated auth logic in each service
- Single entry point for client applications

---

## 3. Frontend Stack (Why It Fits)

### React (Storefront)
**Why**:
- Lightweight SPA with fast iteration
- Excellent ecosystem for data fetching and forms

**Problems solved**:
- Rapid storefront delivery without UI reimplementation

### Angular (Backoffice)
**Why**:
- Strong structure for large internal apps
- Built-in tooling for forms, routing, and state

**Problems solved**:
- Consistent patterns for a large internal team

### TailwindCSS + UI Core
**Why**:
- Design consistency and fast UI assembly
- UI Core prevents re-implementation drift

**Problems solved**:
- Avoids fragmented design across multiple teams

---

## 4. DevOps & Observability (Why It Fits)

### Docker + Docker Compose
**Why**:
- Reproducible local environments for multiple services

**Problems solved**:
- Reduces “works on my machine” issues

### Kubernetes (Production Scale)
**Why**:
- Standard orchestration for microservices

**Problems solved**:
- Scaling, rolling updates, and service discovery

### Prometheus + Grafana
**Why**:
- Metrics for service health and performance

**Problems solved**:
- Detects slow queries, API spikes, and latency regressions

### ELK Stack
**Why**:
- Centralized log aggregation and search

**Problems solved**:
- Faster debugging across multiple services

### GitHub Actions
**Why**:
- CI/CD integrated with the repository

**Problems solved**:
- Consistent builds, tests, and deployment gates

---

## 5. Proof of Evidence (Code Samples)

### 5.1 API Gateway Routing (Conceptual)
```text
Client -> API Gateway -> Services
```

Gateway routes all traffic and applies auth once, so services only implement
business logic.

---

### 5.2 .NET Minimal API (Order Service)
```csharp
app.MapPost("/orders", async (CreateOrderRequest request, OrderDbContext db) =>
{
    var order = new Order
    {
        CustomerId = request.CustomerId,
        Lines = request.Lines
    };

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
})
.RequireAuthorization("Member");
```

**Why this matters**:
- Shows a clean CRUD endpoint with auth
- Matches requirement for role-based access

---

### 5.3 React (Storefront) Data Fetching
```tsx
const { data, isLoading } = useQuery({
  queryKey: ["products"],
  queryFn: () => api.get("/products").then((res) => res.data),
});
```

**Why this matters**:
- Keeps server state in React Query
- Prevents duplicated source of truth

---

### 5.4 Angular (Backoffice) Service Pattern
```ts
@Injectable({ providedIn: "root" })
export class ProductService {
  constructor(private http: HttpClient) {}

  list() {
    return this.http.get<Product[]>("/api/products");
  }
}
```

**Why this matters**:
- Centralizes API access in services
- Matches enterprise Angular conventions

---

## 6. System Diagram (ASCII)

```text
               +--------------------+
               |   Storefront (React)|
               +----------+---------+
                          |
               +----------v---------+
               |  Backoffice (Angular)
               +----------+---------+
                          |
                    +-----v-----+
                    | API Gateway|
                    +-----+-----+
                          |
     +----------+---------+---------+----------+
     |          |                   |          |
 +---v---+  +---v----+         +----v---+  +---v-----+
 |Identity| |UserSvc|         |Product|  |OrderSvc |
 |  SSO   | |       |         |Svc    |  |         |
 +---+----+ +---+---+         +---+---+  +---+-----+
     |          |                 |          |
     +----------+-----+-----------+----------+
                    Databases
```

**Why this matters**:
- Visual proof of microservice separation
- Identity isolated as its own service

---

## 7. Summary: Why This Stack Is the Right Fit

- **Meets all functional requirements** (React + Angular + .NET microservices)
- **Scales with team size** (clear boundaries and shared UI Core)
- **Reduces long-term risk** (LTS .NET, strong observability, CI/CD)
- **Supports enterprise security** (SSO, MFA, centralized gateway)
