using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using Messages;

namespace MIWAInfrastructure
{
    class MiwaNotifiedHandler :
          IHandleMessages<MiwaNotified>
    {
        static ILog log = LogManager.GetLogger<MiwaNotifiedHandler>();

        public Task Handle(MiwaNotified message, IMessageHandlerContext context)
        {
            log.Info($"Received MiwaNotified, TrainingId = {message.TrainingId} - Done...");

            return Task.CompletedTask;
        }
    }
}
