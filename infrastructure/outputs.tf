output "function_app_name" {
  value = azurerm_function_app.fa_dotnetsix.name
  description = "Deployed dotnetsix function app name"
}

output "function_app_default_hostname" {
  value = azurerm_function_app.fa_dotnetsix.default_hostname
  description = "Deployed dotnetsix function app hostname"
}