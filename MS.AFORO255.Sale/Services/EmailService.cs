using Microsoft.Extensions.Configuration;
using System.IO;
//using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using MailKit.Net.Smtp;
using MimeKit;


namespace MS.AFORO255.Sale.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* public async Task SendEmailWithAttachment(string toEmail, string subject, string body, byte[] attachment, string attachmentName)
         {
             Console.WriteLine($"Email original: '{toEmail}'");

             toEmail = toEmail.Trim();
             Console.WriteLine($"Email después de trim: '{toEmail}'");

             toEmail = Regex.Replace(toEmail, @"[\x00-\x1F\x7F\u200B]", "");
             Console.WriteLine($"Email después de limpieza: '{toEmail}'");

             foreach (char c in toEmail)
             {
                 Console.WriteLine($"Char: '{c}', Hex: {((int)c).ToString("X2")}");
             }

             if (!IsValidEmail(toEmail))
             {
                 Console.WriteLine($"Error: El correo '{toEmail}' no es válido.");
                 throw new ArgumentException("La dirección de correo electrónico no es válida.");
             }

             try
             {
                 var smtpHost = _configuration["EmailSettings:SmtpHost"];
                 var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                 var smtpUser = _configuration["EmailSettings:SmtpUser"];
                 var smtpPass = _configuration["EmailSettings:SmtpPass"];

                 Console.WriteLine($"Configuración SMTP: Host={smtpHost}, Port={smtpPort}, User={smtpUser}");

                 using var client = new SmtpClient(smtpHost, smtpPort)
                 {
                     Credentials = new NetworkCredential(smtpUser, smtpPass),
                     EnableSsl = true
                 };

                 using var message = new MailMessage
                 {
                     From = new MailAddress(smtpUser),
                     Subject = subject,
                     Body = body,
                     IsBodyHtml = true
                 };

                 message.To.Add(toEmail);
                 Console.WriteLine($"Añadida dirección de correo: {toEmail}");

                 using var attachmentStream = new MemoryStream(attachment);
                 message.Attachments.Add(new Attachment(attachmentStream, attachmentName, "application/pdf"));

                 Console.WriteLine("Enviando el correo...");
                 await client.SendMailAsync(message);
                 Console.WriteLine("Correo enviado exitosamente.");
             }
             catch (SmtpException smtpEx)
             {
                 Console.WriteLine($"Error al enviar el correo: {smtpEx.Message}");
                 throw;
             }
         }*/





        public async Task SendEmailWithAttachment(string toEmail, string subject, string body, byte[] attachmentBytes, string attachmentName)
        {
            // Crear el mensaje de correo
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tu Nombre", _configuration["EmailSettings:SmtpUser"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            // Construir el cuerpo del correo
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body // Cuerpo del correo en formato HTML
            };

            // Adjuntar el archivo si existe
            if (attachmentBytes != null)
            {
                bodyBuilder.Attachments.Add(attachmentName, attachmentBytes, ContentType.Parse("application/pdf"));
            }

            message.Body = bodyBuilder.ToMessageBody();

            // Configurar el cliente SMTP y enviar el correo
            using (var client = new SmtpClient())
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPass = _configuration["EmailSettings:SmtpPass"];

                await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUser, smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }







        public bool EsCorreoValido(string correo)
        {
            // Expresión regular para validar el formato de un correo electrónico
            var regex = new System.Text.RegularExpressions.Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            return regex.IsMatch(correo);
        }
        /*public async Task SendEmailWithAttachment(string toEmail, string subject, string body, byte[] attachment, string attachmentName)
        {
            MostrarBytesDelCorreo(toEmail);
            try
            {
                Console.WriteLine($"Dirección de correo electrónico proporcionada: '{toEmail}'");
                // Limpiar la dirección de correo electrónico (si es necesario)
                toEmail = LimpiarCorreo(toEmail);

                // Verificar si el correo es válido antes de proceder
                if (!IsValidEmail(toEmail))
                {
                    Console.WriteLine($"Correo inválido: '{toEmail}'");
                    throw new InvalidOperationException($"El correo '{toEmail}' no tiene un formato válido.");
                }

                // Configuración SMTP
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPass = _configuration["EmailSettings:SmtpPass"];

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                // Crear el mensaje de correo sin usar MailAddress para el destinatario
                var message = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // Agregar destinatario sin validación
                message.To.Add(toEmail);

                // Adjuntar el archivo
                using var attachmentStream = new MemoryStream(attachment);
                message.Attachments.Add(new Attachment(attachmentStream, attachmentName, "application/pdf"));

                // Enviar el correo electrónico
                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
                throw new InvalidOperationException($"Error SMTP: {ex.Message}", ex);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error de formato del correo: {ex.Message}");
                throw new InvalidOperationException($"Formato del correo incorrecto: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error de argumento: {ex.Message}");
                throw new InvalidOperationException($"Argumento inválido: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                throw new InvalidOperationException($"Error inesperado: {ex.Message}", ex);
            }
        }
        */
        // Método para validar el correo electrónico con expresión regular
        private bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }
        private string LimpiarCorreo(string email)
        {
            // Eliminar espacios en blanco y caracteres no imprimibles
            email = Regex.Replace(email, @"\s+", ""); // Elimina todos los espacios en blanco
            email = Regex.Replace(email, @"[^\u0020-\u007E]", ""); // Elimina caracteres no imprimibles
            return email.Trim();
        }

        // Este método se puede invocar para verificar y limpiar el correo
        private void MostrarBytesDelCorreo(string email)
        {
            byte[] emailBytes = Encoding.UTF8.GetBytes(email);
            Console.WriteLine("Bytes del correo: " + BitConverter.ToString(emailBytes)); // Muestra los bytes del correo

            // Mostrar el correo después de la limpieza
            email = email.Trim();  // Eliminar espacios en blanco
            email = Regex.Replace(email, @"[^\x20-\x7E]", "");  // Eliminar caracteres no imprimibles

            Console.WriteLine($"Correo limpio: '{email}'");  // Mostrar el correo después de limpiarlo
        }


        /* private bool IsValidEmail(string email)
         {
             try
             {
                 var addr = new MailAddress(email);
                 return addr.Address == email;
             }
             catch (FormatException ex)
             {
                 Console.WriteLine($"Formato de correo inválido: {email}");
                 Console.WriteLine($"Detalles del error: {ex.Message}");
                 return false;
             }
         }*/

        // Validación del correo electrónico utilizando una expresión regular robusta
        /*private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var isValid = Regex.IsMatch(email, emailRegex);
            if (!isValid)
            {
                Console.WriteLine($"Correo no válido según la regex: {email}");
            }
            return isValid;
        }*/
    }
}
