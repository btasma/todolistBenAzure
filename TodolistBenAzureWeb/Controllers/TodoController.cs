using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TodolistBenShared;
using TodolistBenShared.Clients;
using TodolistBenShared.Interfaces;

namespace TodolistBenAzureWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> logger;
        private readonly ITodoQueueClient queueClient;
        private readonly ITodoStorageClient storageClient;
        private readonly ITodoDbClient dbClient;
        private readonly ITodoSearchClient searchClient;

        public TodoController(ILogger<TodoController> logger, ITodoQueueClient queueClient, ITodoStorageClient storageClient, ITodoDbClient dbClient, ITodoSearchClient searchClient)
        {
            this.logger = logger;
            this.queueClient = queueClient;
            this.storageClient = storageClient;
            this.dbClient = dbClient;
            this.searchClient = searchClient;
        }

        [HttpGet("search/{term}")]
        public async Task<List<TodoSearchResult>> Get(string term)
        {
            return await this.searchClient.SearchTodosAsync(term);
        }

        [HttpGet]
        public async Task<List<Todo>> Get()
        {
            return await this.dbClient.GetTodosAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string text, [FromHeader] string connectionId)
        {
            var todoId = Guid.NewGuid();
            var blobId = Guid.NewGuid();

            await this.storageClient.UploadAsync(blobId, text, todoId);
            await this.queueClient.SendAsync(new Todo() { Id = todoId, BlobId = blobId, ConnectionId = connectionId });

            return Ok(todoId);
        }
    }
}
