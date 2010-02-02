using System;

namespace NHibernateSample.PersisterService.Entities
{
    public class PersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Data { get; set; }
    }
}