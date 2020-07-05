using Caliburn.Micro;
using FinancePriceToolProject.Helpers;
using FinancePriceToolProject.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.ViewModels
{
    public class AboutViewModel : Screen
    {
        public string FormTitle => FormTitleHelper.FormTitleShort;
        public string GithubUrl => Settings.Default.AppGithubUrl;

        public AboutViewModel()
        {
           
        }


        public void CloseAboutForm()
        {
            this.TryClose();
        }


        public void FollowHyperlink()
        {
            // Open the link in default web browser.
            Process.Start(new ProcessStartInfo(GithubUrl));
            //CloseAboutForm();
        }

    }
}
