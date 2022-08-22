az login

az group create \
    --name jimbuss-demo-rg \
    --location uksouth

ACR_NAME='jimbussdemoacr'

az acr create \
    --resource-group jimbuss-demo-rg \
    --name $ACR_NAME \
    --sku Standard

az acr login --name $ACR_NAME