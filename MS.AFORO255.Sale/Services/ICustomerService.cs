using System.Threading.Tasks;

namespace MS.AFORO255.Sale.Services
{
    public interface ICustomerService
    {
        Task<bool> ValidateCustomer(int customer_id);
        Task<string> GetCustomerName(int customer_id);  // Agregar este método
    }





}
