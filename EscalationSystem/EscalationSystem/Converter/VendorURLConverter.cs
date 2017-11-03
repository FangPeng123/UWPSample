using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace EscalationSystem.Converter
{
    public class VendorURLConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Uri Myuri = new Uri(value.ToString());
            return Myuri;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}
