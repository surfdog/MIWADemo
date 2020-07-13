using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using Messages;

namespace Scheduling
{
    class SchedulingChangePolicy : Saga<SchedulingChangeState>,
        IAmStartedByMessages<ScheduleTraining>,
        IHandleMessages<CancelTraining>,
        IHandleTimeouts<SchedulingChangeIsOver>
    {
        static ILog log = LogManager.GetLogger<SchedulingChangePolicy>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SchedulingChangeState> mapper)
        {
            mapper.ConfigureMapping<ScheduleTraining>(p => p.TrainingId).ToSaga(s => s.TrainingId);
            mapper.ConfigureMapping<CancelTraining>(p => p.TrainingId).ToSaga(s => s.TrainingId);
        }
        public async Task Handle(ScheduleTraining message, IMessageHandlerContext context)
        {
            log.Info($"Received ScheduleTraining, TrainingId = {message.TrainingId}");
            Data.TrainingId = message.TrainingId;

            log.Info($"Starting cool down period for training #{Data.TrainingId}.");
            await RequestTimeout(context, TimeSpan.FromSeconds(20), new SchedulingChangeIsOver());
        }

        public Task Handle(CancelTraining message, IMessageHandlerContext context)
        {
            log.Info($"Training #{message.TrainingId} was cancelled.");

            //TODO: Possibly publish an TrainingCancelled event?

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Timeout(SchedulingChangeIsOver timeout, IMessageHandlerContext context)
        {
            log.Info($"Cooling down period for training #{Data.TrainingId} has elapsed.");

            var trainingScheduled = new TrainingScheduled
            {
                TrainingId = Data.TrainingId
            };

            await context.Publish(trainingScheduled);

            MarkAsComplete();
        }
    }

    class SchedulingChangeIsOver
    {
    }


    public class SchedulingChangeState : ContainSagaData
    {
        public string TrainingId { get; set; }
    }
}
