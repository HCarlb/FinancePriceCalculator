using Caliburn.Micro;
using FinancePriceToolProject.Events;
using FinancePriceToolProject.Forms;
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
        private int _priceFileRows;
        private int _bomFileRows;

        #region Properties
        public int BomFileRows
        {
            get
            {
                return _bomFileRows;
            }
            set
            {
                _bomFileRows = value;
                NotifyOfPropertyChange(() => BomFileRows);
            }
        }
        public int PriceFileRows
        {
            get
            {
                return _priceFileRows;
            }
            set
            {
                _priceFileRows = value;
                NotifyOfPropertyChange(() => PriceFileRows);
            }
        }
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
                _bomFileSelected = Path.GetFileName(value);
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
                _pricesFileSelected = Path.GetFileName(value);
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
        #endregion

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
            string newFile = Filebrowser.GetExcelFileName("Select Bill of Materials file");

            if (newFile.Length > 0)
            {
                try
                {
                    Globals.RawExcelBomFile = ExcelReader.ExcelToDataSet(newFile);
                }
                catch (Exception ex)
                {
                    ShowReadFileMessage(ex);
                    return;
                }
                
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

        public void ShowReadFileMessage(Exception ex)
        {
            // Give the user a notice about the error
            string mbText = $"Could not read file.\n\nError:\n{ex.Message}";

            MessageBox.Show(
                messageBoxText: mbText,
                caption: $"File read error",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error
            );

        }

        public void SelectPricesFile()
        {
            string newFile = newFile = Filebrowser.GetExcelFileName("Select Prices file");

            if (newFile.Length > 0)
            {
                try
                {
                    Globals.RawExcelPriceFile = ExcelReader.ExcelToDataSet(newFile);
                }
                catch (Exception ex)
                {
                    ShowReadFileMessage(ex);
                    return;
                }
                
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

            BomFileRows = Globals.BomData.Count;
        }
        public string GetText()
        {
            return $"asdad {BomFileRows} asdad";
        }
        private void ProcessPriceData()
        {
            // Read excelfile and extract the first sheet as IEnumerable to make it work with LINQ
            var prices = Globals.RawExcelPriceFile.Tables[0].AsEnumerable();

            // Convert the read data into a List of PriceItems
            Globals.PriceData = (from p in prices
                                  select new PriceItem
                                  {
                                      ProductID = Convert.ToString(p.Field<dynamic>("ProductID")),
                                      MaterialPrice = Convert.ToDecimal(p.Field<dynamic>("MaterialPrice")),
                                  }).ToList();

            PriceFileRows = Globals.PriceData.Count;
        }


        public void ShowAbout()
        {
            var x = new About();
            x.ShowDialog();
        }

        public void Handle(GotoPageOneEvent message)
        {
            //If needed, do something here when entering this page.
        }
    }
}
