# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY ./src/IdentityService.sln .
COPY ./src/IdentityService.API/ ./IdentityService.API/
COPY ./src/IdentityService.Application/ ./IdentityService.Application/
COPY ./src/IdentityService.Domain/ ./IdentityService.Domain/
COPY ./src/IdentityService.Infrastructure/ ./IdentityService.Infrastructure/

# Restore dependencies
RUN dotnet restore IdentityService.sln

# Build and publish
WORKDIR /src/IdentityService.API
RUN dotnet publish -c Release -o /app/publish

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose app port
EXPOSE 80

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Docker

# Optional: Add health check (make sure endpoint exists)
HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
  CMD curl --fail http://localhost/health || exit 1

# Launch app
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]