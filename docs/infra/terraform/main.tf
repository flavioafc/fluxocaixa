provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "fluxocaixa_rg" {
  name     = "fluxocaixa-rg"
  location = "East US"
}

resource "azurerm_kubernetes_cluster" "aks" {
  name                = "fluxocaixa-aks"
  location            = azurerm_resource_group.fluxocaixa_rg.location
  resource_group_name = azurerm_resource_group.fluxocaixa_rg.name
  dns_prefix          = "fluxocaixa"

  default_node_pool {
    name       = "default"
    node_count = 2
    vm_size    = "Standard_DS2_v2"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_sql_server" "sqlserver" {
  name                         = "fluxocaixa-sqlserver"
  resource_group_name          = azurerm_resource_group.fluxocaixa_rg.name
  location                     = azurerm_resource_group.fluxocaixa_rg.location
  version                      = "12.0"
  administrator_login          = "adminuser"
  administrator_login_password = var.sql_admin_password
}

resource "azurerm_redis_cache" "redis" {
  name                = "fluxocaixa-redis"
  location            = azurerm_resource_group.fluxocaixa_rg.location
  resource_group_name = azurerm_resource_group.fluxocaixa_rg.name
  capacity           = 1
  family             = "C"
  sku_name           = "Basic"
}

resource "azurerm_key_vault" "keyvault" {
  name                = "fluxocaixa-keyvault"
  resource_group_name = azurerm_resource_group.fluxocaixa_rg.name
  location            = azurerm_resource_group.fluxocaixa_rg.location
  sku_name            = "standard"
  tenant_id           = var.tenant_id
}

output "kube_config" {
  value     = azurerm_kubernetes_cluster.aks.kube_config_raw
  sensitive = true
}
