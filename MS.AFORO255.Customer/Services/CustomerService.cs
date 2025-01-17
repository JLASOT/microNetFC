using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Customer.Repositories;
using System.Collections.Generic;
using MS.AFORO255.Customer.Models;
using System.Threading.Tasks;
using Polly;
using System.Linq;

namespace MS.AFORO255.Customer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ContextDatabase _contextDatabase;

        public CustomerService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        // Obtener todos los clientes
        public async Task<IEnumerable<Models.Customer>> GetAllCustomersAsync()
        {
            return await _contextDatabase.Customers.ToListAsync();
        }

        // Obtener un cliente por su ID
        public async Task<Models.Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _contextDatabase.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        // Crear un nuevo cliente
        public async Task CreateCustomerAsync(Models.Customer customer)
        {
            _contextDatabase.Customers.Add(customer);
            await _contextDatabase.SaveChangesAsync();
        }

        // Actualizar un cliente existente
        public async Task UpdateCustomerAsync(Models.Customer customer)
        {
            _contextDatabase.Customers.Update(customer);
            await _contextDatabase.SaveChangesAsync();
        }

        // Eliminar un cliente
        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _contextDatabase.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer != null)
            {
                _contextDatabase.Customers.Remove(customer);
                await _contextDatabase.SaveChangesAsync();
            }
        }

        public bool Exists(int customerId)
        {
            return _contextDatabase.Customers.Any(c => c.CustomerId == customerId);
        }
    }
}
