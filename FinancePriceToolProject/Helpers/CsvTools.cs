using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Helpers
{
    public static class CsvTools
    {

        public static void ExportListToFile(IEnumerable records, string fileName, string delimiter)
        {
            using (var writer = new StreamWriter(fileName))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    csv.Configuration.Delimiter = delimiter;
                    csv.WriteRecords(records);
                }
            }
        }
    }
}
