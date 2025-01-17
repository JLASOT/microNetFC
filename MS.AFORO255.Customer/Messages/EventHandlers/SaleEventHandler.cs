using Aforo255.Cross.Event.Src.Bus;
using MS.AFORO255.Customer.Messages.Events;
using System.Threading.Tasks;

namespace MS.AFORO255.Customer.Messages.EventHandlers
{
    public class SaleEventHandler : IEventHandler<SaleCreatedEvent>
    {
        public Task Handle(SaleCreatedEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
