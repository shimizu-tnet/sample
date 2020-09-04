using System.Collections.Generic;

namespace JuniorTennis.SeedWork
{
    public abstract class EntityBase
    {
        public int Id { get; set; }

        public List<DomainEventBase> Events = new List<DomainEventBase>();
    }
}
