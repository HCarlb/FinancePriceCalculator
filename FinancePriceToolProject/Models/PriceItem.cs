using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Models
{
    public class PriceItem
    {
        public string LocationID { get; set; }
        public string ProductID { get; set; }
        //public decimal StandardPrice { get; set; }
        public decimal MaterialPrice { get; set; }

    }
}
