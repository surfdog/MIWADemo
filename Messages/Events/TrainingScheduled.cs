using NServiceBus;

namespace Messages
{
    public class TrainingScheduled :
        IEvent
    {
        public string TrainingId { get; set; }
    }


}
