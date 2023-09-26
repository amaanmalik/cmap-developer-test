using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Globalization;
using System.Text;
using Timesheets.Models;

namespace Timesheets.Services
{
    public interface ICsvExportService<T>
    {
        byte[] ExportToCsv(List<T> data);
    }

    public class CsvExportService<T> : ICsvExportService<T>
    {
        public byte[] ExportToCsv(List<T> data)
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, new UTF8Encoding(true)))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(data);
                writer.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}
