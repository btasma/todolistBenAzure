using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TodolistBenAzureWeb.Clients;
using TodolistBenShared;

namespace TodolistBenAzureWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> logger;
        private readonly ITodoQueueClient queueClient;
        private readonly ITodoStorageClient storageClient;

        public TodoController(ILogger<TodoController> logger, ITodoQueueClient queueClient, ITodoStorageClient storageClient)
        {
            this.logger = logger;
            this.queueClient = queueClient;
            this.storageClient = storageClient;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string text, [FromHeader] string connectionId)
        {
            var todoId = Guid.NewGuid();
            var blobId = Guid.NewGuid();

            await this.storageClient.UploadAsync(blobId, text);
            await this.queueClient.SendAsync(new Todo() { Id = todoId, BlobId = blobId, ConnectionId = connectionId });

            return Ok(todoId);
        }
    }
}
