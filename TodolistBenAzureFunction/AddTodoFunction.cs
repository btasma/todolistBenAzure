using System;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TodolistBenAzureWeb.Clients;
using TodolistBenShared;

namespace TodolistBenAzureFunction
{
    public static class AddTodoFunction
    {
        [FunctionName("AddTodo")]
        public static async Task Run([ServiceBusTrigger("incoming-todos", Connection = "SERVICEBUS_CONNECTION_STRING")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var todo = JsonSerializer.Deserialize<Todo>(myQueueItem);

            var connectionString = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var query = "INSERT INTO dbo.Todos (ID) VALUES (@ID);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", todo.Id);

                    var rows = await cmd.ExecuteNonQueryAsync();
                    if (rows == 0)
                    {
                        log.LogError("todo {id} not added", todo.Id);
                    }
                }
            }
        }
    }
}
