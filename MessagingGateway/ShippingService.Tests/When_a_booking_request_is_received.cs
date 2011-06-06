namespace ShippingService.Tests
{
    using System;
    using NServiceBus.Saga;
    using NServiceBus.Testing;
    using NUnit.Framework;

    public class When_a_booking_request_is_received: by_our_saga
    {
        [Test]
        public void A_fedex_request_should_be_dispatched()
        {       
            Test.Saga<BookShipmentSaga>(SagaId)
                .ExpectSend<RequestShipmentFromFedex>(m => m.OrderId == BookingRequest.OrderID)
                .When(os => os.Handle(BookingRequest));
        }

    }



    public class When_the_fedex_receipt_is_received : by_our_saga
    {
        [Test]
        public void A_timeout_for_T_should_be_set()
        {
            var pickupTime = DateTime.Now.AddDays(1);

            Test.Saga<BookShipmentSaga>(SagaId)
                .ExpectSend<TimeoutMessage>(m => m.SagaId == SagaId && m.Expires >= pickupTime)
                .When(os => os.Handle(new FedexResponseReceiptReceived
                                          {
                                              OrderID = BookingRequest.OrderID,
                                              PickupTime = pickupTime
                                          }));
        }

    }
}
