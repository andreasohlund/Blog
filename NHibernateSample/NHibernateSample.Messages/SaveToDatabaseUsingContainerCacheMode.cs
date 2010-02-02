using NServiceBus;

namespace NHibernateSample
{
    public class SaveToDatabaseUsingContainerCacheMode : IMessage
    {
        public string DataToPersist { get; set; }
        public bool Throw { get; set; }
    }
}