terraform {
  required_version = ">=0.12"

  required_providers {
    azapi = {
      source  = "azure/azapi"
      version = "~>1.5"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~>3.0"
    }
  }
}

provider "azurerm" {
  features {}
}


resource "azurerm_resource_group" "rg2025" {
  name     = "rg-2025"
  location = var.location
  tags = {
    created_by  = "terraform"
    environment = "dev"
  }
}

# Create the App Service Plan
resource "azurerm_service_plan" "weekday_plan" {
  name                = "weekday-2025-plan"
  resource_group_name = azurerm_resource_group.rg2025.name
  location            = azurerm_resource_group.rg2025.location
  os_type            = "Linux"
  sku_name           = "F1"  # Free tier
}

# Create the App Service (Web App)
resource "azurerm_linux_web_app" "weekday_app" {
  name                = "weekday-2025"
  resource_group_name = azurerm_resource_group.rg2025.name
  location            = azurerm_resource_group.rg2025.location
  service_plan_id     = azurerm_service_plan.weekday_plan.id

  site_config {
    application_stack {
      dotnet_version = "8.0"  # Using .NET 8.0
    }
    always_on = false  # Must be false for Free tier
    # Enable WebSocket support if needed by your ASP.NET app
    websockets_enabled = true
  }

  app_settings = {
    "WEBSITE_RUN_FROM_PACKAGE"             = "1"  # Enable running from a package URL
    "ASPNETCORE_ENVIRONMENT"               = "Production"
    "WEBSITE_TIME_ZONE"                    = "UTC"
    "ASPNETCORE_FORWARDEDHEADERS_ENABLED" = "true"
  }

  # Deploy from the published package
  # didn't work
  # zip_deploy_file = "/home/edward/work/repos/weekday/publish/weekday.zip"  # Path to your published app package

  tags = {
    created_by  = "terraform"
    environment = "dev"
  }
}

