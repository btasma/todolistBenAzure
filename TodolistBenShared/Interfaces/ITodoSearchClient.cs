using System.Collections.Generic;
using System.Threading.Tasks;
using TodolistBenShared.Clients;

namespace TodolistBenShared.Interfaces
{
    public interface ITodoSearchClient
    {
        Task<List<TodoSearchResult>> SearchTodosAsync(string term);
    }
}