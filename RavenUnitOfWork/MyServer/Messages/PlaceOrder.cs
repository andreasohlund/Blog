namespace MyServer.Messages
{
    using System;
    using NServiceBus;

    public class PlaceOrder:IMessage
    {
        public Guid OrderId { get; set; }

        public bool BlowUp { get; set; }
    }
}