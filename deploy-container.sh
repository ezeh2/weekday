#!/bin/bash
set -e  # Exit on any error

# Set paths
REPO_ROOT=$(git rev-parse --show-toplevel)
PROJECT_PATH="$REPO_ROOT/src/WeekDayWebApplication/WeekDayWebApplication"
PUBLISH_PATH="$REPO_ROOT/publish"

# Azure resource names
RESOURCE_GROUP="rg-2025"
WEBAPP_NAME="weekday-2025"
ACR_NAME=${ACR_NAME:-"weekdayacr"}  # Default ACR name if not set

# Build and publish .NET app (needed for Docker build)
echo "Building and publishing WeekDayWebApplication..."

# Clean and restore
dotnet clean "$PROJECT_PATH"
dotnet restore "$PROJECT_PATH"

# Build and publish
dotnet publish "$PROJECT_PATH" \
    --configuration Release \
    --output "$PUBLISH_PATH" \
    --no-restore \
    /p:UseAppHost=false

echo "Building Docker image..."
cd "$REPO_ROOT"
docker build -t weekday:latest .

# Push to ACR
echo "Pushing to ACR: $ACR_NAME"
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --resource-group $RESOURCE_GROUP --query loginServer -o tsv)

# Tag for ACR
docker tag weekday:latest "$ACR_LOGIN_SERVER/weekday:latest"

# Login and push
az acr login --name $ACR_NAME
docker push "$ACR_LOGIN_SERVER/weekday:latest"

# Update web app to use container
echo "Updating web app to use container image..."
az webapp config container set \
    --name $WEBAPP_NAME \
    --resource-group $RESOURCE_GROUP \
    --docker-custom-image-name "$ACR_LOGIN_SERVER/weekday:latest" \
    --docker-registry-server-url "https://$ACR_LOGIN_SERVER"

# Restart the web app to pick up the new image
echo "Restarting web app..."
az webapp restart --name $WEBAPP_NAME --resource-group $RESOURCE_GROUP

echo "Done! 🚀"