using System.Threading.Tasks;

namespace JuniorTennis.SeedWork
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(DomainEventBase domainEvent);
    }
}
