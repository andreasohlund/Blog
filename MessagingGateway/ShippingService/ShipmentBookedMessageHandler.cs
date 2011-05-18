namespace ShippingService
{
    using NServiceBus;

    public class ShipmentBookedMessageHandler : IHandleMessages<ShipmentBooked>
    {
        public ShipmentBookedMessageHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(ShipmentBooked message)
        {
            var order = repository.Get<Order>(message.OrderID);

            order.TrackingCode = message.TrackingCode;

            repository.Save(order);
        }

        readonly IRepository repository;
    }
}