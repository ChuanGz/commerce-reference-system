# Authorization Policies

This backend uses **JWT Bearer** with a deny-by-default policy.
All business endpoints require authentication unless explicitly allowlisted.

## Scope Model

We use **scopes** via the `scp` (or `scope`) claim.
Tokens should include the required scopes for each service.

Pattern:
- `<ServiceName>.read`
- `<ServiceName>.write`

Examples:
- `OrderService.read`
- `OrderService.write`

## Service Policies (Standard)

| Service | Read Policy | Write Policy |
| --- | --- | --- |
| Customer | `CustomerService.Read` | `CustomerService.Write` |
| Inventory | `InventoryService.Read` | `InventoryService.Write` |
| Order | `OrderService.Read` | `OrderService.Write` |
| Payment | `PaymentService.Read` | `PaymentService.Write` |
| Product | `ProductService.Read` | `ProductService.Write` |
| User | `UserService.Read` | `UserService.Write` |

## Identity Service Policies

Identity uses fine-grained policies based on permission scopes defined in
`IdentityService.Application.Constants.AuthorizationPolicies`.

Examples:
- `CanViewRole` → requires scope `CAN_VIEW_ROLE`
- `CanEditRole` → requires scope `CAN_EDIT_ROLE`

If your IdP uses roles instead of scopes, update the policy requirement
accordingly.
