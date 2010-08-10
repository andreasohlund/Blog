using NServiceBus;

namespace SendOnlyEndpoint.Custom
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bus = Configure.With()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .CreateBus()
                .Start();

            bus.Send("SendOnlyDestination",new TestMessage());
        }
    }

    public class TestMessage : IMessage
    {
    }
}
