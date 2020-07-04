using Caliburn.Micro;
using FinancePriceToolProject.Events;
using FinancePriceToolProject.Models;
using FinancePriceToolProject.Properties;
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
        #region Fields
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _events;
        private object _dataGrid1ItemSource;
        private DateTime _targetDatePicker;
        private string _selectedDate;
        #endregion

        #region Properties
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
        #endregion

        #region Constructor
        public PageTwoViewModel(SimpleContainer container, IEventAggregator events)
        {
            _container = container;
            _events = events;
            _events.Subscribe(this);
            TargetDatePicker = DateTime.Now;

        }
        #endregion

        #region Methods
        #region Navigation
        public void GotoPageOne()
        {
            _events.PublishOnUIThread(new GotoPageOneEvent(this));
        }
        public void GotoPageThree()
        {
            _events.PublishOnUIThread(new GotoPageThreeEvent(this));
        }
        public void Handle(GotoPageTwoEvent message)
        {
            BuildProductsHierarchy();
        }
        #endregion

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
                .Select(x => x.UnitPrice)
                .SingleOrDefault();
        }
        public void CreateDataGridContent()
        {
            DateTime targetDate = TargetDatePicker.Date;
            CultureInfo culture = CultureInfo.CreateSpecificCulture(Settings.Default.ApplicationSpecificCulture);
            string displayFormat = Settings.Default.CurrencyDisplayFormat;
            string separator = Settings.Default.CsvSeparator + " ";


            SelectedDate = targetDate.ToLongDateString();
            var isource = (from p in Globals.Products
                           where p.HasComponents == true
                           select new
                           {
                               p.Id,
                               p.ComponentCount,
                               FixedPrice = p.FixedPrice,
                               CalculatedPrice = decimal.Round(p.GetCalculatedPrice(targetDate), Settings.Default.DefaultRoundingDecimals),
                               CalculatedPriceActual = decimal.Round(p.GetCalculatedPriceActual(targetDate),Settings.Default.DefaultRoundingDecimals),
                               DeltaFixedVsActualPrice = decimal.Round(ToPercetile(p.FixedPrice, p.GetCalculatedPriceActual(targetDate)), Settings.Default.DefaultRoundingDecimals),
                               ContainsProductsLackingFixedPrice = String.Join(separator, p.GetSubProductsLackingPrice().ToArray()),
                               //FixedPrice = p.FixedPrice.ToString(displayFormat, culture),
                               //CalculatedPrice = p.GetCalculatedPrice(targetDate).ToString(displayFormat, culture),
                               //CalculatedPriceActual = p.GetCalculatedPriceActual(targetDate).ToString(displayFormat, culture),
                               //DeltaFixedVsActualPrice = ToPercetile(p.FixedPrice, p.GetCalculatedPriceActual(targetDate)),
                               //p.HasComponents
                           }).ToList();

            DataGrid1ItemSource = isource;
        }
        public decimal ToPercetile(decimal value1, decimal value2)
        {
            if (value2 == 0)
            {
                return 0;
            }
            else
            {
                return decimal.Round(((value1 - value2) / value2), Settings.Default.DefaultRoundingDecimals);
            }
        }
        #endregion

    }
}
