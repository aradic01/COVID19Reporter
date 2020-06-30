using System.Net;
using System.Net.Mail;

namespace covid19DataRetrieveAndSend.Common.Email
{
    public class SmtpClientConfiguration
    {
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public NetworkCredential NetworkCredential { get; set; }
    }
}
