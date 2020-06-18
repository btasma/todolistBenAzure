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

    public class TodoSearchResult
    {
        public Guid TodoId { get; set; }
        public double Score { get; set; }
    }
}
