# Frontend Common Standard

**Version**: 2.0

## Authority

This document is the single source of truth for frontend standards.
Personal preferences do not override these rules.
Deviations require explicit technical justification and reviewer approval.

## Scope

- Applies to all frontend code and AI agents in this repository.
- Covers architecture, TypeScript usage, styling, and execution workflow.
- These rules are enforceable during code review.

## Enforceability & Review

- Violations without justification are review failures.
- AI agents must follow the same rules as human engineers.
- Any exception must be documented in the PR description with reviewer approval.

## Allowed / Forbidden / Requires Justification

**Allowed**
- Feature-based organization for application code.
- Framework-specific implementations when they obey these standards.
- Plain CSS or CSS Modules by default.

**Forbidden**
- `any` in TypeScript.
- Cross-feature imports without explicit approval.
- UI or rendering layers containing business logic.
- Mixing multiple styling systems in one project.
- Duplicating server data into client state.
- Framework demo code or architecture for show.

**Requires justification**
- Unsafe type assertions (`as`).
- SCSS or CSS-in-JS usage.
- Dependency inversion when no change or testing need exists.

## TypeScript Coding Standard

TypeScript is a strict contract system, not optional typing.

Rules:
- TypeScript is mandatory for all frontend code.
- `any` is forbidden.
- `unknown` is allowed only at system boundaries and must be narrowed.
- Unsafe `as` assertions are forbidden unless justified.
- API request/response types must be centralized.
- External data must be validated at boundaries (API, forms).

References:
- https://www.typescriptlang.org/docs/handbook/
- https://effectivetypescript.com/

## Frontend Architecture Concepts

Rules:
- Business logic must be organized by feature, not technical modules.
- Feature-based organization is mandatory for application code.
- Cross-feature imports are forbidden unless explicitly approved.
- Technical module-based organization is allowed only for shared or infrastructure code.
- Features must not call HTTP or depend on concrete service implementations.
- All external interactions must go through interfaces.
- Services implement interfaces and are wired in the app bootstrap.

References:
- https://martinfowler.com/articles/modularization.html
- https://web.dev/
- https://developer.mozilla.org/

## Styling & CSS Standards

Rules:
- Styling systems must be consistent per project.
- Allowed approaches (in order of preference):
  1. Plain CSS / CSS Modules
  2. SCSS (only if justified)
  3. CSS-in-JS (only if framework-required)
- Mixing multiple styling systems in one project is forbidden.
- CSS must be organized by responsibility (base, layout, components, utilities).
- Responsive design must be mobile-first.
- Breakpoints must be centralized, not hardcoded.

References:
- https://cssguidelin.es/
- https://every-layout.dev/

## SOLID Application (Controlled, Not Dogmatic)

Rules:
- SOLID exists to reduce long-term risk, not to increase abstraction.
- No abstraction without a concrete need.
- Dependency inversion is required only when change or testing is expected.
- CRUD flows do not require complex patterns by default.

Violating SOLID principles without justification is a review failure.

## UI Core Usage

Rules:
- Use UI Core components as the primary UI system.
- Do not wrap, fork, or copy UI Core components.
- If a component is missing, request it from the UI Core team.

## Data & State

Rules:
- Server data must remain in the server-state layer and be refetched after mutations.
- Client state must not duplicate server data.
- CRUD mutations must trigger a refetch or invalidation.

## AI Agent Execution Workflow

### Phase 1: Planning

- Identify the exact files to change.
- Keep scope minimal and aligned to the request.
- Prefer early return and low nesting.
- Do not design for non-existent use cases.

### Phase 2: Implementation

- Use clear naming; no unclear abbreviations.
- Keep logic at the lowest indentation level.
- Avoid abstractions used in only one place.
- Do not reimplement UI Core.
- Do not duplicate server state into client state.
- CRUD mutations must have clear refetch or invalidation.

### Phase 3: Cleanup & Build

- Format code with the project formatter.
- Run the build (`npm run build` or equivalent).
- If the build fails, fix the root cause immediately.

## Final Checklist (Mandatory)

- [ ] Logic uses early return; no deep nesting.
- [ ] Naming is clear; no confusing abbreviations.
- [ ] No function exceeds ~20 lines without justification.
- [ ] No `console.log`, `alert`, or `debugger`.
- [ ] No duplication of server data in client state.
- [ ] CRUD mutations refetch or invalidate data.
- [ ] TypeScript has no errors.
- [ ] Build succeeds.
