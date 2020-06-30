using System.Net.Mail;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Common.Email.Contracts;

namespace covid19DataRetrieveAndSend.Common.Email
{
    public class SmtpClient : ISmtpClient
    {
        private readonly System.Net.Mail.SmtpClient _client;

        public SmtpClient(SmtpClientConfiguration configuration)
        {
            _client = new System.Net.Mail.SmtpClient
            {
                EnableSsl = configuration.EnableSsl,
                Host = configuration.Host,
                Port = configuration.Port,
                DeliveryMethod = configuration.DeliveryMethod,
                Credentials = configuration.NetworkCredential
            };
        }

        public Task SendMailAsync(MailMessage message)
        {
            return _client.SendMailAsync(message);
        }
    }
}
