using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FinancePriceToolProject.Converters
{
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Color color = Color.FromRgb(1, 123, 233);
            if(value.GetType() == typeof(decimal))
            {
                decimal val = (decimal)value;
                if(val > 0)
                {
                    return "#FF0017CB"; // Blue if possitive
                } 
                else if(val < 0) 
                {
                    return "#FFCB3200";  // Red if negative
                }
            }
            return "Transparent"; // Shows Zero values as transparent
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
