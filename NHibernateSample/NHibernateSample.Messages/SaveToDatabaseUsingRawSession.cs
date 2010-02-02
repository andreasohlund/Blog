using NServiceBus;

namespace NHibernateSample
{
    public class SaveToDatabaseUsingRawSession:IMessage
    {
        public string DataToPersist { get; set; }
        public bool Throw { get; set; }
    }
}
