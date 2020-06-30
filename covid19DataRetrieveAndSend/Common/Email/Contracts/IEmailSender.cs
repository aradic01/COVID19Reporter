using System.Collections.Generic;
using System.Threading.Tasks;

namespace covid19DataRetrieveAndSend.Common.Email.Contracts
{
    public interface IEmailSender
    {
        Task SendEmail(string to, string subject, string body, IDictionary<string, byte[]> attachments);
        Task SendEmails(IEnumerable<string> to, string subject, string body, IDictionary<string, byte[]> attachments);
    }
}
