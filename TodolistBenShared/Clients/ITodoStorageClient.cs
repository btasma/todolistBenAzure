using System;
using System.Threading.Tasks;

namespace TodolistBenAzureWeb.Clients
{
    public interface ITodoStorageClient
    {
        Task<string> DownloadAsync(Guid Id);
        Task UploadAsync(Guid Id, string todoText);
    }
}