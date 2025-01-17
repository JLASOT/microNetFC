using Aforo255.Cross.Event.Src.Bus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MS.AFORO255.Sale.Messages.Commands;
using MS.AFORO255.Sale.Messages.Events;
using MS.AFORO255.Sale.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.AFORO255.Sale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IEventBus _bus;
        private readonly ISaleService _saleService;
        private readonly ICustomerService _customerService;  // Inyectar el servicio de cliente
        private readonly IProductService _productService;  // Inyectar el servicio de Producto
        private readonly IConfiguration _configuration; // Campo para la configuración

        public SaleController(IEventBus bus,ISaleService saleService, ICustomerService customerService, IProductService productService, IConfiguration configuration)
        {
            _bus = bus;
            _saleService = saleService;
            _customerService = customerService;  // Inicializar la dependencia
            _productService = productService;  // Inicializar la dependencia
            _configuration = configuration; // Inicializar el campo de configuración
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<IEnumerable<Models.Sale>> GetSales()
        {
            return await _saleService.GetAllSalesAsync();
        }

        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Sale>> GetSale(int id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return sale;
        }

        // POST: api/Sale
        [HttpPost]
        public async Task<ActionResult<Models.Sale>> PostSale(Models.Sale sale)
        {
            if (sale == null || sale.SaleDetails == null || !sale.SaleDetails.Any())
                return BadRequest("datos de venta invalido");

            // Validar si el cliente existe y es válido antes de procesar la venta
            var customerValid = await _customerService.ValidateCustomer(sale.CustomerId);
            if (!customerValid)
            {
                return BadRequest(new { Message = "El cliente no es válido." });
            }
            // Validar si el Producto existe y es válido antes de procesar la venta
            //foreach (var saleDetail in sale.SaleDetails)
            //{
            //    var isAvailable = await _productService.ValidateProduct(saleDetail.ProductId);
            //    if (!isAvailable)
            //    {
            //        return BadRequest(new { Message = $"El producto con id : {saleDetail.ProductId} no fue encotrado o no existe" });
            //    }
            //}
            //foreach (var saleDetail in sale.SaleDetails)
            //{
            //    var isStockAvailable = await _productService.CheckStockAsync(saleDetail.ProductId, saleDetail.Quantity);
            //    if (!isStockAvailable)
            //    {
            //        return BadRequest(new { Message = $"Stock insuficiente para el producto {saleDetail.ProductId}" });
            //    }
            //}
            double totalAmount = 0;

            // Asignar el totalAmount al objeto sale y Calcular el totalAmount
            sale.TotalAmount = _saleService.CalculateTotalAmount(sale);

            // Guardar la venta en la base de datos
            await _saleService.AddSaleAsync(sale);

            var saleCreateCommand = new SaleCreateCommand(
                sale_id: sale.SaleId,
                sale_date: sale.SaleDate,
                total_amount: sale.TotalAmount,
                customer_id: sale.CustomerId,
                saleDetails: sale.SaleDetails

            );
            // Publicar mensaje en RabbitMQ
            _bus.SendCommand( saleCreateCommand );


            return CreatedAtAction(nameof(GetSale), new { id = sale.SaleId }, sale);
        }

       

        // PUT: api/Sale/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Models.Sale sale)
        {
            if (id != sale.SaleId)
            {
                return BadRequest();
            }

            await _saleService.UpdateSaleAsync(sale);
            return NoContent();
        }

        // DELETE: api/Sale/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            await _saleService.DeleteSaleAsync(id);
            return NoContent();
        }



        [HttpGet("report")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            var sales = await _saleService.GetSalesByDateRangeAsync(startDate, endDate);

            var report = sales.Select(sale => new
            {
                CustomerName = _customerService.GetCustomerName(sale.CustomerId).Result, // Obtener nombre del cliente
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount
            });

            return Ok(report);
        }


        //, [FromQuery] string email
        [HttpPost("send-pdf-report")]
        public async Task<IActionResult> SendSalesReportPdf([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string email)
        {

            // Validar el formato del correo electrónico
            if (!EsCorreoValido(email))
            {
                return BadRequest("La dirección de correo electrónico proporcionada no es válida.");
            }

            var sales = await _saleService.GetSalesByDateRangeAsync(startDate, endDate);

            if (!sales.Any())
            {
                return NotFound("No hay ventas en el rango de fechas especificado.");
            }

            var pdfService = new PdfService();
            var pdfBytes = pdfService.GenerateSalesReportPdf(sales.Select(s => new
            {
                CustomerName = _customerService.GetCustomerName(s.CustomerId).Result,
                SaleDate = s.SaleDate,
                TotalAmount = s.TotalAmount
            }));

            var emailService = new EmailService(_configuration);
            await emailService.SendEmailWithAttachment(email, "Reporte de Ventas", "Adjunto el reporte solicitado.", pdfBytes, "ReporteVentas.pdf");
            //return File(pdfBytes, "application/pdf", "ReporteVentas.pdf");
            return Ok(new { Message = "El reporte se envió correctamente." });
        }



        public bool EsCorreoValido(string correo)
        {
            try
            {
                var direccion = new System.Net.Mail.MailAddress(correo);
                return direccion.Address == correo;
            }
            catch
            {
                return false;
            }
        }




    }
}
