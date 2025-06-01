using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MediaStore.Services.Email
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }
        public async Task SendOrderConfirmationEmail(string toEmail, string orderCode, string customerName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress(customerName, toEmail));
            message.Subject = $"Xac nhan don hang #{orderCode}";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <h3>Hello {customerName},</h3>
                    <p>Your order <strong>{orderCode}</strong> has been successfully paid.</p>
                    <p>We will process and deliver it as soon as possible.</p>
                    <p>Best regards!</p>
                    "
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}