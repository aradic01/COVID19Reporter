using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Common.Email.Contracts;
using Microsoft.Extensions.Configuration;

namespace covid19DataRetrieveAndSend.Common.Email
{
    public class EMailSender : IEmailSender
    {
        private readonly string _emailSourceAddress;
        private readonly ISmtpClient _client;

        public EMailSender(IConfiguration configuration, ISmtpClient client)
        {
            _emailSourceAddress = configuration.GetSection("emailConfig")["sourceAddress"];
            _client = client;
        }

        public async Task SendEmails(IEnumerable<string> to, string subject, string body, IDictionary<string, byte[]> attachments)
        {
            using var message = BuildMessage(to, subject, body, attachments);
            await _client.SendMailAsync(message);
        }


        public Task SendEmail(string to, string subject, string body, IDictionary<string, byte[]> attachments)
        {
            return SendEmails(new[] { to }, subject, body, attachments);
        }

        private MailMessage BuildMessage(IEnumerable<string> tos, string subject, string body, IDictionary<string, byte[]> attachments)
        {
            var message = new MailMessage { From = new MailAddress(_emailSourceAddress), Subject = subject, Body = body };

            foreach (var to in tos)
            {
                message.To.Add(to);
            }

            foreach (var (key, value) in attachments)
            {
                var stream = new MemoryStream(value);

                message.Attachments.Add(new Attachment(stream, key));
            }

            return message;
        }
    }
}
