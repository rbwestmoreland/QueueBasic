using System;

namespace QueueBasic
{
    public interface IQueueFactory : IDisposable
    {
        IQueue Create(string name);
    }
}
