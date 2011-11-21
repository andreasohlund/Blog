namespace MyServer.Handlers
{
    using System;
    using Messages;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Document;

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        readonly IDocumentSession session;

        public PlaceOrderHandler(IDocumentSession session)
        {
            this.session = session;
        }


        public void Handle(PlaceOrder message)
        {
            Console.WriteLine(string.Format("SId({0}) - Saving order to raven", ((DocumentSession)session).Id));

            session.Store(new Order
            {
                OrderId = message.OrderId
            });
            
            if(message.BlowUp)
                throw new Exception("There was a problem with the order");
        }
    }

    public class Order
    {
        public Guid OrderId { get; set; }
    }
}