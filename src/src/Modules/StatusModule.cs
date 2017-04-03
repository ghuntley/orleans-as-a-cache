
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

namespace OrleansCache.Modules
{
    public class StatusModule : NancyModule
    {
        public StatusModule()
        {
            Get["/status/{identifier}", true] = async (parameters, cancellectionToken) =>
            {
                if (string.IsNullOrWhiteSpace(parameters.identifier))
                {
                    var response = (Response)"Identifier missing";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var device = Program.Orleankka.ActorOf(typeof(DeviceActor), parameters.identifier);

                try
                {
                    var response = (Response)"Success";
                    response.StatusCode = HttpStatusCode.OK;
                    return response;

                }
                catch (Exception ex)
                {
                    // warning: this is poc code, returning exceptions to client is a security
                    // risk factor.
                    var response = (Response)ex.Message;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
            };
        }
    }
}
