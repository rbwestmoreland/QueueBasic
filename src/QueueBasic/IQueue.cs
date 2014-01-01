using System;

namespace QueueBasic
{
    public interface IQueue : IDisposable
    {
        string Address { get; }

        void Push(string data);
        IQueueMessage Peek();
        void Pop(IQueueMessage queueMessage);
        long Count();
    }
}
