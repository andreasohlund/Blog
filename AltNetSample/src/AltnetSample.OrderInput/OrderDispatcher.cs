using AltnetSample.Messages;
using NServiceBus;

namespace AltnetSample.OrderInput
{
    public class OrderDispatcher
    {
        readonly IBus bus;

        public OrderDispatcher(IBus bus)
        {
            this.bus = bus;
        }

        public void Place(int productId)
        {
            bus.Send(new CompleteSaleCommand {ProductID = productId});
        }
    }
}