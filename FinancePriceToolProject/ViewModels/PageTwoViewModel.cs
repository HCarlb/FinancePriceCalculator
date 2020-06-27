using Caliburn.Micro;
using FinancePriceToolProject.Events;
using FinancePriceToolProject.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.ViewModels
{
    public class PageTwoViewModel : Screen, IHandle<GotoPageTwoEvent>
    {
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _events;
        private object _dataGrid1ItemSource;
        private DateTime _targetDatePicker;

        private string _selectedDate;
        public string SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                NotifyOfPropertyChange(() => SelectedDate);
            }
        }

        public object DataGrid1ItemSource
        {
            get
            {
                return _dataGrid1ItemSource;
            }
            set
            {
                _dataGrid1ItemSource = value;
                NotifyOfPropertyChange(() => DataGrid1ItemSource);
            }
        }
        public DateTime TargetDatePicker
        {
            get
            {
                return _targetDatePicker;
            }
            set
            {
                _targetDatePicker = value;
                NotifyOfPropertyChange(() => TargetDatePicker);
            }
        }


        public PageTwoViewModel(SimpleContainer container, IEventAggregator events)
        {
            _container = container;
            _events = events;
            _events.Subscribe(this);
            TargetDatePicker = DateTime.Now;
            
        }
        public void GotoPageOne()
        {
            _events.PublishOnUIThread(new GotoPageOneEvent(this));
        }
        public void BuildProductsHierarchy()
        {
            Globals.Products = CreateProducts();
            LinkProductsByBom(Globals.Products);
        }
        private void LinkProductsByBom(List<ProductModel> products)
        {
            List<BomItem> productBomData;

            // Inject bomdata into products to fill it and make the links between all of them.
            foreach (ProductModel product in products)
            {
                productBomData = Globals.BomData
                    .Where(x => x.ProductID == product.Id)
                    .ToList();

                foreach (BomItem item in productBomData)
                {
                    ProductModel ch = GetProductFromrelation(products, item);
                    product.AddChild(
                        ch,
                        item.Quantity,
                        item.ValidFromDateTime,
                        item.ValidToDateTime);
                }
            }
        }
        private static ProductModel GetProductFromrelation(List<ProductModel> products, BomItem item)
        {
            return products
                .Where(x => x.Id == item.ComponentID)
                .FirstOrDefault();
        }
        public List<string> GetProducts()
        {
            // Get manufactured parts from Bill of Materials
            List<string> products = Globals.BomData
                .Select(x => x.ProductID)
                .GroupBy(x => x).Select(g => g.First()).ToList();


            // Get undelying component parts Bill of Materials
            List<string> components = Globals.BomData
                .Select(x => x.ComponentID)
                .GroupBy(x => x).Select(g => g.First()).ToList();


            //Merge all Products into a distinct list.
            var allProducts = products
                .Concat(components)
                .GroupBy(x => x)
                .Select(g => g.First())
                .ToList();

            // return a distinct list of products
            return allProducts;
        }
        public List<ProductModel> CreateProducts()
        {
            List<ProductModel> productList = new List<ProductModel>();

           var products = GetProducts();
            foreach (string product in products)
            {
                ProductModel item = new ProductModel()
                {
                    Id = product,
                    FixedPrice = GetMaterialPrice(product)
                };
                productList.Add(item);
            }

            return productList;
        }
        public decimal GetMaterialPrice(string product)
        {
            return Globals.PriceData
                .Where(x => x.ProductID == product)
                .Select(x => x.MaterialPrice)
                .SingleOrDefault();
        }
        public void Handle(GotoPageTwoEvent message)
        {
            BuildProductsHierarchy();
        }


        public void CreateDataGridContent()
        {
            var targetDate = TargetDatePicker.Date;
            SelectedDate = targetDate.ToLongDateString();
            var isource = (from p in Globals.Products
                           where p.HasComponents == true
                           select new
                           {
                               p.Id,
                               p.ComponentCount,
                               FixedPrice = p.FixedPrice.ToString("C2", CultureInfo.CreateSpecificCulture("sv-SE")),
                               CalculatedPrice = p.GetCalculatedPrice(targetDate).ToString("C2", CultureInfo.CreateSpecificCulture("sv-SE")),
                               CalculatedPriceActual = p.GetCalculatedPriceActual(targetDate).ToString("C2", CultureInfo.CreateSpecificCulture("sv-SE")),
                               DeltaFixedVsActualPrice = ToPercetile(p.FixedPrice, p.GetCalculatedPriceActual(targetDate)),
                               ContainsProductsLackingFixedPrice = String.Join("; ",p.GetSubProductsLackingPrice().ToArray()),
                               //p.HasComponents
                           }).ToList();

            DataGrid1ItemSource = isource;
        }

        public string ToPercetile(decimal value1, decimal value2)
        {
            if (value2 == 0)
            {
                return "#N/A";
            }
            else
            {
                return decimal.Round((((value1 - value2) / value2) * 100), 1).ToString() + " %";
            }
        }
    }
}
