variable "project" {
  type = string
  description = "Project name"
}

variable "id" {
  type = string
  description = "Unique project id (5 chars ^[a-z0-9]{5}$)"
}

variable "environment" {
  type = string
  description = "Environment (dev / qa / prod)"
}

variable "location" {
  type = string
  description = "Azure region to deploy module to"
}