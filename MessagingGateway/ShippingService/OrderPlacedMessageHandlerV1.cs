namespace ShippingService
{
    using NServiceBus;
    using OrderService.Messages;

    public class OrderPlacedMessageHandlerV1 : IHandleMessages<OrderPlaced>
    {
        public OrderPlacedMessageHandlerV1(IRepository repository, IFedexService fedex)
        {
            this.fedex = fedex;
            this.repository = repository;
        }

        public void Handle(OrderPlaced message)
        {
            var order = new Order{ Id = message.OrderID };

            order.TrackingCode = fedex.BookPickup(order);
        
            order.Status = ShippingStatus.PickupBooked;

            repository.Save(order);
        }

        readonly IRepository repository;

        readonly IFedexService fedex;
    }
}