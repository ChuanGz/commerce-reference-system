#!/bin/bash

echo "🛠️  Starting log shrinker loop..."

# Wait for SQL Server to be ready
until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <set-me> -Q "SELECT 1" > /dev/null 2>&1; do
  echo "⏳ Waiting for SQL Server to be ready..."
  sleep 5
done

echo "✅ SQL Server is ready. Starting shrink loop."

while true; do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <set-me> -i /shrink-userdb.sql
  sleep 30
done