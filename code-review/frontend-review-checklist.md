
# ✅ Frontend Code Review Checklist (React/Angular)

## 1. Project Structure
- [ ] Clear folder structure (feature-based preferred)
- [ ] Separation of concerns: core, shared, features, services
- [ ] Proper use of index.ts for module exports

## 2. Routing
- [ ] Uses lazy loading (Angular) or dynamic import (React)
- [ ] Separation of public and private routes
- [ ] Guards or ProtectedRoute for auth control

## 3. State Management
- [ ] Uses NgRx / Signal (Angular) or Redux / Zustand / React Query
- [ ] State is immutable, selectors are memoized
- [ ] Proper separation between UI state and business logic

## 4. Authentication
- [ ] Auth flow via service or custom hook
- [ ] Interceptor or middleware for attaching token
- [ ] Secure refresh token logic (if applicable)

## 5. UI & Theming
- [ ] Uses a consistent UI framework (Material, Tailwind, Ant Design)
- [ ] Theme switching support (optional)
- [ ] Layouts are reusable and modular

## 6. Internationalization
- [ ] Uses ngx-translate (Angular) or react-i18next
- [ ] All strings are extracted to language files
- [ ] Supports fallback language

## 7. Code Quality
- [ ] ESLint and Prettier configured
- [ ] Husky pre-commit hooks for formatting/lint
- [ ] No console logs or commented code in production

## 8. Environment Configuration
- [ ] Uses .env or environment.ts files properly
- [ ] No hardcoded credentials or URLs
- [ ] Supports multiple environments (dev, staging, prod)

## 9. Testing & Dev Experience
- [ ] At least one unit test per core feature
- [ ] Jest (React) or Karma/Jasmine (Angular) setup
- [ ] Has README for setup and contribution

## 10. Performance & Accessibility
- [ ] Uses lazy loading where needed
- [ ] No unnecessary re-renders (useMemo, memo)
- [ ] Follows basic a11y best practices
