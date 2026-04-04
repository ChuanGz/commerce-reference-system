# Service Map

This document explains service ownership, system flows, and failure boundaries.

## Why The Repository Uses This Split

- each service owns its data and business rules
- order, inventory, and payment can evolve independently
- identity stays isolated as a security boundary

## Core Flows

### Draft To Approval

1. storefront creates an order draft
2. order-service stores the draft
3. inventory-service reserves stock
4. backoffice approves or rejects the order
5. rejected orders release reserved stock

### Delivery

1. approved order moves to delivery
2. order-service records delivery progress
3. completed delivery closes the fulfillment path

### Payment

1. payment-service creates or confirms payment activity
2. order-service reacts to the resulting payment state
3. payment outcomes affect final order completion

## Service Ownership

### identity-service

- owns authentication, authorization, token issuance, and identity integration
- affects every client-facing flow

### user-service

- owns internal users, roles, and user administration
- supports backoffice access and operational user management

### customer-service

- owns customer profile data used by order flows
- supports delivery and customer-specific order context

### product-service

- owns product catalog, pricing, and product metadata
- supports storefront browsing and order line creation

### inventory-service

- owns stock, reservations, and availability calculations
- prevents oversell and supports order lifecycle transitions

### order-service

- owns the order lifecycle
- orchestrates customer, product, inventory, and payment dependencies

### payment-service

- owns payment workflow integration and payment status handling
- supports prepaid, COD, and transfer-style flows

## Failure Boundaries

- identity-service failure blocks login and token refresh
- product-service failure blocks catalog browsing and order creation
- inventory-service failure blocks reservation and availability checks
- order-service failure blocks the core transaction path
- payment-service failure blocks payment confirmation and some completion paths

## Open Design Questions

- what is the exact order state machine
- what is the reservation timeout policy
- what level of payment abstraction is required
- where should role ownership live between identity and user-service
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
