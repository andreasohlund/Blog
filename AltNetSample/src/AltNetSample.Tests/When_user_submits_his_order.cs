using System;
using AltnetSample.Messages;
using AltnetSample.OrderInput;
using NServiceBus;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltNetSample.Tests
{
    [TestFixture]
    public class When_user_submits_his_order
    {
        [Test]
        public void A_comple_Sale_command_should_be_sent_to_our_backend()
        {
            var bus = MockRepository.GenerateStub<IBus>();
            var orderDispatcher = new OrderDispatcher(bus);

            orderDispatcher.Place(34);

            bus.AssertWasSent<CompleteSaleCommand>(x=>x.ProductID == 34);
        }
    }
}