using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;

namespace AutoRep.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "autorepxxz@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("autorepxxz@gmail.com", "123EWQasd");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
        public void SendEmailTest()
        {
            var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Администрация сайта", "autorepxxz@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", "boris37544@gmail.com"));
                emailMessage.Subject = "hello?";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "mama mia..."
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("autorepxxz@gmail.com", "123EWQasd");
                    client.Send(emailMessage);

                    client.Disconnect(true);
                }
        }
    }
}
