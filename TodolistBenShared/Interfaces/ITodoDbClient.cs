using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodolistBenShared.Interfaces
{
    public interface ITodoDbClient
    {
        Task AddTodoAsync(Todo todo);
        Task<List<Todo>> GetTodosAsync();
    }
}