using Aforo255.Cross.Event.Src.Bus;
using MediatR;
using MS.AFORO255.Sale.Messages.Commands;
using MS.AFORO255.Sale.Messages.Events;
using MS.AFORO255.Sale.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MS.AFORO255.Sale.Messages.CommandHandlers
{
    public class SaleCommandHandler: IRequestHandler<SaleCreateCommand, bool>
    {
        private readonly IEventBus _bus;
        public SaleCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }

        public Task<bool> Handle(SaleCreateCommand request, CancellationToken cancellationToken)
        {
            // Convertir los detalles de la venta a DTOs
            var saleDetails = request.SaleDetails
                .Select(detail => new SaleDetail
                {
                    ProductId = detail.ProductId,
                    SalePrice = detail.SalePrice,
                    Quantity = detail.Quantity,
                    SaleId = detail.SaleId,
                }).ToList();

           /* _bus.Publish(new SaleCreatedEvent(
                    request.Sale_ID,
                    request.Sale_Date ,
                    request.Total_Amount,
                    request.Customer_Id,
                    saleDetails: saleDetails

                ));
           */
            
            return Task.FromResult(true);

        }
    }
}
