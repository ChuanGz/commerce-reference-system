# AI Frontend Application Guide
*(Enterprise CRUD SPA – Internal AI Prompt)*

This document is used as:
- An **internal AI prompt** for all AI agents
- A **standard coding guide** for the frontend team

Goals:
- Build a **frontend application** (not a UI library)
- Reuse the existing **UI Core**
- Focus on **CRUD** and **API integration**
- Keep code **readable, maintainable, and easy to onboard new developers**

---

## 1. Scope & Mindset

- This project is a **pure SPA** (API-backed CRUD)
- We are **not building a UI system**
- Avoid unnecessary complexity and “architecture for architecture’s sake”
- Code must be easy for newcomers to understand and easy for future maintainers to change

Always ask:
> “If someone else edits this file 6 months from now, will they understand it quickly?”

---

## 2. UI Core — Correct Usage

- Use **UI Core only**
- **Do not wrap, fork, or copy** UI Core logic
- If a component is missing → **request it from the UI Core team**
- The application **must not** take ownership of the UI system

---

## 3. Data & State — React Query + Zustand

### 3.1 Responsibilities

**React Query**
- Manages **server state**
- Fetches data from APIs
- Caching, refetching, invalidation

**Zustand**
- Manages **client/UI state**, e.g.:
  - Page-level UI mode
  - Feature flags
  - Local view preferences (outside UI Core components)

For DataGrid, state such as filters, pagination, sorting, and selected rows
is already handled inside UI Core (via its hooks/context). Use the UI Core
custom hook as-is; do not reimplement or mirror that state in Zustand.

Do not mix responsibilities.  
Do not use Zustand as the primary source for server data fetching.  
If data comes from API responses, keep it in React Query and derive UI state from it.

---

### 3.2 React Query usage

- Use React Query data **directly**
- After mutations → **invalidate** or **refetch**
- Avoid copying server data into Zustand

If you need derived data, compute it in selectors or memoized helpers instead of storing a second copy.

Objectives:
- Prevent stale data
- Avoid “double source of truth”
- Improve maintainability

---

### 3.3 CRUD refetch strategy

- Create → refetch list
- Update → refetch list and/or detail
- Delete → refetch list
- Reload page → data must still be correct

Prefer data correctness over premature optimization.

---

## 4. DataGrid / Table

- Use the UI Core **DataGrid**
- Prefer **server-side** pagination, sorting, and filtering

**State management**
- React Query: data
- UI Core hooks/context: filters, pagination, sorting, selection

**Performance**
- Memoize column definitions
- Avoid inline functions inside render
- Prevent unnecessary table re-renders

---

## 5. Forms & Validation

### 5.1 Form definition

- Use `useForm`
- Strong typing
- Declare only the fields you actually use
- Always provide `defaultValues`

---

### 5.2 Validation

- Use a **Zod schema** + resolver
- The schema is the **single source of truth**
- Do not scatter manual validation logic

---

### 5.3 Behavior & performance

- `isValid` and `isDirty` should be tracked in real-time as needed
- Never submit when invalid
- Avoid watching the entire form (`watch()` everything)
- For large forms → split into smaller components

---

## 6. Routing

- Use **TanStack Router**
- Do not call router APIs directly everywhere
- Route through a small abstraction (e.g., `useAppRouter`) to reduce lock-in

**URL query params**
- Use for filters, sorting, pagination
- Do not put sensitive data in URLs
- Anything in the URL is considered **public**

---

## 7. Feature Flags

- Feature flags must be supported
- Feature flags are **configuration**, not business logic
- Features should be easy to toggle and test independently

---

## 8. Pre-Push Checklist (Before Opening a PR)

Treat this checklist as **mandatory** before creating a PR or pushing to production.

### Functionality
- CRUD works correctly
- Update/Delete triggers refetch
- Reloading the page does not show stale data

### Data & State
- React Query is used for the correct responsibilities
- Server data is not copied into Zustand

### Forms
- `defaultValues` exist
- Validation is correct
- No unnecessary re-renders

### Tables
- Pagination/filter/sort behave correctly
- Performance is acceptable with large datasets

### Routing
- Query params do not leak sensitive information

### Clean code & tooling
- No `console.log`
- No `alert`
- No `debugger`
- Code is readable; no overengineering
- TypeScript has no errors
- Lint/format checks pass

### PR review checklist selection
Before pushing a PR, identify the project stack and complete **all** items in the
matching checklist:
- React SPA → `docs/code-review/[PR] React code review checklist.md`
- Angular SPA → `docs/code-review/[PR] Angular code review checklist.md`
- .NET backend → `docs/code-review/[PR] .NET code review checklist.md`

If the project type is unclear, confirm with the tech lead before opening the PR.

---

## 9. Long-Term Maintainability Rules

### 9.1 TypeScript discipline

- Use TypeScript for all frontend code
- Avoid `any`; prefer `unknown` and narrow it
- Do not silence type errors with unsafe `as` unless justified
- Keep API response/request types in one place (shared types module)
- Validate external data at boundaries (API or form schemas)

---

### 9.2 Code organization for large teams

- Feature-based folders; keep feature internals private
- Only expose feature public APIs via a single entry point
- Avoid cross-feature imports that bypass public APIs
- Keep components small; split when a file becomes hard to scan
- Name things clearly; no abbreviations that require team context

---

### 9.3 Consistency & collaboration

- One canonical pattern for API calls and mutation handling
- One canonical pattern for form setup and validation
- Keep PRs small; avoid mixing refactors with feature work
- Add brief docs for any non-obvious flow or convention

---

## Conclusion

This guide is a baseline, not a rigid law.

Breaking it is acceptable with a valid technical reason.

Keep code clean because you respect future maintainers — not because of rules.
