ACR_NAME='jimbussdemoacr'
ACR_LOGINSERVER=$(az acr show --name $ACR_NAME --query loginServer --output tsv)
echo $ACR_LOGINSERVER

#jimbussdemoacr.azurecr.io

docker tag webappimage:v1 $ACR_LOGINSERVER/webappimage:v1
docker image ls $ACR_LOGINSERVER/webappimage:v1
docker image ls

docker push $ACR_LOGINSERVER/webappimage:v1

az acr repository list --name $ACR_NAME --output table
az acr repository show-tags --name $ACR_NAME --repository webappimage --output table

#Build using ACR Tasks
az acr build --image "webappimage:v1-acr-task" --registry $ACR_NAME .