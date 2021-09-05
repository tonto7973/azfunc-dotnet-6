# test terraform file to create infrastructure for azure functions V4 with net6.0
provider "azurerm" {
    features {}
}

# create resource group
resource "azurerm_resource_group" "rg_project" {
 name = "${var.environment}-rg-${var.project}"
 location = var.location
}

# create storage account
resource "azurerm_storage_account" "sa_func" {
  name = "${var.environment}func${var.project}fn${var.id}"
  resource_group_name = azurerm_resource_group.rg_project.name
  location = var.location
  account_tier = "Standard"
  account_replication_type = "LRS"
}

# create app insights
resource "azurerm_application_insights" "ai_health" {
  name                = "${var.environment}-health-${var.project}-${var.id}"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg_project.name
  application_type    = "web"
}

# create app service plan
resource "azurerm_app_service_plan" "asp_func" {
  name                = "${var.environment}-aspfunc-${var.project}-${var.id}"
  resource_group_name = azurerm_resource_group.rg_project.name
  location            = var.location
  kind                = "app"
  reserved            = false
  per_site_scaling    = false
  sku {
    tier     = "Dynamic"
    size     = "Y1"
  }
}

# create function app
resource "azurerm_function_app" "fa_dotnetsix" {
  name                       = "${var.environment}-func-${var.project}-dotnetsix-${var.id}"
  resource_group_name        = azurerm_resource_group.rg_project.name
  location                   = var.location
  app_service_plan_id        = azurerm_app_service_plan.asp_func.id
  storage_account_name       = azurerm_storage_account.sa_func.name
  storage_account_access_key = azurerm_storage_account.sa_func.primary_access_key
  version                    = "~4"
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.ai_health.instrumentation_key
    "FUNCTIONS_EXTENSION_VERSION"    = "~4"
    "FUNCTIONS_WORKER_RUNTIME"       = "dotnet"
  }
  site_config {
    dotnet_framework_version  = "v6.0"
    min_tls_version           = "1.2"
    use_32_bit_worker_process = true
    always_on                 = false
  }
}