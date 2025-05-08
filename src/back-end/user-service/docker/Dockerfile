# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ./src/UserService.sln .
COPY ./src/UserService.API/ ./UserService.API/
COPY ./src/UserService.Application/ ./UserService.Application/
COPY ./src/UserService.Domain/ ./UserService.Domain/
COPY ./src/UserService.Infrastructure/ ./UserService.Infrastructure/

# Restore dependencies using the solution
RUN dotnet restore UserService.sln

# Build and publish the app
WORKDIR /src/UserService.API
RUN dotnet publish -c Release -o /app/publish

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy build output from previous stage
COPY --from=build /app/publish .

# Expose default port
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development

# Health check endpoint
HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
  CMD curl --fail http://localhost/health || exit 1

# Run the app
ENTRYPOINT ["dotnet", "UserService.API.dll"]
