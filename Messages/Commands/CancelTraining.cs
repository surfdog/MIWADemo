
using NServiceBus;

namespace Messages
{
    public class CancelTraining :
        ICommand
    {
        public string TrainingId { get; set; }
    }
}

