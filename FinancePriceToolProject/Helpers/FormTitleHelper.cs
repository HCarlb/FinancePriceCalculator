using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinancePriceToolProject.Helpers
{
    public static class FormTitleHelper
    {

        public static string FormTitleWithCopyright
        {
            // Dynamic Form Title
            get
            {
                return $"{FormTitleShort} - {CopyrightText}";
            }
        }

        public static string FormTitleShort
        {
            // Dynamic Form Title
            get
            {
                var fi = GetFileVersionInfo();
                return $"{fi.ProductName} v{fi.FileVersion}";
            }
        }

        public static string CopyrightText
        {
            get
            {
                var fi = GetFileVersionInfo();
                return fi.LegalCopyright;
            }
        }

        public static FileVersionInfo GetFileVersionInfo()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
        }

    }
}
