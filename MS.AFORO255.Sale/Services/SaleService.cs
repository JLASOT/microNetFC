using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Sale.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using MS.AFORO255.Sale.Models;
using System.Linq;
using System;



namespace MS.AFORO255.Sale.Services
{
    public class SaleService : ISaleService
    {

        private readonly ContextDatabase _contextDatabase;

        public SaleService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        // Sales CRUD
        public async Task<IEnumerable<Models.Sale>> GetAllSalesAsync()
        {
            return await _contextDatabase.Sales.Include(s => s.SaleDetails).ToListAsync();
        }

        public async Task<Models.Sale> GetSaleByIdAsync(int saleId)
        {
            return await _contextDatabase.Sales.Include(s => s.SaleDetails)
                                       .FirstOrDefaultAsync(s => s.SaleId == saleId);
        }

        public async Task AddSaleAsync(Models.Sale sale)
        {
            await _contextDatabase.Sales.AddAsync(sale);
            await _contextDatabase.SaveChangesAsync();
        }

        public async Task UpdateSaleAsync(Models.Sale updatedSale)
        {
            // Recuperar la venta existente con los detalles relacionados
            var existingSale = await _contextDatabase.Sales
                .Include(s => s.SaleDetails) // Incluir los detalles relacionados
                .FirstOrDefaultAsync(s => s.SaleId == updatedSale.SaleId);

            if (existingSale == null)
            {
                throw new InvalidOperationException("La venta no existe.");
            }

            // Actualizar las propiedades principales de la venta
            existingSale.CustomerId = updatedSale.CustomerId;
            existingSale.TotalAmount = 0;//updatedSale.TotalAmount;
            existingSale.SaleDate = updatedSale.SaleDate;

            // Actualizar detalles de la venta
            foreach (var updatedDetail in updatedSale.SaleDetails)
            {
                // Buscar si el detalle ya existe
                var existingDetail = existingSale.SaleDetails
                    .FirstOrDefault(d => d.SaleDetailId == updatedDetail.SaleDetailId);

                if (existingDetail != null)
                {
                    // Actualizar el detalle existente
                    existingDetail.ProductId = updatedDetail.ProductId;
                    existingDetail.Quantity = updatedDetail.Quantity;
                    existingDetail.SalePrice = updatedDetail.SalePrice;
                }
                else
                {
                    // Agregar un nuevo detalle si no existe
                    existingSale.SaleDetails.Add(new Models.SaleDetail
                    {
                        ProductId = updatedDetail.ProductId,
                        Quantity = updatedDetail.Quantity,
                        SalePrice = updatedDetail.SalePrice,
                        SaleId = existingSale.SaleId // Asignar la relación con la venta
                    });
                }
            }

            // Eliminar detalles que no están en la lista actualizada
            var detailsToRemove = existingSale.SaleDetails
                .Where(d => !updatedSale.SaleDetails
                    .Any(ud => ud.SaleDetailId == d.SaleDetailId))
                .ToList();

            _contextDatabase.SaleDetails.RemoveRange(detailsToRemove);
            // Calcular el monto total automáticamente
            existingSale.TotalAmount = existingSale.SaleDetails
            .Sum(d => d.Quantity * d.SalePrice); // Suma del total de cada detalle


            // Guardar los cambios
            await _contextDatabase.SaveChangesAsync();
        }

        public async Task DeleteSaleAsync(int saleId)
        {
            
                var saleDetails = _contextDatabase.SaleDetails
                                .Where(sd => sd.SaleId == saleId)
                                .ToListAsync();
                // Eliminar los detalles de la venta
                _contextDatabase.SaleDetails.RemoveRange(await saleDetails);
            
            var sale = await _contextDatabase.Sales.FindAsync(saleId);
                _contextDatabase.Sales.Remove(sale);
                await _contextDatabase.SaveChangesAsync();
        }

        // SaleDetails CRUD
        public async Task AddSaleDetailAsync(SaleDetail saleDetail)
        {
            _contextDatabase.SaleDetails.Add(saleDetail);
            await _contextDatabase.SaveChangesAsync();
        }

        public async Task<IEnumerable<SaleDetail>> GetSaleDetailsBySaleIdAsync(int saleId)
        {
            return await _contextDatabase.SaleDetails.Where(sd => sd.SaleId == saleId).ToListAsync();
        }

        public double CalculateTotalAmount(Models.Sale sale) {
            double totalAmount = 0;
            foreach (var saleDetail in sale.SaleDetails)
            {
                totalAmount += saleDetail.SalePrice * saleDetail.Quantity;
            }
            return totalAmount;
        }



        //reporte
        public async Task<IEnumerable<Models.Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _contextDatabase.Sales
                .Where(sale => sale.SaleDate >= startDate && sale.SaleDate <= endDate)
                .ToListAsync();
        }
    }
}
