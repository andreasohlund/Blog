using System;
using AltnetSample.Messages;
using NServiceBus;

namespace AltnetSample.OrderService
{
    public class CompleteSaleHandler:IHandleMessages<CompleteSaleCommand>
    {
        public void Handle(CompleteSaleCommand message)
        {
            Console.WriteLine(message.ProductID);
        }
    }
}