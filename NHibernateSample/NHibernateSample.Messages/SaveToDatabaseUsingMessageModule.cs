using NServiceBus;

namespace NHibernateSample
{
    public class SaveToDatabaseUsingMessageModule : IMessage
    {
        public string DataToPersist { get; set; }
        public bool Throw { get; set; }
    }
}