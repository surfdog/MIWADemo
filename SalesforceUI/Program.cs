using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Salesforce
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "SalesforceUI";

            var endpointConfiguration = new EndpointConfiguration("SalesforceUI");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();

            routing.RouteToEndpoint(typeof(ScheduleTraining), "Scheduling");
            routing.RouteToEndpoint(typeof(CancelTraining), "Scheduling");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);

        }


        static ILog log = LogManager.GetLogger<Program>();

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {


            var lastTraining = string.Empty;

            while (true)
            {
                log.Info("Press 'S' to schedule a training, 'C' to cancel last scheduled training, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.S:
                        // Instantiate the command
                        var command = new ScheduleTraining
                        {
                            TrainingId = Guid.NewGuid().ToString()
                        };

                        // Send the command
                        log.Info($"Sending ScheduleTraining command, TrainingId = {command.TrainingId}");
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        lastTraining = command.TrainingId; // Store training identifier to cancel if needed.
                        break;

                    case ConsoleKey.C:
                        var cancelCommand = new CancelTraining
                        {
                            TrainingId = lastTraining
                        };
                        await endpointInstance.Send(cancelCommand)
                            .ConfigureAwait(false);
                        log.Info($"Sent a correlated message to {cancelCommand.TrainingId}");
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
