# Backend Services Duty & Flow Map

This document explains **why services are split**, what each service owns,
the **end-to-end e-commerce flows**, and **failure impact + mitigation**.

---

## 1. Why This Service Split

- **Domain ownership**: each service owns its data and business rules.
- **Lower coupling**: changes in Order do not break Product/Inventory.
- **Scales by load**: Order/Inventory/Payment can scale independently.
- **Security boundary**: Identity is isolated and easier to harden.

---

## 2. End-to-End E-commerce Flows (Overview)

When demoing, **Payment** and **Inventory** flows must be explicit to show
real-world domain understanding.

### Flow A: Customer drafts order → Admin approves
1. Storefront creates an **Order Draft**.
2. Order Service saves draft and calls Inventory Service to **reserve/commit**.
3. Backoffice shows the order → Admin **approves/rejects**.
4. If rejected: release inventory, update order status.

### Flow B: Delivery + proof of delivery
1. Approved order → **Delivering**.
2. Delivery evidence (photo/signature/receipt) stored by Order Service
   or an attached storage.
3. On completion → **Delivered**.

### Flow C: Pay before/after, cash/bank transfer
1. **Prepaid**: Payment Service creates intent → confirm success →
   Order becomes **Paid**.
2. **COD**: Payment recorded after delivery confirmation.
3. **Bank transfer**: Payment verified via webhook or manual reconciliation.

### Flow D: Invoice + accounting entries
1. On order completion, Invoice is issued (Order + Payment).
2. Payment Service records accounting entries:
   - **Debit/Credit** based on payment method.
   - Links transaction ↔ order ↔ invoice.

---

## 3. Service Definitions

Each service includes:
- **Name**
- **Primary responsibility** (problem solved, users, input, output)
- **Related services**
- **Main flows**
- **Failure impact**
- **Open questions**

---

### 3.1 Identity Service

- **Primary responsibility**: AuthN, AuthZ, SSO, MFA.
  - **Users**: everyone (Storefront + Backoffice).
  - **Input**: credentials / SSO token / MFA challenge.
  - **Output**: access token + roles/claims + refresh token.

- **Related services**: User Service, API Gateway (if present), all services.

- **Main flows**:
  - Login → token issuance.
  - Token validation → role/claim enforcement.
  - MFA challenge when required.

- **Defense in depth (evidence)**:
  - Password hashing (hash + salt).
  - Signed JWT + short expiry.
  - Refresh token rotation.
  - Role-based policies at API layer.
  - Rate limiting + audit logs.

```csharp
services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer", options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateIssuerSigningKey = true,
      ValidateLifetime = true
    };
  });

services.AddAuthorization(options =>
{
  options.AddPolicy("Backoffice", p => p.RequireRole("BackofficeUser"));
});
```

- **Failure impact**:
  - Users cannot login or refresh tokens.
  - New requests may fail when tokens expire.

- **Open questions**:
  - IdP choice (Azure AD / MSAL / custom)?
  - MFA required for which roles?
  - Token lifetime and rotation policy?

---

### 3.2 User Service

- **Primary responsibility**: user profile + role management.
  - **Users**: Admin/Backoffice.
  - **Input**: CRUD for users/roles.
  - **Output**: user data and role mapping.

- **Related services**: Identity Service, Customer Service.

- **Main flows**:
  - Admin creates user → Identity grants access.
  - Role sync: User Service is the source of role data.

- **Failure impact**:
  - Cannot create or update users/roles.
  - Existing tokens still allow normal runtime use.

- **Open questions**:
  - Identity vs User Service: single source of truth for roles?
  - Sync strategy between Identity ↔ User?

---

### 3.3 Customer Service

- **Primary responsibility**: customer profile data.
  - **Users**: Customer + Backoffice.
  - **Input**: profile updates, history, contact.
  - **Output**: customer profile.

- **Related services**: User Service, Order Service.

- **Main flows**:
  - Order creation → Order Service fetches customer profile.
  - Customer updates profile → affects delivery/invoice data.

- **Failure impact**:
  - New orders may miss customer info.
  - Backoffice cannot view customer history.

- **Open questions**:
  - Customer data ownership: Customer vs User Service?
  - Do we need profile caching in Order Service?

---

### 3.4 Product Service

- **Primary responsibility**: product catalog, pricing, and descriptions.
  - **Users**: Storefront + Backoffice.
  - **Input**: product CRUD.
  - **Output**: product data for storefront and orders.

- **Related services**: Inventory Service, Order Service.

