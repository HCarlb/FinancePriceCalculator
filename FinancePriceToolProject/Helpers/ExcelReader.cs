using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Helpers
{
    public static class ExcelReader
    {

        public static DataSet ExcelToDataSet(string fileName)
        {
            IExcelDataReader reader;
            DataSet dataSet;

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                reader = ExcelReaderFactory.CreateReader(stream);

                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    },
                    UseColumnDataType = false
                };

                dataSet = reader.AsDataSet(conf);
            }

            return dataSet;
        }
    }
}
