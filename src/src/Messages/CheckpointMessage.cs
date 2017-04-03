using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace OrleansCache.Messages
{
    public class CheckpointMessage
    {
        public string Identifier { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Instant LastSeenAt { get; set; }

    }
}
