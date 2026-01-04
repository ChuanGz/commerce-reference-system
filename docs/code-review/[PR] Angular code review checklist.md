# Angular PR Review Checklist (Angular 17/18+ — forward-compatible)

Pragmatic checklist for reviewing Angular PRs in a modern frontend codebase.  
Focus: correctness, UX, performance, security, accessibility, and maintainability. Patterns are optional; outcomes are required.

> Tailor the “state” section to your stack (NgRx / Signals / RxJS-first).  
> If a guideline is intentionally broken, document the reason in the PR description.

---

## How to use
- Reviewers: focus on items relevant to the PR’s risk (auth/data/perf/a11y).
- Authors: self-check before requesting review.
- If a guideline is intentionally broken, document the reason in the PR description.

---

## PR Summary (Author)
- **What changed?**
- **Why now?**
- **Risk / Impact:** Routing | Data | Auth | UI | Perf | A11y
- **How to validate locally:** steps + any feature flags

---

## 1. Change Clarity
- [ ] PR scope is focused; unrelated refactors are split or justified.
- [ ] Screenshots/video included for UI changes (before/after if meaningful).
- [ ] User-facing changes and any breaking changes are documented.

---

## 2. Project Structure & Ownership
- [ ] Code is placed within the correct feature boundary (feature-first preferred).
- [ ] Shared code is truly shared (avoid dumping feature logic into `shared/`).
- [ ] Public exports are intentional (avoid barrels that create hidden coupling).
- [ ] Standalone architecture usage is consistent (standalone components, functional providers, etc.).

---

## 3. Routing (Angular Router)
- [ ] Lazy loading used for feature areas where appropriate (`loadChildren`, standalone routes).
- [ ] Public vs authenticated routes are clearly separated.
- [ ] Guards are used consistently for auth/role checks (or route-level checks in a single standard).
- [ ] Route resolvers/data loading strategy is consistent (resolver vs component fetch vs store).
- [ ] Deep-linking works: refresh, direct URL entry, back/forward navigation.

---

## 4. Data Fetching & Server State (HttpClient / Store / Signals)
- [ ] HTTP calls are centralized in services; components do not own low-level HTTP concerns.
- [ ] Error, loading, and empty states are handled consistently.
- [ ] Cancellation is considered where relevant (unsubscribe, `takeUntilDestroyed`, abort patterns).
- [ ] Retry/backoff used only when appropriate (avoid retry storms).
- [ ] Caching strategy is explicit (store cache, memoization, HTTP caching, etc.).

---

## 5. State Management (NgRx / Signals / RxJS-first)
- [ ] State split is clear: UI state vs server state vs derived state.
- [ ] Immutable update discipline is maintained (NgRx reducers) or state updates are predictable (Signals store).
- [ ] Selectors/derived computations are memoized / efficient.
- [ ] Side effects are isolated (NgRx Effects / services) and are testable.
- [ ] Avoid “god store” / “global everything” state — keep state local to feature where possible.

---

## 6. Forms & Validation (Reactive Forms preferred)
- [ ] Form model is clear and typed where possible; default values are explicit.
- [ ] Validators match business rules; errors are shown in a user-friendly way.
- [ ] Submit handling is robust (disable during submit, prevent double-submit, show progress).
- [ ] Server validation errors map to the correct fields.
- [ ] Accessibility: labels, error announcements, and keyboard interactions are correct.

---

## 7. Authentication & Authorization
- [ ] Auth integration follows one standard (service + interceptor + guard), not duplicated logic.
- [ ] Token attachment is centralized via an `HttpInterceptor`.
- [ ] Token refresh logic is safe (single-flight refresh, queue pending requests, handles 401/403 correctly).
- [ ] Sensitive data is not persisted insecurely (avoid `localStorage` for access tokens unless approved).
- [ ] Authorization checks are enforced consistently in routing and critical UI actions.

---

## 8. UI Consistency & Design System
- [ ] Uses the agreed UI library/design system; avoids re-implementing existing components.
- [ ] Styling follows conventions (CSS/Tailwind/Material tokens), avoids magic values.
- [ ] Layout patterns are reusable (page shells, shared layouts) and avoid duplication.
- [ ] Responsive behavior is verified where relevant.

---

## 9. Accessibility (A11y) — Required
- [ ] Keyboard navigation works (tab order, focus visible, no unintended traps).
- [ ] Correct semantics (buttons/links/headings/landmarks) are used.
- [ ] Dialogs/menus/tooltips: focus management, escape to close, aria roles/labels correct.
- [ ] Images/icons have accessible names (alt text / aria-label).
- [ ] Live updates announce appropriately when needed (aria-live) without stealing focus.

---

## 10. Performance & Change Detection
- [ ] ChangeDetectionStrategy is chosen intentionally (OnPush where appropriate).
- [ ] Avoids unnecessary subscriptions; uses `async` pipe; cleanup via `takeUntilDestroyed`.
- [ ] Large lists use virtualization where needed; avoids expensive template computations.
- [ ] Lazy loading is used for heavy routes/features.
- [ ] Bundle impact considered for new deps (duplicate libs, heavy icon packs, non-tree-shakeable modules).

---

## 11. Error Handling & UX Robustness
- [ ] User-visible error states are actionable (retry, guidance); no raw stack traces.
- [ ] Global error handler/logging is used where applicable.
- [ ] Edge cases handled: empty data, partial failure, slow network, offline-ish behavior (if applicable).

---

## 12. Security & Privacy (Frontend)
- [ ] No secrets/credentials committed; no sensitive data logged.
- [ ] User-generated content is rendered safely (avoid bypassing sanitization; justify if needed).
- [ ] External links use safe target/rel settings; URL handling avoids open redirect.
- [ ] PII is not stored in client state or persisted storage unless required and approved.

---

## 13. Tooling & Code Quality
- [ ] Linting/formatting passes (ESLint/Prettier) with no new warnings.
- [ ] No stray `console.*`, commented-out code, or debug toggles left on.
- [ ] Types are correct; avoid `any` sprawl; runtime guards exist for untrusted data.
- [ ] Public utilities/components are documented with clear contracts and inputs/outputs.

---

## 14. Testing (Right-Sized)
- [ ] Core logic has unit tests (Jest/Karma) where risk warrants it.
- [ ] Components have tests for critical flows (happy path + key edge case).
- [ ] Store/effects tests exist for critical state transitions and side effects.
- [ ] Tests are deterministic (stable mocks, no real network, avoid timing flakiness).
- [ ] Manual QA steps are included when changes are hard to automate.

---

## Merge Exit Criteria (Team Rule)
- [ ] CI is green; tests cover risk areas.
- [ ] Auth, data, and routing behavior are verified for impacted flows.
- [ ] A11y baseline checks pass for changed UI.
- [ ] UX is acceptable on slow network / low-end device scenario when relevant.

---

<details>
  <summary><strong>Reviewer Notes (Optional)</strong></summary>

- **Blockers:** auth/security regressions, broken routing/data flows, a11y failures on core paths, major perf regressions, design system fragmentation.
- **Non-blocking:** minor styling nits, small refactor suggestions (unless they prevent maintainability).

</details>
