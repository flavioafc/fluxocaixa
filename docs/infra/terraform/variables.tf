variable "location" {
  description = "Localização do Azure"
  default     = "East US"
}

variable "tenant_id" {
  description = "Azure Tenant ID"
  type        = string
}

variable "sql_admin_password" {
  description = "Senha do administrador do SQL Server"
  type        = string
  sensitive   = true
}

variable "node_count" {
  description = "Número de nós no AKS"
  default     = 2
}

variable "rabbitmq_user" {
  description = "Usuário do RabbitMQ"
  default     = "guest"
}

variable "rabbitmq_password" {
  description = "Senha do RabbitMQ"
  type        = string
  sensitive   = true
}
