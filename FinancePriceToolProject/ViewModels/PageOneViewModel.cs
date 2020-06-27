using Caliburn.Micro;
using FinancePriceToolProject.Events;
using FinancePriceToolProject.Helpers;
using FinancePriceToolProject.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancePriceToolProject.ViewModels
{
    public class PageOneViewModel : Screen, IHandle<GotoPageOneEvent>
    {

        private readonly SimpleContainer _container;
        private readonly IEventAggregator _events;
        private string _bomFileSelected;
        private DateTime _targetDate;
        private string _pricesFileSelected;

        public DateTime TargetDate
        {
            get
            {
                return _targetDate;
            }
            set
            {
                _targetDate = value;
                NotifyOfPropertyChange(() => TargetDate);
            }
        }
        public string BomFileSelected
        {
            get
            {
                return _bomFileSelected;
            }
            set
            {
                _bomFileSelected = value;
                NotifyOfPropertyChange(() => BomFileSelected);
                NotifyOfPropertyChange(() => CanGotoPageTwo);
            }
        }

        public string PricesFileSelected
        {
            get
            {
                return _pricesFileSelected;
            }
            set
            {
                _pricesFileSelected = value;
                NotifyOfPropertyChange(() => PricesFileSelected);
                NotifyOfPropertyChange(() => CanGotoPageTwo);
            }
        }

        public bool CanGotoPageTwo
        {
            get
            {
                if (BomFileSelected?.Length > 0  && PricesFileSelected?.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }

        // Methods
        public PageOneViewModel(SimpleContainer container, IEventAggregator events)
        {
            _container = container;
            _events = events;
            _events.Subscribe(this);

            TargetDate = DateTime.Now;
        }

        public void SelectBomFile()
        {
            var newFile = Filebrowser.GetExcelFileName("Select Bill of Materials file");
            if (newFile.Length > 0)
            {
                Globals.RawExcelBomFile = ExcelReader.ExcelToDataSet(newFile);
                try
                {
                    ProcessBomData();
                    BomFileSelected = newFile;
                }
                catch (Exception ex)
                {
                    // Give the user a notice about the error
                    string mbText = $"Could not process data\n\nError:\n{ex.Message}";

                    MessageBox.Show(
                        messageBoxText: mbText,
                        caption: $"An unexpected error has occurred",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error
                    );
                }
            }
        }

        public void SelectPricesFile()
        {
            var newFile = Filebrowser.GetExcelFileName("Select Prices file");
            if (newFile.Length > 0)
            {
                Globals.RawExcelPriceFile = ExcelReader.ExcelToDataSet(newFile);
                try
                {
                    ProcessPriceData();
                    PricesFileSelected = newFile;
                }
                catch (Exception ex)
                {
                    // Give the user a notice about the error
                    string mbText = $"Could not process data\n\nError:\n{ex.Message}";

                    MessageBox.Show(
                        messageBoxText: mbText,
                        caption: $"An unexpected error has occurred",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error
                    );
                }
            }
        }

        public void GotoPageTwo()
        {
            _events.PublishOnUIThread(new GotoPageTwoEvent(this));
        }


        private void ProcessBomData()
        {
            string ConvertLocationToLocationID(string location)
            {
                // Hardcoded fix
                if (location.Trim().ToUpper() == "SKÖVDE") return "SE09";
                return location;
            }

            var bom = Globals.RawExcelBomFile.Tables[0].AsEnumerable();

            // Convert the read data into a List of BomItems
            Globals.BomData = (from b in bom
                               select new BomItem
                               {
                                   LocationID = ConvertLocationToLocationID(Convert.ToString(b.Field<dynamic>("ns2:Sender"))),
                                   ProductID = Convert.ToString(b.Field<dynamic>("ns4:Base")),
                                   ComponentID = Convert.ToString(b.Field<dynamic>("ns4:Base3")),
                                   ValidFromText = Convert.ToString(b.Field<dynamic>("ns3:ValidFrom")),
                                   ValidToText = Convert.ToString(b.Field<dynamic>("ns3:ValidTo")),
                                   Quantity = Convert.ToDecimal(b.Field<dynamic>("ns3:Quantity")),
                               }).ToList();

        }

        private void ProcessPriceData()
        {
            // Read excelfile and extract the first sheet as IEnumerable to make it work with LINQ
            var prices = Globals.RawExcelPriceFile.Tables[0].AsEnumerable();

            // Convert the read data into a List of PriceItems
            Globals.PriceData = (from p in prices
                                  select new PriceItem
                                  {
                                      LocationID = Convert.ToString(p.Field<dynamic>("LocationID")),
                                      ProductID = Convert.ToString(p.Field<dynamic>("ProductID")),
                                      //StandardPrice = Convert.ToDecimal(p.Field<dynamic>("StandardPrice")),
                                      MaterialPrice = Convert.ToDecimal(p.Field<dynamic>("MaterialPrice")),
                                  }).ToList();
        }

        public void Handle(GotoPageOneEvent message)
        {
            //If needed, do something here when entering this page.
        }
    }
}
