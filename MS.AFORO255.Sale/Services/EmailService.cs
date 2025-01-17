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


                // Desactivar la validación del certificado
                client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;


                await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                //await client.AuthenticateAsync(smtpUser, smtpPass);
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
      
       


    }
}
