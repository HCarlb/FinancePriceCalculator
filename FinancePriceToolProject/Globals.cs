using FinancePriceToolProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject
{
    public static class Globals
    {
        public static DataSet RawExcelBomFile { get; set; }
        public static DataSet RawExcelPriceFile { get; set; }

        public static List<BomItem> BomData { get; set; }
        public static List<PriceItem> PriceData { get; set; }


        public static List<ProductModel> Products { get; set; }
    }
}
