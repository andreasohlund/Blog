namespace MyServer
{
    using System;
    using Messages;
    using NServiceBus;

    public class Runner:IWantToRunAtStartup
    {
        public IBus Bus { get; set; }

        public void Run()
        {
            Console.WriteLine("Press 'm' to send a message");
            Console.WriteLine("Press 'e' to send a message that will result in a exception");
        
            string cmd;

            while ((cmd = Console.ReadKey().Key.ToString().ToLower()) != "q")
            {
                switch (cmd)
                {
                    case "m":
                        SendRequest();
                        break;
                    case "e":
                        SendRequest(true);
                        break;
                }
            }
        }

        void SendRequest(bool blowUp = false)
        {
            var orderId = Guid.NewGuid();

            Bus.SendLocal<PlaceOrder>(m =>
            {
                m.OrderId = orderId;
                m.BlowUp = blowUp;
            });
            Console.WriteLine("Request sent, order id: " + orderId); 
        }


        public void Stop()
        {
        }
    }
}