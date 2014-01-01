using System;

namespace QueueBasic
{
    public interface IQueueMessage
    {
        string Id { get; }
        string Data { get; }
    }
}
