# React PR Review Checklist (React 19 + Tailwind + TanStack Router + Zustand + RHF/Zod + React Query + MSAL)

Pragmatic checklist for reviewing React PRs in a modern SPA codebase.  
Focus: correctness, UX, performance, security, and maintainability. Patterns are optional; outcomes are required.

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
- [ ] Files are placed in the correct feature/module boundary (feature-first organization preferred).
- [ ] Public exports are intentional (avoid “export everything” barrels that increase coupling).
- [ ] Shared code belongs in shared/ui-core/utils only when it is truly cross-feature.

---

## 3. Routing (TanStack Router — file-based)
- [ ] Route file naming and nesting follow the repo convention (layout routes, index routes, params).
- [ ] Route loaders/actions are used appropriately (avoid fetching in random components when router data is the source of truth).
- [ ] Protected routes are enforced consistently (auth gate belongs in router/guard layer, not sprinkled in components).
- [ ] Navigation and deep-linking work (back/forward, refresh, direct URL entry).

---

## 4. Data Fetching & Server State (React Query)
- [ ] Query keys are stable and correctly scoped (include parameters that affect results).
- [ ] Cache invalidation is correct and minimal (invalidate only what must change).
- [ ] Loading, error, and empty states are handled and consistent with UI-core patterns.
- [ ] Mutations use optimistic updates only when safe; rollback paths are correct.
- [ ] Requests are cancelable where possible (abort signals) and do not race into stale UI.

---

## 5. Client State (Zustand)
- [ ] Zustand stores contain **UI/client state** only; server state stays in React Query.
- [ ] Store shape is minimal; actions are explicit; no “god store”.
- [ ] Selectors are used to minimize re-renders; avoid subscribing to whole store.
- [ ] Persistence (if used) is intentional, versioned, and safe (no sensitive data stored).

---

## 6. Forms & Validation (React Hook Form + Zod)
- [ ] Form schema is defined with Zod; validation messages are user-friendly and consistent.
- [ ] Default values are explicit; controlled/uncontrolled inputs are not mixed incorrectly.
- [ ] Async validation and submit handling are robust (disable submit, prevent double-submit, show progress).
- [ ] Error mapping from server → form fields is correct and localized to the right fields.
- [ ] Accessibility: inputs have associated labels; error text is announced (aria-describedby / role).

---

## 7. Authentication & Authorization (MSAL SSO)
- [ ] MSAL integration follows a single standard entrypoint (provider/hook/service), not duplicated.
- [ ] Token acquisition uses the correct flow (silent first; interactive fallback) and handles errors.
- [ ] Tokens are not stored insecurely (no localStorage for access tokens unless explicitly approved).
- [ ] API calls attach auth headers via a single interceptor/fetch wrapper; no manual header scattering.
- [ ] Authorization checks (roles/claims) are enforced consistently in routing and critical UI actions.
- [ ] Logout behavior is correct (session cleanup, redirect, cache clearing as required).

---

## 8. UI Consistency (Tailwind + UI-Core)
- [ ] Uses UI-core components where available; avoids re-implementing existing patterns.
- [ ] Tailwind usage follows conventions (utility order, responsive breakpoints, design tokens).
- [ ] Layouts are reusable and avoid duplication (page shell / section templates).
- [ ] No hardcoded colors/spacings that violate design system (unless explicitly approved).

---

## 9. Accessibility (A11y) — Required
- [ ] Keyboard navigation works (tab order, focus visible, no focus traps unless intended).
- [ ] Correct semantic elements are used (button vs div, headings, landmarks).
- [ ] Modals/menus/tooltips: focus management, escape to close, aria roles/labels correct.
- [ ] Images/icons have appropriate accessible names (alt text / aria-label).
- [ ] Dynamic updates are announced when necessary (aria-live) and do not cause focus loss.

---

## 10. Performance & Rendering Behavior
- [ ] Avoids unnecessary re-renders (stable props, memo only where it helps, correct dependency arrays).
- [ ] Expensive computations are avoided or scoped (virtualization for long lists when needed).
- [ ] Code-splitting is used for heavy routes/features (router-based chunking).
- [ ] No large synchronous work on the main thread during interactions.
- [ ] Bundle impact is considered for new deps (tree-shaking, duplicate libs, large icons).

---

## 11. Error Handling & UX Robustness
- [ ] Error boundaries are used for feature-level crash containment where appropriate.
- [ ] User-visible error states are actionable (retry, guidance); no raw stack traces.
- [ ] Edge cases: empty data, partial failure, slow network.

---

## 12. Security & Privacy (Frontend)
- [ ] No secrets/credentials committed; no sensitive data logged.
- [ ] User-generated content is rendered safely (no `dangerouslySetInnerHTML` without sanitization + justification).
- [ ] External links use safe target/rel settings; URL building avoids open redirect.
- [ ] PII is not stored in client state or persisted storage unless required and approved.

---

## 13. Tooling & Code Quality
- [ ] Linting/formatting passes (ESLint/Prettier) with no new warnings.
- [ ] No stray `console.*`, commented-out code, or debug toggles left on.
- [ ] Types are correct (no `any` sprawl); runtime checks exist when data is untrusted.
- [ ] Public utilities/hooks are documented with clear contracts.

---

## 14. Testing (Right-Sized)
- [ ] Core logic has unit tests (Vitest/Jest) where risk warrants it.
- [ ] Components have RTL tests for critical flows (happy path + key edge case).
- [ ] Router/data tests cover loaders/actions where applicable.
- [ ] Tests are deterministic (stable mocks; avoid real network).

---

## Merge Exit Criteria (Team Rule)
- [ ] CI is green; tests cover risk areas.
- [ ] Auth, data, and routing behavior are verified for impacted flows.
- [ ] A11y baseline checks pass for changed UI.
- [ ] UX is acceptable on slow network / low-end device scenario when relevant.

---

<details>
  <summary><strong>Reviewer Notes (Optional)</strong></summary>

- **Blockers:** auth/security regressions, broken routing/data flows, a11y failures on core paths, major perf regressions, inconsistent UI-core usage that creates fragmentation.
- **Non-blocking:** minor styling nits, small refactor suggestions (unless they prevent maintainability).

</details>
