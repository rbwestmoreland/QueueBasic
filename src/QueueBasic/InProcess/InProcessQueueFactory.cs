using System.Collections.Generic;

namespace QueueBasic.InProcess
{
    public class InProcessQueueFactory : IQueueFactory
    {
        private readonly static object _lock;
        private readonly static Dictionary<string, InProcessQueue> _queues;

        static InProcessQueueFactory()
        {
            _lock = new object();
            _queues = new Dictionary<string, InProcessQueue>();
        }

        public IQueue Create(string name)
        {
            InProcessQueue queue;

            lock (_lock)
            {
                if (_queues.ContainsKey(name))
                {
                    queue = _queues[name];
                }
                else
                {
                    queue = new InProcessQueue(name);
                    _queues.Add(name, queue);
                }
            }

            return queue;
        }

        public void Dispose()
        {
        }
    }
}
