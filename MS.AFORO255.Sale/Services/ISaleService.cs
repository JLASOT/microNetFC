using MS.AFORO255.Sale.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.AFORO255.Sale.Services
{
    public interface ISaleService
    {
        Task<IEnumerable<Models.Sale>> GetAllSalesAsync();
        Task<Models.Sale> GetSaleByIdAsync(int saleId);
        Task AddSaleAsync(Models.Sale sale);
        Task UpdateSaleAsync(Models.Sale sale);
        Task DeleteSaleAsync(int saleId);

        Task AddSaleDetailAsync(SaleDetail saleDetail);
        Task<IEnumerable<SaleDetail>> GetSaleDetailsBySaleIdAsync(int saleId);

        public double CalculateTotalAmount(Models.Sale sale);

        Task<IEnumerable<Models.Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}
