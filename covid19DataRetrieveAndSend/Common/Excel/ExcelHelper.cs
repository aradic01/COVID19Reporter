using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using ClosedXML.Excel;

namespace covid19DataRetrieveAndSend.Common.Excel
{
    public class ExcelHelper : IExcelHelper
    {
        public byte[] GenerateXlsFile<T>(IEnumerable<T> data) where T : class
        {
            using var dataTable = ConvertToDataTable(data);
            using var ds = new DataSet();
            ds.Tables.Add(dataTable);

            using var workBook = new XLWorkbook();
            workBook.Worksheets.Add(ds);

            using var stream = new MemoryStream();
            workBook.SaveAs(stream, false);

            return stream.GetBuffer();
        }

        private static DataTable ConvertToDataTable<T>(IEnumerable<T> data) where T : class
        {
            var dataTable = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            AddHeader(props, dataTable);

            foreach (var item in data)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static void AddHeader(IEnumerable<PropertyInfo> props, DataTable dataTable)
        {
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }
        }
    }
}
