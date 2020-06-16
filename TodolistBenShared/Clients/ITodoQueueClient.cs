using System;
using System.Threading.Tasks;
using TodolistBenShared;

namespace TodolistBenAzureWeb.Clients
{
    public interface ITodoQueueClient
    {
        Task SendAsync(Todo todo);
    }
}