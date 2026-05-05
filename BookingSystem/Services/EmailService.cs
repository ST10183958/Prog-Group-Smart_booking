using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace BookingSystem.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _port = 587;
        private readonly string _email = "midastouch2026g@gmail.com";
        private readonly string _password = "zmsm eurs btoz nake";

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Booking System", _email));
            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;
            email.Body = new TextPart("plain")
            {
                Text = message
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpServer, _port, false);
            await smtp.AuthenticateAsync(_email, _password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}