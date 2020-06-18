using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TodolistBenShared.Interfaces;

namespace TodolistBenShared.Clients
{
    public class TodoDbClient : ITodoDbClient
    {
        private readonly string connectionString;

        public TodoDbClient(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AddTodoAsync(Todo todo)
        {
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
                        throw new Exception();
                    }
                }
            }
        }

        public async Task<List<Todo>> GetTodosAsync()
        {
            var todos = new List<Todo>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var query = "SELECT ID, BlobId FROM dbo.Todos;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            todos.Add(new Todo() { Id = reader.GetGuid(0), BlobId = reader.GetGuid(1) });
                        }
                    }
                }
            }
            return todos;
        }
    }
}
