namespace ShippingService
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class BookShipmentSaga : Saga<BookShipmentSagaData>,
        IAmStartedByMessages<BookShipment>,
        IHandleMessages<FedexResponseReceiptReceived>,
        IHandleMessages<FailedToRequestShipmentFromFedex>,
        IHandleMessages<FedexShippingDetailsReceived>
    {
        public void Handle(BookShipment message)
        {
            Data.OrderId = message.OrderID;

            Bus.Send<RequestShipmentFromFedex>(m =>
                {
                    m.OrderId = Data.OrderId;
                });
        }

   
        public void Handle(FedexResponseReceiptReceived message)
        {
            Data.Receipt = message.Receipt;

            RequestTimeout(message.PickupTime, null);
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<FailedToRequestShipmentFromFedex>
                (s => s.OrderId, m => m.OrderID);
            ConfigureMapping<FedexResponseReceiptReceived>
                (s => s.OrderId, m => m.OrderID);
            ConfigureMapping<FedexShippingDetailsReceived>(s => s.Receipt, m => m.Receipt);
        }
        public override void Timeout(object state)
        {
            if (Data.Receipt != null)
            {
                Bus.Send<FetchShipmentDetailsFromFedex>(m =>
                    {
                        m.Receipt = Data.Receipt;
                    });
                return;
            }

            Bus.Send<RequestShipmentFromFedex>(m =>
                {
                    m.OrderId = Data.OrderId;
                });
        }


        public void Handle(FedexShippingDetailsReceived message)
        {
            ReplyToOriginator<ShipmentBooked>(m =>
                {
                    m.OrderID = Data.OrderId;
                    m.TrackingCode = message.TrackingCode;
                });

            MarkAsComplete();
        }


        public void Handle(FailedToRequestShipmentFromFedex message)
        {
            Data.NumRetries++;

            if (MaxRetriesReached())
            {
                Bus.Send<BookShippingManuallyForOrder>(m =>
                    {
                        m.OrderID = Data.OrderId;
                    });

                MarkAsComplete();

                return;
            }

            RequestTimeout(TimeSpan.FromMinutes(1), null);
        }

        bool MaxRetriesReached()
        {
            return Data.NumRetries > 60;
        }






       

    }

    public class FedexShippingDetailsReceived : IMessage
    {
        public string Receipt { get; set; }

        public string TrackingCode { get; set; }
    }

    public class BookShippingManuallyForOrder : IMessage
    {
        public Guid OrderID { get; set; }
    }

    public class FetchShipmentDetailsFromFedex : IMessage
    {
        public string Receipt { get; set; }
    }

    public class FedexResponseReceiptReceived : IMessage
    {
        public string Receipt { get; set; }

        public DateTime PickupTime { get; set; }

        public Guid OrderID { get; set; }
    }

    public class FailedToRequestShipmentFromFedex : IMessage
    {
        public Guid OrderID { get; set; }
    }

    public class RequestShipmentFromFedex : IMessage
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