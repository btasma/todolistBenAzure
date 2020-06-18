using System;
using System.Threading.Tasks;

namespace TodolistBenShared.Interfaces
{
    public interface ITodoStorageClient
    {
        Task<string> DownloadAsync(Guid Id);
        Task UploadAsync(Guid Id, string todoText, Guid todoId);
    }
}