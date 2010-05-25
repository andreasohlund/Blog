using NServiceBus;
using StructureMap.Pipeline;

namespace NHibernateSample.PersisterService
{
    public class NServiceBusThreadLocalStorageLifestyle : ThreadLocalStorageLifecycle,  IMessageModule
    {
        public void HandleBeginMessage(){}

        public void HandleEndMessage()
        {
            EjectAll();
        }

        public void HandleError(){}
    }
}