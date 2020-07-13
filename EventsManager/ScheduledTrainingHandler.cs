using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using Messages;

namespace EventsManager
{
    public class ScheduledTrainingHandler :
          IHandleMessages<TrainingScheduled>
    {
        static ILog log = LogManager.GetLogger<ScheduledTrainingHandler>();

        public Task Handle(TrainingScheduled message, IMessageHandlerContext context)
        {
            log.Info($"Received TrainingScheduled, TrainingId = {message.TrainingId} - Notifying MIWA...");

            var miwaNotified = new MiwaNotified
            {
                TrainingId = message.TrainingId
            };
            return context.Publish(miwaNotified);
        }
    }
}
