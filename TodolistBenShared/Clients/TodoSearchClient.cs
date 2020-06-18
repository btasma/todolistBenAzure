using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodolistBenShared;
using TodolistBenShared.Interfaces;

namespace TodolistBenShared.Clients
{
    public class TodoSearchClient : ITodoSearchClient
    {
        private readonly ISearchIndexClient indexClient;

        public TodoSearchClient(string serviceName, string apiKey, string indexName)
        {
            var searchclient = new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            this.indexClient = searchclient.Indexes.GetClient(indexName);
        }

        public async Task<List<TodoSearchResult>> SearchTodosAsync(string term)
        {
            var res = await indexClient.Documents.SearchAsync(term);
            return res.Results.Select(x => new TodoSearchResult() { Score = x.Score, TodoId = Guid.Parse(x.Document["TodoId"].ToString()) }).ToList();
        }
    }
}
