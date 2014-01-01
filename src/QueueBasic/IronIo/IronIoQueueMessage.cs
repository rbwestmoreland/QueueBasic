using System;

namespace QueueBasic.IronIo
{
    internal class IronIoQueueMessage : IQueueMessage
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
}
