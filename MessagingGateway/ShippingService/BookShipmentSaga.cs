namespace ShippingService
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class BookShipmentSaga : Saga<BookShipmentSagaData>,
        IAmStartedByMessages<BookShipment>,
        IHandleMessages<FedexResponseReceiptReceived>,
        IHandleMessages<FailedRequestingShipmentFromFedex>,
        IHandleMessages<ShippingDetailsReceivedFromFedex>

    {
        public void Handle(BookShipment message)
        {
            Data.OrderId = message.OrderID;

            RequestShipmentFromFedex();
        }

        public void Handle(FailedRequestingShipmentFromFedex message)
        {
            Data.NumRetries++;

            if(MaxRetriesReached())
            {
                Bus.Send<BookShippingManuallyForOrder>(m =>{ m.OrderID = Data.OrderId;});

                MarkAsComplete();

                return;
            }

            RequestTimeout(TimeSpan.FromMinutes(1),null);
        }

        bool MaxRetriesReached()
        {
            return Data.NumRetries > 60;
        }

        public override void Timeout(object state)
        {
            if (Data.Receipt != null)
            {
                Bus.Send<FetchShipmentDetailsFromFedex>(m => { m.Receipt = Data.Receipt;});
                return;
            }

            RequestShipmentFromFedex();
        }

        
        public void Handle(FedexResponseReceiptReceived message)
        {
            Data.Receipt = message.Receipt;

            RequestTimeout(message.PickupTime,null);
        }

        public void Handle(ShippingDetailsReceivedFromFedex message)
        {
            ReplyToOriginator<ShipmentBooked>(m=>
                {
                    m.OrderID = Data.OrderId;
                    m.TrackingCode = message.TrackingCode;
                });

            MarkAsComplete();
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<FailedRequestingShipmentFromFedex>(s => s.OrderId, m => m.OrderID);
            ConfigureMapping<FedexResponseReceiptReceived>(s => s.OrderId, m => m.OrderID);
            ConfigureMapping<ShippingDetailsReceivedFromFedex>(s => s.Receipt, m => m.Receipt);
        }



        void RequestShipmentFromFedex()
        {
            Bus.Send<RequestShipmentFromFedex>(m =>{ m.OrderId = Data.OrderId;});
        }

    }

    public class ShippingDetailsReceivedFromFedex : IMessage
    {
        public string Receipt { get; set; }

        public string TrackingCode { get; set; }
    }

    public class BookShippingManuallyForOrder:IMessage
    {
        public Guid OrderID { get; set; }
    }

    public class FetchShipmentDetailsFromFedex:IMessage
    {
        public string Receipt { get; set; }
    }

    public class FedexResponseReceiptReceived : IMessage
    {
        public string Receipt { get; set; }

        public DateTime PickupTime { get; set; }

        public object OrderID { get; set; }
    }

    public class FailedRequestingShipmentFromFedex : IMessage
    {
        public Guid OrderID { get; set; }
    }

    public class RequestShipmentFromFedex:IMessage
    {
        public Guid OrderId { get; set; }
    }

    public class BookShipmentSagaData : ISagaEntity
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public Guid OrderId { get; set; }

        public string Receipt { get; set; }

        public int NumRetries { get; set; }
    }
}