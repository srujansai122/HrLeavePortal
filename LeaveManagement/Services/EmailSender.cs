using System.Net.Mail;

namespace LeaveManagement.Services
{
    public class EmailSender(IConfiguration configuration) : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress(configuration["EmailSettings:DefaultEmailAddress"], configuration["EmailSettings:SenderName"]),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            using var client = new SmtpClient(configuration["EmailSettings:Server"], Convert.ToInt32(configuration["EmailSettings:Port"]));

            await client.SendMailAsync(message);

        }

    }
}