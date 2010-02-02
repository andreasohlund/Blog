using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace NHibernateSample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Configure.With()
                .SpringBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .LoadMessageHandlers()
                .CreateBus()
                .Start();

            Console.WriteLine("Bus started, type a message to send");
            Console.WriteLine("Start with -m to invoke the messagemodule implementation)");
            Console.WriteLine("Start with -c to invoke the container cache mode implementation)");

            Console.WriteLine("Include the word 'throw' to cause the message handler to throw an exception and force a rollback");
            string m = "";

            while((m= Console.ReadLine()) != "")
            {
                IMessage messageToSend = new SaveToDatabaseUsingRawSession { DataToPersist = m,Throw = m.Contains("throw") };

                if (m.StartsWith("-m "))
                    messageToSend = new SaveToDatabaseUsingMessageModule { DataToPersist = m.Substring(3), Throw = m.Contains("throw") };
               
                if (m.StartsWith("-c "))
                    messageToSend = new SaveToDatabaseUsingContainerCacheMode { DataToPersist = m.Substring(3), Throw = m.Contains("throw") };
                
                    
                bus.Send("PersisterServiceInputQueue", messageToSend);
                Console.WriteLine("Message sent: " + m + " - MessageType:" + messageToSend.GetType().Name);
            }
          
            
        }
    }
}
