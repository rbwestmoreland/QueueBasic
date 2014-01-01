using System;

namespace QueueBasic.InProcess
{
    internal class InProcessQueueMessage : IQueueMessage
    {
        public string Id { get; set; }
        public string Data { get; set; }
        private DateTime? ReservedUntil { get; set; }

        public void Reserve()
        {
            ReservedUntil = DateTime.UtcNow.AddSeconds(60);
        }

        public bool IsReserved()
        {
            if (!ReservedUntil.HasValue)
                return false;

            return ReservedUntil.Value > DateTime.UtcNow;
        }
    }
}
