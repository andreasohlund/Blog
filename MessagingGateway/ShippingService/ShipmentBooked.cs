namespace ShippingService
{
    using System;
    using NServiceBus;

    public class ShipmentBooked:IMessage
    {
        public Guid OrderID { get; set; }
        public string TrackingCode { get; set; }
    }
}