- **Main flows**:
  - Storefront product list → Product Service.
  - Order draft needs product snapshot (price/UoM) → Product Service.

- **Failure impact**:
  - Storefront cannot display products.
  - Order drafts cannot be created.

- **Open questions**:
  - Snapshot pricing in Order Service?
  - Dynamic or fixed pricing?

---

### 3.5 Inventory Service

- **Primary responsibility**: stock, reservations, and movements.
  - **Users**: Order Service + Backoffice.
  - **Input**: reserve, commit, release, adjust.
  - **Output**: available stock + movements.

- **Related services**: Product Service, Order Service.

- **Main flows**:
  - Order Draft → reserve/commit.
  - Order Reject/Cancel → release stock.
  - Delivery/Complete → decrement stock.

- **Failure impact**:
  - Cannot reserve stock → prevent oversell.
  - Order creation may be blocked or delayed.

- **Open questions**:
  - Reservation timeout policy?
  - Stock movement audit requirements?

---

### 3.6 Order Service

- **Primary responsibility**: order lifecycle management.
  - **Users**: Customer + Backoffice.
  - **Input**: draft, approve/reject, deliver, cancel.
  - **Output**: order status + history.

- **Related services**: Customer, Product, Inventory, Payment.

- **Main flows**:
  - Draft → Approved/Rejected.
  - Approved → Delivering → Delivered → Completed.
  - Cancel in allowed states → release inventory.

- **Failure impact**:
  - Core business down (no orders).
  - Storefront cannot transact.

- **Open questions**:
  - Official order state machine?
  - SLA for approvals?

---

### 3.7 Payment Service

- **Primary responsibility**: payment processing + money movement.
  - **Users**: Customer + Backoffice + Finance.
  - **Input**: payment intent, confirm, refund.
  - **Output**: payment status + transactions.

- **Related services**: Order Service, Identity Service.

- **Main flows**:
  - Prepaid: create intent → confirm → order paid.
  - COD: mark paid after delivery.
  - Bank transfer: verify via webhook/manual.
  - Invoice + accounting entry.

- **Failure impact**:
  - Cannot process new payments.
  - Orders can continue but remain unpaid/pending.

- **Open questions**:
  - PSP provider (VNPay/Momo/Stripe)?
  - Refund/chargeback required?
  - Accounting integration depth?

---

## 4. Failure Impact Summary + Mitigation

### Impact Summary (Quick View)

```text
Identity down   -> login/refresh fails; existing tokens may still work
User down       -> cannot manage users/roles; runtime mostly ok
Customer down   -> order creation risk; backoffice loses customer view
Product down    -> storefront catalog fails; no new orders
Inventory down  -> order creation blocked to avoid oversell
Order down      -> core business down (no orders)
Payment down    -> no new payments; orders can be pending
```

### Mitigation Strategies (Recommended)

- **Identity down**: allow existing JWT validation; short-term read-only mode.
- **User down**: cache role claims in token; defer admin operations.
- **Customer down**: use cached profile snapshot for draft orders.
- **Product down**: serve cached catalog; block checkout if price is stale.
- **Inventory down**: create order in **PendingInventory** status and retry.
- **Order down**: storefront shows maintenance and rejects new order actions.
- **Payment down**: allow COD flow; queue payment attempts for retry.

### Sample Resilience Code (Production-style)

```csharp
// Order Service -> Inventory Service with retry + circuit breaker + fallback
builder.Services.AddHttpClient("InventoryClient", c =>
{
    c.BaseAddress = new Uri(inventoryUrl);
    c.Timeout = TimeSpan.FromSeconds(5);
})
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(r => !r.IsSuccessStatusCode)
    .WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * retry)))
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

app.MapPost("/orders/draft", async (CreateDraft req, IHttpClientFactory factory) =>
{
    var client = factory.CreateClient("InventoryClient");
    var reserve = await client.PostAsJsonAsync("/reserve", req.Lines);

    if (!reserve.IsSuccessStatusCode)
    {
        // Fallback: create draft as PendingInventory for later retry
        return Results.Accepted("/orders/pending", new { status = "PendingInventory" });
    }

    return Results.Created("/orders/draft", new { status = "Reserved" });
});
```

---

## 5. Open Questions (To Confirm With Stakeholders)

- Where is the API Gateway and when will it be introduced?
- Source of truth for roles/profile: Identity vs User/Customer?
- Official order state machine?
- Inventory reserve/commit/release policy and timeouts?
- Payment provider and reconciliation workflow?
