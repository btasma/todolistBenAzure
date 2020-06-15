using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodolistBenShared;

namespace TodolistBenAzureWeb.Clients
{
    public class TodoQueueClient : ITodoQueueClient
    {
        private readonly IQueueClient queueClient;

        public TodoQueueClient(string connectionString, string queueName)
        {
            queueClient = new QueueClient(connectionString, queueName);
        }

        public async Task SendAsync(Guid Id)
        {
            var messageBody = new Todo() { Id = Id };

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody)))
            {
                ContentType = "application/json",
            };

            await queueClient.SendAsync(message);
        }
    }
}
