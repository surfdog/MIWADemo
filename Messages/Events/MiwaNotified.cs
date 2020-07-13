using NServiceBus;

namespace Messages
{
    public class MiwaNotified :
        IEvent
    {
        public string TrainingId { get; set; }
    }


}
