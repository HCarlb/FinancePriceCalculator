using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Models
{
    public class BomItem
    {
        public string ProductID { get; set; }
        public string ComponentID { get; set; }
        public string ValidFromText { get; set; }
        public string ValidToText { get; set; }
        public decimal Quantity { get; set; }

        public DateTime ValidFromDateTime => GetDateFromText(ValidFromText, DateTime.Parse("2000-01-01"));
        public DateTime ValidToDateTime => GetDateFromText(ValidToText, DateTime.Parse("2999-12-31"));


        private DateTime GetDateFromText(string text, DateTime defaultIfNull)
        {
            if (text?.Length >= 10)
            {
                return DateTime.Parse(text.Substring(0, 10));
            }
            return defaultIfNull;
        }
    }
}
