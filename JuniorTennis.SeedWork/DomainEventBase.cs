using System;

namespace JuniorTennis.SeedWork
{
    public abstract class DomainEventBase
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
