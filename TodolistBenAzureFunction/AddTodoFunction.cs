using System;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TodolistBenShared;
using TodolistBenShared.Clients;

namespace TodolistBenAzureFunction
{
    public static class AddTodoFunction
    {
        [FunctionName("AddTodo")]
        public static async Task Run(
            [ServiceBusTrigger("incoming-todos", Connection = "SERVICEBUS_CONNECTION_STRING")]string myQueueItem,
            [SignalR(HubName = "todoHub")]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var todo = JsonSerializer.Deserialize<Todo>(myQueueItem);

            var connectionString = Environment.GetEnvironmentVariable("sqldb_connection");
            await new TodoDbClient(connectionString).AddTodoAsync(todo);

            if (string.IsNullOrEmpty(todo.ConnectionId))
                return;

            // Only send a response if the connection id is set
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    ConnectionId = todo.ConnectionId,
                    Target = "todoResponse",
                    Arguments = new[] { $"Todo {todo.Id} saved" }

                });
        }
    }
}
