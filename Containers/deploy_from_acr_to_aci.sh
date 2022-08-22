az login

az container create \
    --resource-group jimbuss-demo-rg \
    --name jimbussdemo-hello-world-cli \
    --dns-name-label jimbussdemo-hello-world-cli \
    --image mcr.microsoft.com/azuredocs/aci-helloworld \
    --ports 80

az container show --resource-group 'jimbuss-demo-rg' --name 'jimbussdemo-hello-world-cli'

$URL=$(az container show --resource-group 'jimbuss-demo-rg' --name 'jimbussdemo-hello-world-cli' --query ipAddress.fqdn | tr -d '"')
echo 'http://$URL'

ACR_NAME='jimbussdemoacr'

ACR_REGISTRY_ID=$(az acr show --name $ACR_NAME --query id --output tsv)
ACR_LOGINSERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)

echo "ACR ID: $ACR_REGISTRY_ID"
echo "ACR Login Server: $ACR_LOGINSERVER"

SP_NAME=acr-service-principal
SP_PASSWD=$(MSYS_NO_PATHCONV=1 az ad sp create-for-rbac \
    --name http://$ACR_NAME-pull \
    --scopes $ACR_REGISTRY_ID \
    --role acrpull \
    --query password \
    --output tsv)

SP_APPID=$(az ad sp show \
    --id http://$ACR_NAME-pull \
    --query appID
    --output tsv)

echo "Service principal ID: $SP_APPID"
echo "Service principal password: $SP_PASSWD"

az container show --resource-group jimbuss-demo-rg --name jimbussdemo-webapp-cli

az container create \
    --resource-group jimbuss-demo-rg \
    --name jimbussdemo-webapp-cli \
    --dns-name-label jimbussdemo-webapp-cli \
    --ports 80 \
    --image $ACR_LOGINSERVER/webappimage:v1 \
    --registry-login-server $ACR_LOGINSERVER \
    --registry-username $SP_APPID \
    --registry-password $SP_PASSWD