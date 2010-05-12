using System;
using NServiceBus;

namespace AltnetSample.OrderService
{
    public class HelloWorld:IWantToRunAtStartup
    {
        public void Run()
        {
            Console.WriteLine("Hello world");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}