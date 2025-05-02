#!/bin/bash

# --- Function: convert kebab-case to PascalCase ---
to_pascal() {
  IFS="-" read -ra PARTS <<< "$1"
  local result=""
  for part in "${PARTS[@]}"; do
    result+=$(tr '[:lower:]' '[:upper:]' <<< "${part:0:1}")${part:1}
  done
  echo "$result"
}

# --- Main Script ---
SERVICE_NAME=$1
BASE_DIR="./$SERVICE_NAME"

if [ -z "$SERVICE_NAME" ]; then
  echo "ERR Please provide a service name in kebab-case (e.g., order-service)"
  exit 1
fi

SERVICE_PASCAL=$(to_pascal "$SERVICE_NAME")

echo "1... Creating service '$SERVICE_NAME' with PascalCase '$SERVICE_PASCAL'"

# Folder structure
mkdir -p "$BASE_DIR/docker"
mkdir -p "$BASE_DIR/tests"
mkdir -p "$BASE_DIR/docs"
mkdir -p "$BASE_DIR/src"
cd "$BASE_DIR/src" || exit

# Create projects
for layer in Domain Application Infrastructure API; do
  if [ "$layer" = "API" ]; then
    dotnet new webapi -n "$SERVICE_PASCAL.$layer" -f net8.0
  else
    dotnet new classlib -n "$SERVICE_PASCAL.$layer"
  fi
done

# Create solution
dotnet new sln -n "$SERVICE_PASCAL.sln"
dotnet sln add "$SERVICE_PASCAL."{Domain,Application,Infrastructure,API}

# Add references
dotnet add "$SERVICE_PASCAL.Application" reference "$SERVICE_PASCAL.Domain"
dotnet add "$SERVICE_PASCAL.Infrastructure" reference "$SERVICE_PASCAL.Application"
dotnet add "$SERVICE_PASCAL.Infrastructure" reference "$SERVICE_PASCAL.Domain"
dotnet add "$SERVICE_PASCAL.API" reference "$SERVICE_PASCAL.Infrastructure"
dotnet add "$SERVICE_PASCAL.API" reference "$SERVICE_PASCAL.Application"

# Add appsettings.Development.json
mkdir -p "$SERVICE_PASCAL.API"
cat <<EOF > "$SERVICE_PASCAL.API/appsettings.Development.json"
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:your-sqlserver.database.windows.net,1433;Initial Catalog=${SERVICE_PASCAL%%Service}Db;;User ID=your-user;Password=your-password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
EOF

# Add Dockerfile
cat <<EOF > "../docker/Dockerfile"
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../src .
RUN dotnet publish "${SERVICE_PASCAL}.API/${SERVICE_PASCAL}.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "${SERVICE_PASCAL}.API.dll"]
EOF

# Add docker-compose.yml
cat <<EOF > "../docker/docker-compose.yml"
version: '3.4'

services:
  ${SERVICE_NAME}:
    build:
      context: .
      dockerfile: docker/Dockerfile
    ports:
      - "${PORT:-5000}:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
EOF

# Add VS Code launch config
mkdir -p "../.vscode"
cat <<EOF > "../.vscode/launch.json"
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Launch ${SERVICE_PASCAL}.API",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "\${workspaceFolder}/src/${SERVICE_PASCAL}.API/bin/Debug/net8.0/${SERVICE_PASCAL}.API.dll",
      "args": [],
      "cwd": "\${workspaceFolder}/src/${SERVICE_PASCAL}.API",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "Now listening on: (https?://\\S+)"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "sourceFileMap": {
        "/Views": "\${workspaceFolder}/Views"
      }
    }
  ]
}
EOF

echo "2... $SERVICE_NAME scaffolded with Clean Arch + Docker + appsettings.Development.json + launch.json"
