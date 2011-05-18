namespace ShippingService
{
    using NServiceBus;
    using OrderService.Messages;

    public class OrderPlacedMessageHandlerV2 : IHandleMessages<OrderPlaced>
    {
        readonly IBus bus;

        public OrderPlacedMessageHandlerV2(IBus bus, IRepository repository)
        {
            this.bus = bus;
            this.repository = repository;
        }

        public void Handle(OrderPlaced message)
        {
            repository.Save(new Order
                               {
                                   Id = message.OrderID,
                                   Status = ShippingStatus.AwaitingShipment
                               });

            bus.Send<BookShipment>(m =>
                {
                    m.OrderID = message.OrderID;
                });
        }

        readonly IRepository repository;
    }
}
