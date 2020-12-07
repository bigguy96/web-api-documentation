using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApiDocumentationWebApplication.Utilities
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string email, string name, string subject, string body)
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using var smtp = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = myDocuments
            };
            var message = new MailMessage
            {
                Body = body,
                Subject = subject,
                From = new MailAddress(email, name),
                IsBodyHtml = true
            };
            message.To.Add("noreply@tc.gc.ca");
            
            await smtp.SendMailAsync(message);
        }
    }
}