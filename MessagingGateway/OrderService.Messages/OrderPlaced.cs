namespace OrderService.Messages
{
    using System;
    using NServiceBus;

    public interface OrderPlaced:IMessage
    {
        Guid OrderID { get; set; }
    }
}
