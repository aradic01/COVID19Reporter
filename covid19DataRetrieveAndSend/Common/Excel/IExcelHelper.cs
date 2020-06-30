using System.Collections.Generic;

namespace covid19DataRetrieveAndSend.Common.Excel
{
    public interface IExcelHelper
    {
        byte[] GenerateXlsFile<T>(IEnumerable<T> data) where T : class;
    }
}
