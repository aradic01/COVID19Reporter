using System.Collections.Generic;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Models;

namespace covid19DataRetrieveAndSend.Services.Contracts
{
    public interface ICovidStatisticsNotifier
    {
        Task NotifySubscribers(IEnumerable<Covid19Stats> data);
    }
}
