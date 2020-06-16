using System;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TodolistBenAzureWeb.Clients;
using TodolistBenShared;

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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var query = "INSERT INTO dbo.Todos (ID, BlobId) VALUES (@ID, @BlobId);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", todo.Id);
                    cmd.Parameters.AddWithValue("@BlobId", todo.BlobId);

                    var rows = await cmd.ExecuteNonQueryAsync();
                    if (rows == 0)
                    {
                        log.LogError("todo {id} not added", todo.Id);
                        throw new Exception();
                    }
                }
            }

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
