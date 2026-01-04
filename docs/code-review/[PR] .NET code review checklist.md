# .NET PR Review Checklist (.NET 8 — forward-compatible to .NET 10)

Pragmatic checklist for reviewing pull requests in a .NET backend team.  
Patterns (Clean Architecture / CQRS / MediatR / Repository) are **optional** — correctness, operability, and maintainability are required.

---

## How to use
- Reviewers: check items relevant to the PR’s risk profile (API/data/security/perf).
- Authors: self-check before requesting review.
- If a guideline is intentionally broken, **document the reason** in the PR description.

---

## PR Summary (Author)
- **What changed?**
- **Why now?**
- **Risk / Impact:** API | Data | Perf | Security | Ops
- **How to validate locally:** commands / steps

---

## 1. Change Clarity
- [ ] PR description states **what** changed, **why**, and **impact/risk** (API | Data | Perf | Security | Ops).
- [ ] Scope is focused; unrelated refactors are separated or explicitly justified.
- [ ] Public-facing changes are listed: endpoints/contracts/events/config flags (if any).

---

## 2. Functional Correctness
- [ ] Business rules match requirements; edge cases covered (null/empty, ranges, timezone, concurrency).
- [ ] Failure paths are correct; no silent success.
- [ ] Idempotency considered for retryable operations (webhooks, payments, async triggers).
- [ ] Data invariants preserved (no partial updates without intent).

---

## 3. API & Contract Quality (HTTP / gRPC / Messaging)
- [ ] Status codes and semantics are correct (200/201/204/400/401/403/404/409/422/500).
- [ ] Contract changes are stable; breaking changes are versioned or communicated.
- [ ] Error format is consistent (ProblemDetails or team standard), includes correlation/trace id if used.
- [ ] Pagination/filter/sort follow team conventions and are validated.

---

## 4. Security & Privacy Baseline
- [ ] AuthN/AuthZ applied correctly (policy-based preferred); no “forgotten” endpoints.
- [ ] Input validation exists; no injection risk (raw SQL must be parameterized).
- [ ] Secrets/tokens/keys are not logged or committed; PII is redacted in logs.
- [ ] File upload/download is safe (size/type/path traversal protection, storage isolation).

---

## 5. Error Handling & Resilience
- [ ] Centralized exception handling maps errors to expected responses.
- [ ] External calls (HTTP/DB/broker) handle timeouts; retry/backoff used where appropriate.
- [ ] No catch-all swallowing; expected exceptions handled explicitly.
- [ ] Error details are useful to clients without leaking internals.

---

## 6. Observability (Production-readiness)
- [ ] Structured logging (message templates); log levels used correctly.
- [ ] Correlation/trace id preserved across boundaries (request → dependencies).
- [ ] Important events are logged (auth failures, critical writes/deletes, integration failures).
- [ ] Metrics/tracing exist for critical paths when applicable (latency, error rate, dependency latency).

---

## 7. Data Access & Persistence (EF Core / SQL)
- [ ] Queries are efficient: projection is used; `Include` is intentional; no N+1 in loops.
- [ ] Read-only queries use `AsNoTracking()` (or team convention).
- [ ] Transaction boundaries are clear; multi-step writes are atomic where required.
- [ ] Concurrency strategy is correct (RowVersion/ETag/unique constraints as needed).
- [ ] Index/migration impact considered for new queries or columns.

> Note: “DbContext only in repositories” is not required. What matters is controlled persistence concerns, correctness, and testability.

---

## 8. Async, Cancellation, and Resource Use
- [ ] I/O is async end-to-end; no `.Result`/`.Wait()`.
- [ ] `CancellationToken` flows from entrypoints down to async calls.
- [ ] No `Task.Run` in request path unless justified.
- [ ] No large in-memory materialization for big datasets without need.

---

## 9. Code Quality & Maintainability
- [ ] Code is readable; naming is consistent with team conventions.
- [ ] Responsibilities are clear; no “god service” growth.
- [ ] No magic strings/numbers; use constants/enums/options.
- [ ] Duplication reduced only when it improves clarity (avoid premature abstraction).

---

## 10. Boundaries & Dependencies (Architecture is Guideline)
- [ ] Core/business logic does not depend on infrastructure details without intent.
- [ ] Shared library usage is safe (no cross-service business coupling).
- [ ] If a guideline is broken, the PR explains **why** and preserves testability.

---

## 11. DTOs, Mapping, and Serialization
- [ ] Entities are not exposed directly over the wire (API uses DTOs/ViewModels).
- [ ] Mapping is explicit and debuggable; AutoMapper (if used) is controlled.
- [ ] JSON settings are consistent (date/time handling, enum representation, casing).
- [ ] No circular reference / oversized payload risk.

---

## 12. Configuration, Environments, and Deployment Readiness
- [ ] Options are validated on startup where appropriate (`ValidateOnStart`).
- [ ] Environment-specific config works (Dev/Staging/Prod); no hardcoded environment logic.
- [ ] Health checks cover critical dependencies when applicable (DB, broker, downstream services).
- [ ] Docker/CI changes are verified (build, migrations, startup).

---

## 13. Testing (Right-Sized)
- [ ] New/changed business logic has unit tests or equivalent coverage.
- [ ] Integration tests exist for critical persistence/integration behaviors (minimal but meaningful).
- [ ] Tests are deterministic (no order dependence, stable time/data).
- [ ] PR includes how to validate locally when non-trivial.

---

## Merge Exit Criteria (Team Rule)
- [ ] CI is green; tests cover risk areas; no known security/privacy issue.
- [ ] The change is observable in production (logs/trace/correlation for key flows).
- [ ] Data/contract changes are documented and communicated.

---

<details>
  <summary><strong>Optional: Reviewer Notes</strong></summary>

- **Blockers:** correctness, security/privacy, data integrity, major perf regressions, broken contracts, missing observability for critical flows.
- **Non-blocking:** naming nits, formatting, minor refactor suggestions (unless they prevent maintainability).

</details>
