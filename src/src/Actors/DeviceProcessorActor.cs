using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Orleankka;
using OrleansCache.Messages;

namespace OrleansCache.Actors
{
    public class DeviceProcessorActor : Actor
    {
        public Dictionary<string, Instant> RecentDevices = new Dictionary<string, Instant>();
        public DeviceProcessorActor()
        {
            var stream = Program.Orleankka.StreamOf("default", "checkpoint");
            stream.Subscribe<CheckpointMessage>(message =>
            {
                if (!RecentDevices.ContainsKey(message.Identifier))
                {
                    Console.WriteLine($"Howdy '{message.Identifier}'!");

                    RecentDevices.Add(message.Identifier, message.LastSeenAt);
                }
                else
                {
                    if (RecentDevices[message.Identifier] > SystemClock.Instance.GetCurrentInstant().Plus(Duration.FromMinutes(5)))
                    {
                        Console.WriteLine($"Welcome back '{message.Identifier}'!");

                        RecentDevices[message.Identifier] = message.LastSeenAt;
                    }
                }
            });
        }
    }
}
