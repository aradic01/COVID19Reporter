using System.Collections.Generic;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Models;

namespace covid19DataRetrieveAndSend.Services.Contracts
{
    public interface ICovidDataProvider
    {
        Task<IEnumerable<Covid19Stats>> GetData();
    }
}
