using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Orleankka;
using OrleansCache.Messages;

namespace OrleansCache.Actors
{
    public class DeviceActor : Actor
    {
        public Instant LastSeenAt { get; private set; }
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public Task Handle(CheckpointMessage cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd?.Identifier))
            {
                throw new ArgumentException("Identifier not specified.");
            }

            if (cmd.Longitude.Equals(default(double)))
            {
                throw new ArgumentException("Latitude is 0");
            }

            if (cmd.Longitude.Equals(default(double)))
            {
                throw new ArgumentException("Longitude is 0");
            }

            LastSeenAt = cmd.LastSeenAt;
            Longitude = cmd.Longitude;
            Latitude = cmd.Latitude;

            var stream = System.StreamOf("default", "checkpoint");
            stream.Push(cmd);

            return Task.FromResult(0);
        }
    }
}
