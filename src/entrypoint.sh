#!/bin/sh
set -e

echo "Applying database migrations..."
dotnet ef database update || { echo "Failed to apply migrations"; exit 1; }

echo "Starting application..."
exec dotnet PokerPlanning.Api.dll