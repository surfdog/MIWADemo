using System;
using System.Threading.Tasks;
using NServiceBus;

namespace MIWAInfrastructure
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "MIWA";

            var endpointConfiguration = new EndpointConfiguration("MIWA");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
