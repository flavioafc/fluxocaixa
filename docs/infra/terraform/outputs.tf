output "resource_group_name" {
  value = azurerm_resource_group.fluxocaixa_rg.name
}

output "sql_server_name" {
  value = azurerm_sql_server.sqlserver.name
}

output "redis_name" {
  value = azurerm_redis_cache.redis.name
}

output "keyvault_name" {
  value = azurerm_key_vault.keyvault.name
}

output "aks_cluster_name" {
  value = azurerm_kubernetes_cluster.aks.name
}

output "rabbitmq_service_name" {
  value = kubernetes_service.rabbitmq.metadata[0].name
}
