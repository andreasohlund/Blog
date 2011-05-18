namespace ShippingService
{
    using System;

    public class Order
    {
        public ShippingStatus Status { get; set; }
        public Guid Id { get; set; }
        public string TrackingCode { get; set; }
    }
}