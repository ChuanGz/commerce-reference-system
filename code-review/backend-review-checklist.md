
# ✅ Backend Code Review Checklist (.NET Clean Architecture)

## 1. Architecture & Structure
- [ ] Clean Architecture structure (API, Application, Domain, Infrastructure)
- [ ] CQRS pattern via MediatR implemented
- [ ] Domain layer is logic-free and well-encapsulated

## 2. Code Quality
- [ ] Class and method names are clear and descriptive
- [ ] Each class has a single responsibility
- [ ] Avoids hardcoding and magic strings

## 3. Async & Performance
- [ ] All I/O operations use async/await
- [ ] No use of .Result or .Wait()
- [ ] Proper use of CancellationToken

## 4. Repository Pattern
- [ ] No direct usage of DbContext outside repository
- [ ] Interface-driven design (IUserRepository, IRoleRepository, etc.)
- [ ] Proper use of AsNoTracking for read-only queries

## 5. Validation
- [ ] Uses FluentValidation in Application layer
- [ ] Request models are validated properly before processing

## 6. Logging & Error Handling
- [ ] Logs important events (auth, deletion, etc.)
- [ ] No sensitive data in logs
- [ ] Uses middleware or filters for centralized error handling

## 7. Security
- [ ] Auth logic handled via ITokenService / middleware
- [ ] No sensitive data leaks
- [ ] Role-based or policy-based authorization applied

## 8. Mapping & DTOs
- [ ] Does not expose domain entities directly
- [ ] Uses ViewModels or DTOs for response
- [ ] AutoMapper or manual mapping is clean and safe

## 9. Environment & Configuration
- [ ] appsettings.json properly split for dev/staging/prod
- [ ] Secrets are not committed
- [ ] Docker or build scripts available

## 10. Testability
- [ ] Repositories and services are unit-testable
- [ ] Separation of logic and I/O allows for mocking
- [ ] At least basic unit/integration test structure exists
