using System.Net.Mail;
using System.Threading.Tasks;

namespace covid19DataRetrieveAndSend.Common.Email.Contracts
{
    public interface ISmtpClient
    {
        Task SendMailAsync(MailMessage message);
    }
}
