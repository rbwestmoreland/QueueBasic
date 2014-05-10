QueueBasic
==========

Queues Simplified

```csharp
using System;

class Program
{
    static void Main()
    {
        var queueFactory = new QueueBasic.InProcess.InProcessQueueFactory(); //also support: Iron.io
        var queue = queueFactory.Create("exmaple queue");

        //add a message onto the queue
        queue.Push("my message");

        //reserve the next message on the queue
        var message1 = queue.Peek();
        var message2 = queue.Peek(); //null, no more messages on the queue

        //the number of messages on the queue (including reserved)
        var count = queue.Count();

        //remove a message from the queue
        queue.Pop(message1);

        Console.WriteLine("press any key to exit...");
        Console.ReadKey(true);
    }
}
```
