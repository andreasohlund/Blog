namespace ShippingService.Tests
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;

    public class by_our_saga
    {
        protected Guid SagaId;
        protected BookShipment BookingRequest;
         
        [SetUp]
        public void Setup()
        {
            Test.Initialize();
            SagaId = Guid.NewGuid();

            BookingRequest = new BookShipment
                                 {
                                     OrderID = Guid.NewGuid()
                                 };
        }

    }
}