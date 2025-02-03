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

resource "azurerm_api_management" "apim" {
  name                = "fluxocaixa-apim"
  location            = azurerm_resource_group.fluxocaixa_rg.location
  resource_group_name = azurerm_resource_group.fluxocaixa_rg.name
  publisher_name      = "Fluxo Caixa Team"
  publisher_email     = "admin@fluxocaixa.com"

  sku_name = "Consumption_0" # Pode ser alterado para Developer, Basic, Standard, Premium

  identity {
    type = "SystemAssigned"
  }

  tags = {
    Environment = "Dev"
    Project     = "FluxoCaixa"
  }
}

resource "azurerm_api_management_api" "api_lancamentos" {
  name                = "api-controle-lancamentos"
  resource_group_name = azurerm_resource_group.fluxocaixa_rg.name
  api_management_name = azurerm_api_management.apim.name
  revision            = "1"
  display_name        = "API Controle de Lançamentos"
  path                = "lancamentos"
  protocols           = ["https"]

  import {
    content_format = "openapi"
    content_value  = file("${path.module}/swagger-lancamentos.json")
  }
}

output "kube_config" {
  value     = azurerm_kubernetes_cluster.aks.kube_config_raw
  sensitive = true
}

resource "kubernetes_deployment" "rabbitmq" {
  metadata {
    name      = "rabbitmq"
    namespace = "default"
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        app = "rabbitmq"
      }
    }

    template {
      metadata {
        labels = {
          app = "rabbitmq"
        }
      }

      spec {
        container {
          image = "rabbitmq:3-management"
          name  = "rabbitmq"

          port {
            container_port = 5672
          }

          port {
            container_port = 15672
          }

          env {
            name  = "RABBITMQ_DEFAULT_USER"
            value = "guest"
          }

          env {
            name  = "RABBITMQ_DEFAULT_PASS"
            value = "guest"
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "rabbitmq" {
  metadata {
    name = "rabbitmq-service"
  }

  spec {
    selector = {
      app = "rabbitmq"
    }

    port {
      protocol    = "TCP"
      port        = 5672
      target_port = 5672
    }

    port {
      protocol    = "TCP"
      port        = 15672
      target_port = 15672
    }

    type = "LoadBalancer"
  }
}
