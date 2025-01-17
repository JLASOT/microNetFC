using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Aforo255.Cross.Http.Src;
using Newtonsoft.Json.Linq;

namespace MS.AFORO255.Sale.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IHttpClient _httpClient;
        private readonly IConfiguration _configuration;
        //private readonly string _customerServiceUrl;

        public CustomerService(IHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;


            //_customerServiceUrl = configuration["proxy:urlCustomer"];
        }

        public async Task<bool> ValidateCustomer(int customer_id)
        {
            try
            {

                string uri = _configuration["proxy:urlCustomer"];
                // Construye la URL de manera dinámica utilizando el customer_id
                var url = $"{uri}/{customer_id}";

                // Realiza la solicitud GET y obtiene la respuesta
                var response = await _httpClient.GetStringAsync(url);

                // Verifica si la respuesta no está vacía
                if (!string.IsNullOrEmpty(response))
                {
                    // Deserializa la respuesta JSON
                    var jsonResponse = JObject.Parse(response);

                    // Verifica si contiene el mensaje "Cliente no encontrado"
                    var message = jsonResponse["message"]?.ToString();
                    if (message != null && message.Contains("Cliente no encontrado"))
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
                Console.WriteLine($"Error validating customer: {ex.Message}");
                return false;
            }
        }

        /*public async Task<string> GetCustomerName(int customerId)
        {
            try
            {
                string uri = _configuration["proxy:urlCustomer"];
                var url = $"{uri}/{customerId}";

                var response = await _httpClient.GetStringAsync(url);
                var jsonResponse = JObject.Parse(response);
                return jsonResponse["name"]?.ToString() ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }
        */
        public async Task<string> GetCustomerName(int customerId)
        {
            try
            {
                string uri = _configuration["proxy:urlCustomer"];
                var url = $"{uri}/{customerId}";

                var response = await _httpClient.GetStringAsync(url);

                // Si la respuesta es vacía o no contiene los campos esperados, devuelve "Desconocido"
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Respuesta vacía de la API.");
                    return "Desconocido";
                }

                var jsonResponse = JObject.Parse(response);

                // Concatenar los campos para formar el nombre completo
                var firstName = jsonResponse["firstName"]?.ToString() ?? string.Empty;
                var pSurname = jsonResponse["pSurname"]?.ToString() ?? string.Empty;
                var mSurname = jsonResponse["mSurname"]?.ToString() ?? string.Empty;

                // Devolver el nombre completo si está disponible
                var fullName = $"{firstName} {pSurname} {mSurname}".Trim();

                return string.IsNullOrEmpty(fullName) ? "Desconocido" : fullName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el nombre del cliente: {ex.Message}");
                return "Desconocido";
            }
        }


    }
}
