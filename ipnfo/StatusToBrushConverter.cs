using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ipnfo
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            HostStatus h = (HostStatus)value;
            switch(h)
            {
                case HostStatus.Online: 
                    return Brushes.LightGreen;
                case HostStatus.Offline:
                    return Brushes.LightSalmon;
                case HostStatus.Disabled:
                    return Brushes.Transparent;
                case HostStatus.Pending:
                    return Brushes.LightBlue;
                case HostStatus.Checking:
                    return Brushes.LightYellow;
                default: 
                    return Brushes.LightGray;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
