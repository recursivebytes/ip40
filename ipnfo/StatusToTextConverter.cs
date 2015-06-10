using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ipnfo
{
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            HostStatus h = (HostStatus)value;
            switch (h)
            {
                case HostStatus.Online:
                    return "erreichbar";
                case HostStatus.Offline:
                    return "nicht erreichbar";                
                default:
                    return "unbekannt";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
