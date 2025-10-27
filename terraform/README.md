# Terraform: Azure Resource Group `rg-2025`

This folder contains a minimal Terraform configuration that creates an Azure Resource Group named `rg-2025`.

Files:
- `main.tf` — provider and `azurerm_resource_group` resource (name `rg-2025`).
- `variables.tf` — defines `location` (defaults to `eastus`).

Quick start

1. Install Terraform (>= 1.0.0).
2. Authenticate to Azure (e.g., `az login`).
3. From this directory run:

```bash
terraform init
terraform plan
terraform apply -auto-approve
```

You can override location with:

```bash
terraform apply -var="location=westeurope" -auto-approve
```

Notes

- This configuration assumes you want the resource group named exactly `rg-2025` in Azure.
- If you need a different cloud/provider, tell me and I can adapt the config.
