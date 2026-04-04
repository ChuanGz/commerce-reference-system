# Development Process

This document defines the expected delivery workflow for the repository.

## 1. Start From Requirements

- Read [`requirements/requirements.md`](requirements/requirements.md).
- Confirm whether the change affects storefront, backoffice, backend services, or shared contracts.
- If the requirement is unclear, write the open question before coding.

## 2. Confirm Ownership

Use [`service-map.md`](service-map.md) to decide:

- which service owns the data
- which service owns the workflow decision
- which service reacts to cross-service events

## 3. Design Before Coding

For each feature:

- identify the primary service or frontend application
- list dependent services
- define the happy path
- define the failure path

## 4. Implementation Rules

- Keep business rules inside the owning service.
- Avoid duplicating server state in frontend code.
- Prefer simple service boundaries over clever abstractions.
- Keep naming direct and domain-specific.

## 5. Validate Locally

Before opening a PR:

- run the affected service or frontend locally
- validate the changed flow end to end
- update documentation when behavior or ownership changes

Use [`run-local.md`](run-local.md) for commands.

## 6. Testing Expectations

- unit tests for business rules
- integration tests for service boundaries
- frontend tests for critical paths

Avoid adding large test suites that do not map to real risks.

## 7. PR Readiness

- keep PRs small and scoped
- update the right docs with the same change
- use the matching review checklist:
  - React: `docs/code-review/[PR] React code review checklist.md`
  - Angular: `docs/code-review/[PR] Angular code review checklist.md`
  - .NET: `docs/code-review/[PR] .NET code review checklist.md`

## 8. CI Expectations

- workflows are service-specific
- path filters must stay aligned with folder structure
- do not bypass CI for production-bound changes

## 9. Release Handoff

- document what changed
- list operational risks
- give validation steps to stakeholders
