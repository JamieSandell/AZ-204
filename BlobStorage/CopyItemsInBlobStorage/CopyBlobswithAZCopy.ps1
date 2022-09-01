<#
Requires the AZCopy tool from Microsoft
Requires Storage Blob Data Owner permissions on the storage account (can take up to 5 minutes to propagate)
#>
#azcopy login --tenant-id=80294c2e-7d09-469f-95e7-d0e75197f067 # for the access token, run once

$source = 'C:\3DMark_11'
$destination = "https://storagejimbuss.blob.core.windows.net/docs?sv=2021-06-08&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2022-09-02T04:51:56Z&st=2022-09-01T20:51:56Z&spr=https&sig=yW1yGbpGGkH1VI%2FRZ9r7tzqmfuo1MAL1FuHFVPJX7kA%3D"

azcopy copy $source $destination --recursive=true