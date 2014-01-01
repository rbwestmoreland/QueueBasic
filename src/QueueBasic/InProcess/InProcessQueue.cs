using System;
using System.Collections.Generic;
using System.Linq;

namespace QueueBasic.InProcess
{
    internal class InProcessQueue : IQueue
    {
        private readonly object _lock;
        private readonly List<InProcessQueueMessage> _messages;

        public string Address { get; private set; }

        public InProcessQueue(string address)
        {
            _lock = new object();
            _messages = new List<InProcessQueueMessage>();
            Address = address;
        }

        public void Push(string data)
        {
            var message = new InProcessQueueMessage
            {
                Id = Guid.NewGuid().ToString("N"),
                Data = data
            };

            lock (_lock)
            {
                _messages.Add(message);
            }
        }

        public IQueueMessage Peek()
        {
            InProcessQueueMessage message;

            lock (_lock)
            {
                message = _messages.FirstOrDefault(m => !m.IsReserved());

                if (message != null)
                    message.Reserve();
            }

            return message;
        }

        public void Pop(IQueueMessage queueMessage)
        {
            lock (_lock)
            {
                var inProcessQueueMessage = queueMessage as InProcessQueueMessage;

                if (inProcessQueueMessage != null)
                    _messages.Remove(inProcessQueueMessage);
            }
        }

        public long Count()
        {
            lock (_lock)
            {
                return _messages.Count;
            }
        }

        public void Dispose()
        {
        }
    }
}
