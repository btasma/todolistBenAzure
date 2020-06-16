using System;

namespace TodolistBenShared
{
    public class Todo
    {     
        public Guid Id { get; set; }
        public Guid BlobId { get; set; }
        public string ConnectionId { get; set; }
    }
}
