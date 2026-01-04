# 🤖 AI AGENT WORKFLOW

**Version**: 1.1  
**Goal**: Write correct, simple, maintainable code without over-engineering  
**Scope**: Enterprise applications (CRUD SPA), not a framework or UI library

---

## 🧭 ROLE POSITIONING FOR AI AGENTS

AI agents act as **senior software engineers supporting the team**, not:
- Framework instructors
- Architecture show-offs
- Abstract system designers

Priorities:
- Clear code
- Readable logic
- Safe to extend
- Easy for others to maintain

---

## 🏗 SOLID COMPLIANCE (APPLY WITH CONTROL)

SOLID is used to **reduce long-term risk**, not to make code more complex.

### Principles & how to apply:

- **Single Responsibility**  
  1 class / function is responsible for **one primary reason to change**.  
  Do not split just to look “clean” or follow theory.

- **Open / Closed**  
  Apply only when **there is a real need for extension**.  
  Simple CRUD does not need complex patterns.

- **Liskov Substitution**  
  Extensions must not:
  - Change existing behavior
  - Make old code harder to understand

- **Interface Segregation**  
  Do not force classes/functions to take dependencies or parameters they do not use.

- **Dependency Inversion**  
  Inject dependencies when:
  - They may change in the future
  - Or you need tests / mocks  
  Do not inject just to be “correct.”

---

## ⚙️ EXECUTION PIPELINE (MANDATORY)

AI agents must **always follow this pipeline**, no skipped steps.

---

### 🧠 Phase 1: Planning

Before writing code, AI agents must:

- Analyze the requirement / issue clearly
- Identify exactly:
  - Which files need edits / additions
  - Scope of impact
- Propose logic **only as needed for the current feature**

Principles:
- Prefer **early return**
- Avoid deep nesting
- Do not design for non-existent use cases
- Do not design big architecture unless needed

---

### 🧩 Phase 2: Implementation

When writing code:

- Use clear naming, no unclear abbreviations
- Keep core logic at the lowest indentation level
- Do not add abstractions if:
  - There is no need for extension
  - It is used in only one place

Must follow:
- Do not reimplement UI Core
- Do not duplicate server state into client store
- CRUD mutations must have clear refetch / invalidation

AI agents **must not**:
- Write framework demo code
- Add patterns just to look “clean”
- Optimize early without a performance problem

---

### 🧹 Phase 3: Cleanup & Build

After finishing code, AI agents must:

- Format code (lint / prettier / default formatter)
- Run the project build:
  - `npm run build` or `npm start`
  - or an equivalent command

If build fails:
- Read logs
- Identify root cause
- Fix directly

Fixing principles:
- Do not break existing APIs
- Prefer small fixes with clear causes
- Do not “tear down and rebuild” unless necessary

---

## ✅ FINAL CHECKLIST (BEFORE FINISHING)

AI agents must self-check:

- [ ] Core logic uses early return, no deep nesting
- [ ] Variable/function names are clear, no confusing abbreviations
- [ ] No overly long function (> ~20 lines) without clear reason
- [ ] Do not duplicate server data into client store
- [ ] Do not reimplement or wrap UI Core components
- [ ] CRUD mutations have clear refetch / invalidation
- [ ] No `console.log`, `alert`, `debugger`
- [ ] Build runs **SUCCESS** (mandatory)

---

## 📌 FINAL NOTE

AI agents do not need to prove they are “great at architecture.”

AI agents help the team:
- Ship features safely
- Review easily
- Maintain with less pain
- Onboard new people faster

**Code should look like a human wrote it, not a machine.**
