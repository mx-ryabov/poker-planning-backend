#!/bin/sh
set -e

echo "Applying database migrations..."
./efbundle --connection "$ConnectionStrings__PokerPlanningDbConnection"

echo "Starting application..."
exec dotnet PokerPlanning.Api.dll