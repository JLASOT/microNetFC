using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Aforo255.Cross.Http.Src;
using Newtonsoft.Json.Linq;

namespace MS.AFORO255.Sale.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClient _httpClient;
        private readonly IConfiguration _configuration;
        //private readonly string _customerServiceUrl;

        public ProductService(IHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;


            //_customerServiceUrl = configuration["proxy:urlCustomer"];
        }

        public async Task<bool> ValidateProduct(int product_id)
        {
            try
            {

                string uri = _configuration["proxy:urlProduct"];
                // Construye la URL de manera dinámica utilizando el customer_id
                var url = $"{uri}/validate/{product_id}";

                // Realiza la solicitud GET y obtiene la respuesta
                var response = await _httpClient.GetStringAsync(url);

                // Verifica si la respuesta no está vacía
                if (!string.IsNullOrEmpty(response))
                {
                    // Deserializa la respuesta JSON
                    var jsonResponse = JObject.Parse(response);

                    // Verifica si contiene el mensaje "producto no encontrado"
                    var message = jsonResponse["message"]?.ToString();
                    if (message != null && message.Contains("Producto no encontrado"))
                    {
                        // Si el cliente no es encontrado, retorna false
                        return false;
                    }

                    // Si la respuesta es válida, retorna true
                    return true;
                }

                // Si la respuesta está vacía, retorna falso
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error validating producto: {ex.Message}");
                return false;
            }
        
        }


    }
}
