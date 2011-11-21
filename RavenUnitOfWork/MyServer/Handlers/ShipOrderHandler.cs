namespace MyServer.Handlers
{
    using System;
    using Messages;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Document;

    public class ShipOrderHandler:IHandleMessages<PlaceOrder>
    {
        readonly IDocumentSession session;

        public ShipOrderHandler(IDocumentSession session)
        {
            this.session = session;
        }

        public void Handle(PlaceOrder message)
        {
            Console.WriteLine(string.Format("SId({0}) - Saving shipment to raven", ((DocumentSession)session).Id));

            session.Store(new Shipment
                              {
                                  ShipmentId = Guid.NewGuid(),
                                  OrderId = message.OrderId
                              });
        }
    }

    public class Shipment
    {
        public Guid ShipmentId { get; set; }

        public Guid OrderId { get; set; }
    }
}