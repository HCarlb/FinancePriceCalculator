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

        /// <summary>
        /// Reads Excel files into a DataSet which is retured.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string fileName)
        {
            IExcelDataReader reader;
            DataSet dataSet;

            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                reader = ExcelReaderFactory.CreateReader(stream);

                ExcelDataSetConfiguration conf = new ExcelDataSetConfiguration
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
