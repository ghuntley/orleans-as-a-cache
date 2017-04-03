
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses;
using NodaTime;
using OrleansCache.Actors;
using OrleansCache.Messages;

namespace OrleansCache
{
    public class CheckpointModule : NancyModule
    {
        public CheckpointModule()
        {
            Patch["/checkpoint/{identifier}", true] = async (parameters, cancellectionToken) =>
            {
                if (string.IsNullOrWhiteSpace(parameters.identifier))
                {
                    var response = (Response)"Identifier missing";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var device = Program.Orleankka.ActorOf(typeof(DeviceActor), parameters.identifier);

                Instant now = SystemClock.Instance.GetCurrentInstant();

                var checkpoint = new CheckpointMessage()
                {
                    Identifier = parameters.identifier,
                    Longitude = parameters.longitude,
                    Latitude = parameters.latitude,
                    LastSeenAt = SystemClock.Instance.GetCurrentInstant()
                };

                try
                {
                    await device.On(checkpoint);
                    var response = (Response)"Success";
                    response.StatusCode = HttpStatusCode.OK;
                    return response;

                }
                catch (Exception ex)
                {
                    // warning: this is poc code, returning exceptions to client is a security
                    // risk factor.
                    var response = (Response) ex.Message;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
            };
        }
    }
}
