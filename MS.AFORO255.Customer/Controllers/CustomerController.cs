using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Customer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.AFORO255.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        // Obtener todos los clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Customer>>> GetCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // Obtener un cliente por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Customer>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                // Si no se encuentra el cliente, devolver un 404 Not Found
                return NotFound(new { Message = "Cliente no encontrado" });
            }
            return Ok(customer);
        }

        

        // Crear un nuevo cliente
        [HttpPost]
        public async Task<ActionResult> CreateCustomer(Models.Customer customer)
        {
            await _customerService.CreateCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        // Actualizar un cliente
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, Models.Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            await _customerService.UpdateCustomerAsync(customer);
            return NoContent();
        }

        // Eliminar un cliente
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }

        // Validar un cliente
        [HttpGet("{id}/validate")]
        public async Task<IActionResult> ValidateCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { Message = "Cliente no encontrado." });
            }

            //bool isValid = ValidateCustomerLogic(customer);
            if (customer.CustomerId != id)
            {
                return BadRequest(new { Message = "El cliente no es válido." });
            }

            return Ok(new { Message = "El cliente es válido." });
        }

        // Lógica de validación del cliente (aquí puedes agregar las reglas reales)
        private bool ValidateCustomerLogic(Models.Customer customer)
        {
            // Ejemplo de validación:
            // Puede ser una regla basada en el estado, edad, tipo de cliente, etc.
            return true;// customer.Status == "Active";  // Solo un ejemplo
        }
    }
}
