﻿using Azure.Storage.Blobs;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodolistBenShared;

namespace TodolistBenAzureWeb.Clients
{
    public class TodoStorageClient : ITodoStorageClient
    {
        private readonly BlobContainerClient container;

        public TodoStorageClient(string connectionString, string containerName)
        {
            container = new BlobContainerClient(connectionString, containerName);
        }

        public async Task UploadAsync(Guid Id, string todoText)
        {
            BlobClient blob = container.GetBlobClient(Id.ToString());
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(todoText)))
            {
                await blob.UploadAsync(ms);
            }
        }

        public async Task<string> DownloadAsync(Guid Id)
        {
            BlobClient blob = container.GetBlobClient(Id.ToString());
            var download = await blob.DownloadAsync();
            using (var sr = new StreamReader(download.Value.Content))
            {
                return sr.ReadToEnd();
            }
        }

        public async Task DeleteAsync(Guid Id)
        {
            BlobClient blob = container.GetBlobClient(Id.ToString());
            await blob.DeleteIfExistsAsync();
        }
    }
}
