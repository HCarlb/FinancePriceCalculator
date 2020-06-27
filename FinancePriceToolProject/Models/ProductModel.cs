using Caliburn.Micro;
using FinancePriceToolProject.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Models
{
    public class ProductModel 
    {
        //private readonly SimpleContainer _container;
        //private readonly IEventAggregator _events;

        public string Id { get; set; }
        public List<RelationModel> Relations { get; private set; } = new List<RelationModel>();
        public decimal FixedPrice { get; set; }
        public bool HasComponents => Relations.Count > 0;
        public int ComponentCount => Relations.Count;

        public void AddChild(ProductModel product, decimal quantity, DateTime validFrom, DateTime validTo)
        {
            RelationModel newRelation;
            RelationModel existingRelation = null;

            // Look for the component in existing relations. 
            if (Relations.Count > 0)
            {
                existingRelation = GetRelationByProductId(product.Id);
            }
            
            // Create quantity and valitidy item.
            ValidityQuantityModel vq = new ValidityQuantityModel()
            {
                Quantity = quantity,
                StartDate = validFrom,
                EndDate = validTo
            };

            // If an existing relatiuon exis append to it, else create a new.
            if (existingRelation == null)
            {
                //create new relation
                newRelation = new RelationModel();
                newRelation.Product = product;
                newRelation.ValidityQuantities = new List<ValidityQuantityModel>();
                newRelation.ValidityQuantities.Add(vq);
                Relations.Add(newRelation);
            }
            else
            {
                //append ValidityQuantityModel to existing relation
                existingRelation.ValidityQuantities.Add(vq);
            }
        }

        private RelationModel GetRelationByProductId(string product)
        {
            return Relations.Where(x => x.Product.Id == product).SingleOrDefault(); ;
        }

        public decimal GetCalculatedPrice(DateTime targetDate)
        {
            decimal sumPrice = 0;
            foreach (var relation in Relations)
            {
                var quantity = relation.GetQuantity(targetDate);
                if (relation.Product.FixedPrice > 0)
                {
                    sumPrice += relation.Product.FixedPrice * quantity;
                }
                else
                {
                    sumPrice += relation.Product.GetCalculatedPrice(targetDate) * quantity;
                }
            }
            return sumPrice;
        }

        public decimal GetCalculatedPriceActual(DateTime targetDate)
        {
            decimal sumPrice = 0;
            foreach (var relation in Relations)
            {
                var quantity = relation.GetQuantity(targetDate);

                // As purchased parts cant have any calculated value, make sure to pull the fixed price instead.
                if (relation.Product.HasComponents)
                {
                    sumPrice += relation.Product.GetCalculatedPriceActual(targetDate) * quantity;
                }
                else
                {
                    // If value is zero give the app th information if it wants it.
                    if (relation.Product.FixedPrice <= 0)
                    {
                       // _events.PublishOnUIThread(new ZeroValuePurchasedPartFoundEvent { ProductId = relation.Product.Id });
                    } 
                    else
                    {
                        sumPrice += relation.Product.FixedPrice;
                    }
                }
            }
            return sumPrice;
        }

        //public int GetComponentsAsParentsCount(DateTime targetDate)
        //{
        //    int count = 0;
        //    foreach (var relation in Relations)
        //    {
        //        // As purchased parts cant have any calculated value, make sure to pull the fixed price instead.
        //        if (relation.Product.HasComponents)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}
    }
}
