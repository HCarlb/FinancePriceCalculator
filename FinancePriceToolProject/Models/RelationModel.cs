using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Models
{
    public class RelationModel
    {
        public ProductModel Product { get; set; }
        public List<ValidityQuantityModel> ValidityQuantities { get; set; }


        public decimal GetQuantity(DateTime targetDate)
        {
            return GetValidityQuantitiesByDate(targetDate).Sum(x => x.Quantity);
        }

        public List<ValidityQuantityModel> GetValidityQuantitiesByDate(DateTime targetDate)
        {
            return ValidityQuantities
                    .Where(x => x.StartDate <= targetDate && targetDate < x.EndDate)
                    .ToList();
        }
    }
}
