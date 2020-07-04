using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Models
{
    public class BomItem
    {
        #region Fields
        public string ProductID { get; set; }
        public string ComponentID { get; set; }
        public string ValidFromText { get; set; }
        public string ValidToText { get; set; }
        public decimal Quantity { get; set; }
        #endregion

        #region Properties
        public DateTime ValidFromDateTime => GetDateFromText(ValidFromText, DateTime.Parse("2000-01-01"));
        public DateTime ValidToDateTime => GetDateFromText(ValidToText, DateTime.Parse("2999-12-31"));
        #endregion

        #region Methods
        private DateTime GetDateFromText(string text, DateTime defaultIfNull)
        {
            if (text?.Length >= 10)
            {
                // parse an ISO date string ex. 2020-10-29 (YYYY-MM-DD) into a DateTime object.
                return DateTime.Parse(text.Substring(0, 10)).Date;   
            }
            return defaultIfNull;
        }
        #endregion
    }
}
