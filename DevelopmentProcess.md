# Development Process

This process is aligned with the current requirements, service boundaries,
and tech stack. See:
- `ServiceDuty.md` for domain flows and service responsibilities.
- `TechStack.md` for stack choices and rationale.

---

## 1. Requirements Alignment

- Read `docs/requirements/1-requirement.adoc` and confirm:
  - Storefront (React) + Backoffice (Angular)
  - .NET microservices behind an API Gateway
  - Identity: SSO + MFA
  - Core flows: Products, Orders, Users, Inventory

If a requirement is unclear, add it to the **Open Questions** section in
`ServiceDuty.md` before implementation.

---

## 2. Service Boundary Decision

Use `ServiceDuty.md` to decide which service owns:
- Data ownership and write responsibility
- Workflow status transitions
- Error handling and fallback behavior

Do not implement cross-domain rules inside the wrong service.

---

## 3. Design the Flow (Before Coding)

For each feature:
- Identify the **primary service**.
- Map the flow across services (Order ↔ Inventory ↔ Payment).
- Decide what happens when a dependency is down.

Document changes in `ServiceDuty.md` if the flow is new or updated.

---

## 4. Implementation Rules

- Follow the tech stack conventions in `TechStack.md`.
- Keep business rules inside domain services.
- Avoid duplicating server state in frontends.
- Use TypeScript for frontend code and keep types strict.

---

## 5. Local Validation

Before opening a PR:
- Run the affected service locally.
- Verify the end-to-end flow:
  - Draft → Approve/Reject
  - Delivery + evidence
  - Payment (prepaid/COD/bank transfer)
  - Inventory reserve/release

Use `HOW_TO_RUN.md` for commands.

---

## 6. Testing Strategy

Minimum expectations:
- Unit tests for core business rules
- Integration tests for service boundaries (API + data)
- Frontend happy-path + critical edge cases

Avoid large test suites that do not map to real risks.

---

## 7. PR Readiness

- Keep PRs small and goal-focused.
- Update docs when flows or service responsibilities change.
- Follow the correct PR review checklist for the project stack:
  - React SPA → `docs/code-review/[PR] React code review checklist.md`
  - Angular SPA → `docs/code-review/[PR] Angular code review checklist.md`
  - .NET backend → `docs/code-review/[PR] .NET code review checklist.md`

---

## 8. CI/CD Expectations

- Each service has its own workflow in `.github/workflows/`.
- CI should only run for changed service paths.
- Do not bypass CI for production changes.

---

## 9. Release & Handoff

- Ensure documentation reflects what was shipped.
- List operational risks (dependency outages, payment failures, inventory drift).
- Provide validation steps for stakeholders.
