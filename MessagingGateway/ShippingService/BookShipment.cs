namespace ShippingService
{
    using System;
    using NServiceBus;

    public class BookShipment : IMessage
    {
        public Guid OrderID { get; set; }
    }
}