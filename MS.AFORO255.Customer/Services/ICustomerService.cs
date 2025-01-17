using MS.AFORO255.Customer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.AFORO255.Customer.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Models.Customer>> GetAllCustomersAsync();
        Task<Models.Customer> GetCustomerByIdAsync(int customerId);
        Task CreateCustomerAsync(Models.Customer customer);
        Task UpdateCustomerAsync(Models.Customer customer);
        bool Exists(int customerId);
        Task DeleteCustomerAsync(int id);
    }
}
