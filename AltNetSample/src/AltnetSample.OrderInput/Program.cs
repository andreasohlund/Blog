using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace AltnetSample.OrderInput
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = "";
            IBus bus = NServiceBus.Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .IsTransactional(true)
                .UnicastBus()
                .LoadMessageHandlers()
                .CreateBus()
                .Start();

            OrderDispatcher orderDispatcher = new OrderDispatcher(bus);

            Console.WriteLine("Please enter a product id");
            while((line = Console.ReadLine()) != "q")
            {
                int productId = int.Parse(line);

                orderDispatcher.Place(productId);
            }
        }
    }
}
