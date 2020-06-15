using System;
using System.Threading.Tasks;

namespace TodolistBenAzureWeb.Clients
{
    public interface ITodoQueueClient
    {
        Task SendAsync(Guid Id);
    }
}