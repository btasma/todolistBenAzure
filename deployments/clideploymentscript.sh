#!/usr/bin/env bash

export RGLOCATION=northeurope
export RGNAME=todolistbenazure

# Group
az group create --name "${RGNAME}" --location "${RGLOCATION}"

# Signalr
az signalr create --resource-group "${RGNAME}" --name "${RGNAME}-signalr" --location "${RGLOCATION}" --service-mode Default --sku Free_F1 --unit-count 1

# Servicebus
az servicebus namespace create --resource-group "${RGNAME}" --name "${RGNAME}-servicebus" --location "${RGLOCATION}" --sku Basic
az servicebus queue create --resource-group "${RGNAME}" --name incoming-todos --namespace-name "${RGNAME}-servicebus" --enable-partitioning

# Storage
az storage account create --resource-group "${RGNAME}" --name "${RGNAME}storage" --location "${RGLOCATION}" --kind StorageV2 --access-tier Hot --sku Standard_LRS
az storage container create --name todoitems --resource-group "${RGNAME}" --account-name "${RGNAME}storage"

# Search
az search service create --resource-group "${RGNAME}" --name "${RGNAME}-search" --location "${RGLOCATION}" --sku Free
# Create Index ('Add Storage') manually!

# Insight
az extension add --name application-insights
az monitor app-insights component create --resource-group "${RGNAME}" --location "${RGLOCATION}" --app "${RGNAME}-appinsights"

# SQL server
az sql server create --resource-group "${RGNAME}" --location "${RGLOCATION}" --name "${RGNAME}-dbserver" --admin-user "todolistbenazure" --admin-password "toh@kghjdolhjgjhjhgjistK6786"
az sql server firewall-rule create --resource-group "${RGNAME}" --server "${RGNAME}-dbserver" -n allowany --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# SQL DB
az sql db create --resource-group "${RGNAME}" --server "${RGNAME}-dbserver" --name "${RGNAME}-db" --tier Basic
# Connect to the db and create the DB scheme (see schema.sql)

# Function
az functionapp create --resource-group "${RGNAME}" --name "${RGNAME}-functionapp" --consumption-plan-location "${RGLOCATION}" --storage-account "${RGNAME}storage" --app-insights "${RGNAME}-appinsights" --functions-version 3 --os-type Linux --runtime dotnet

# Web App
az appservice plan create --resource-group "${RGNAME}" --name "${RGNAME}-plan" --location "${RGLOCATION}" --sku B1 --is-linux
az webapp create --resource-group "${RGNAME}" --name "${RGNAME}-webapp" --plan t"${RGNAME}-plan" --runtime "DOTNETCORE|3.1"