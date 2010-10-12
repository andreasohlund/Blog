using System;

namespace ChildContainer.Entities
{
    public class PersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Data { get; set; }
    }
}