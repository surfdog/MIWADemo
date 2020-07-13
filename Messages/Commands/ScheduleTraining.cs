
using NServiceBus;

namespace Messages
{
    public class ScheduleTraining :
        ICommand
    {
        public string TrainingId { get; set; }
    }
}

