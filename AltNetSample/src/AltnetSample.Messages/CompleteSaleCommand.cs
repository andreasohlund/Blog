using NServiceBus;

namespace AltnetSample.Messages
{
    public class CompleteSaleCommand : IMessage
    {
        public int ProductID { get; set; }
    }
}