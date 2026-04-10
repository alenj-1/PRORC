using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Interfaces.Integrations;

namespace PRORC.Infrastructure.Integrations.Notifications
{
    public class EmailNotificationService : INotificationSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IConfiguration configuration, ILogger<EmailNotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // Envía un correo electrónico usando configuración SMTP
        public async Task SendAsync(string to, string subject, string message)
        {
            try
            {
                // Lee la configuración desde appsettings.json
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPortValue = _configuration["EmailSettings:SmtpPort"];
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                var enableSslValue = _configuration["EmailSettings:EnableSsl"];

                // Si falta configuración, el sistema no se rompe. Solo registra un warning y sale
                if (string.IsNullOrWhiteSpace(smtpHost) ||
                    string.IsNullOrWhiteSpace(smtpPortValue) ||
                    string.IsNullOrWhiteSpace(fromEmail))
                {
                    _logger.LogWarning(
                        "Email settings are incomplete. The notification email was not sent to {To}.",
                        to);

                    return;
                }

                // Convertimos valores necesarios
                var smtpPort = int.Parse(smtpPortValue);
                var enableSsl = bool.TryParse(enableSslValue, out var parsedSsl) && parsedSsl;

                // Creación del mensaje
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName ?? "ChefCheck's"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false
                };

                // Se agrega el destinatario
                mailMessage.To.Add(to);

                // Creamos el cliente SMTP
                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = enableSsl
                };

                // Si hay usuario y contraseña, los usamos
                if (!string.IsNullOrWhiteSpace(smtpUser) && !string.IsNullOrWhiteSpace(smtpPassword))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                }

                // Enviamos el correo
                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Notification email sent successfully to {To}.", to);
            }
            catch (Exception ex)
            {
                // No cambiamos la firma de la interfaz, así que registramos y relanzamos
                // Luego Application decidirá si tumba o no el flujo principal
                _logger.LogError(ex, "Error sending notification email to {To}.", to);
                throw;
            }
        }
    }
}