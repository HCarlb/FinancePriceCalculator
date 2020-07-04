using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Models
{
    public class RelationModel
    {
        #region Fields
        public ProductModel Product { get; set; }
        public List<ValidityQuantityModel> ValidityQuantities { get; set; }
        #endregion

        #region Methods
        public decimal GetQuantity(DateTime targetDate) => GetValidityQuantitiesByDate(targetDate).Sum(x => x.Quantity);
        public List<ValidityQuantityModel> GetValidityQuantitiesByDate(DateTime targetDate) => 
            ValidityQuantities.Where(x => x.StartDate <= targetDate && targetDate < x.EndDate).ToList();
        #endregion

    }
}
