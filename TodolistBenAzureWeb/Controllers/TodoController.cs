using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodolistBenAzureWeb.Clients;

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
        public async Task<IActionResult> Post([FromBody] string text)
        {
            var messageId = Guid.NewGuid();

            await this.storageClient.UploadAsync(messageId, text);
            await this.queueClient.SendAsync(messageId);

            return Ok();
        }
    }
}
