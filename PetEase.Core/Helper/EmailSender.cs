using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PetEase.Core.Helper
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _ssl;
        private readonly bool _isBodyHtml;

        public EmailSender(string email, string password, string host, bool ssl, int port, bool isBodyHtml)
        {
            _email = email;
            _password = password;
            _host = host;
            _ssl = ssl;
            _port = port;
            _isBodyHtml = isBodyHtml;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            using var message = new MailMessage(_email, to)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = _isBodyHtml
            };

            var credentials = new NetworkCredential(_email, _password);

            using var smtpClient = new SmtpClient(_host)
            {
                Port = _port,
                Credentials = credentials,
                EnableSsl = _ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            await smtpClient.SendMailAsync(message);
        }

    }
}
