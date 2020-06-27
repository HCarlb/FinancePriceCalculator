using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FinancePriceToolProject.Helpers
{
    public static class Filebrowser
    {
        public static string GetExcelFileName(string title)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog.Title = "Select Prices file";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
    }
}
