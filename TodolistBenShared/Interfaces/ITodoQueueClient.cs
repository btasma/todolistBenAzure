using System;
using System.Threading.Tasks;
using TodolistBenShared;

namespace TodolistBenShared.Interfaces
{
    public interface ITodoQueueClient
    {
        Task SendAsync(Todo todo);
    }
}