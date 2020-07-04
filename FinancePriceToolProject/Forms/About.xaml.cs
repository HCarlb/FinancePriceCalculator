using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinancePriceToolProject.Forms
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void GotoWebPage_Click(object sender, RoutedEventArgs e)
        {
            // Get the link from the sender
            Hyperlink link = (Hyperlink)sender;
            string url = link.NavigateUri.ToString();

            // Open the link in default web browser.
            Process.Start(new ProcessStartInfo(url));
            e.Handled = true;
            this.Close();
        }
    }
}
