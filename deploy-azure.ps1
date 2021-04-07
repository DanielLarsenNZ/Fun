$ErrorActionPreference = 'Stop'

$location = 'australiaeast'
$loc = 'aue'
$rg = 'fun-rg'
$tags = 'project=fun'

$storage = "fun$loc"
$blobContainer = "eventhubsprocessorhost"

$cosmos = 'scalefun'
$cosmosDB = 'fun'
$container = 'MyDocuments'
$throughput = 400
$pk = '/id'

$eventhubNamespace = 'scalefun-hub'
$eventhubs = 'myevents'
$eventhubAuthRule = 'ListenerSender1'
$eventhubsSku = 'Basic'
$eventhubsRetentionDays = 1
$eventhubsPartitions = 12    # 2 - 32. Cannot be changed after deployment. Good discussion here: https://medium.com/@iizotov/azure-functions-and-event-hubs-optimising-for-throughput-549c7acd2b75


# RESOURCE GROUP
az group create -n $rg --location $location --tags $tags


# STORAGE ACCOUNT
az storage account create -n $storage -g $rg --location $location --sku 'Standard_LRS' --https-only --kind StorageV2 --tags $tags
az storage container create -n $blobContainer --account-name $storage --public-access off


# COSMOS DB ACCOUNT
az cosmosdb create -n $cosmos -g $rg --default-consistency-level Session
az cosmosdb sql database create -a $cosmos -g $rg -n $cosmosDB --throughput $throughput
az cosmosdb sql container create -a $cosmos -g $rg -d $cosmosDB -n $container -p $pk 


# EVENT HUBS
az eventhubs namespace create -g $rg --name $eventhubNamespace --location $location --tags $tags --sku $eventhubsSku

foreach ($eventhub in $eventhubs) {
    az eventhubs eventhub create -g $rg --namespace-name $eventhubNamespace --name $eventhub --message-retention $eventhubsRetentionDays --partition-count $eventhubsPartitions
}

az eventhubs namespace authorization-rule create -g $rg --namespace-name $eventhubNamespace --name $eventhubAuthRule --rights Listen Send
