using System;
using System.Reflection;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using Orleankka;
using Orleankka.Cluster;
using Orleankka.Core;
using Orleankka.Meta;
using Orleankka.Playground;
using Orleans.Runtime.Configuration;

namespace OrleansCache
{
    public class Program
    {
        public static IActorSystem Orleankka;
        public static void Main()
        {
            Console.WriteLine("Booting cluster might take some time ...\n");

            var config = new ClusterConfiguration().LoadFromEmbeddedResource<Program>("Server.xml");

            Orleankka = ActorSystem.Configure()
                .Cluster()
                .From(config)
                .Register(Assembly.GetExecutingAssembly())
                .Serializer<NativeSerializer>()
                .Done();

            HostConfiguration hostConfigs = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };

            Uri address = new Uri("http://localhost:1234");

            using (var host = new NancyHost(address, new DefaultNancyBootstrapper(), hostConfigs))
            {
                host.Start();
                Console.WriteLine($"API is now accessible via {address}");
            }

            Console.WriteLine("\nPress any key to terminate ...");
            Console.ReadKey(true);

            Orleankka.Dispose();
            Environment.Exit(0);
        }
    }
}