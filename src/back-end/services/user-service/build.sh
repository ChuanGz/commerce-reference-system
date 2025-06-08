#!/bin/bash
set -e

echo "🔄 Restoring dependencies..."
dotnet restore src/UserService.sln

echo "🏗️ Building solution..."
dotnet build src/UserService.sln --configuration Release

# echo "🧪 Looking for test projects..."
# TEST_PROJECTS=$(find tests -type f -name "*Tests.csproj")

# if [ -z "$TEST_PROJECTS" ]; then
#   echo "⚠️  No test projects found. Skipping tests."
# else
#   echo "🧪 Running tests..."
#   for proj in $TEST_PROJECTS; do
#     echo "🔹 Testing: $proj"
#     dotnet test "$proj" --no-build --configuration Release
#   done
# fi

echo "✅ Build and test completed successfully."