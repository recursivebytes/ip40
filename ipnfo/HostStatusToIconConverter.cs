using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ipnfo
{
    public class HostStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri resourceLocater = new Uri("/ip40;component/IconResources.xaml", System.UriKind.Relative);
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            HostStatus hs = (HostStatus)value;
            switch (hs)
            {
                case HostStatus.Offline:
                    return resourceDictionary["offline"] as Style;
                case HostStatus.Online:
                    return resourceDictionary["online"] as Style;
                case HostStatus.Checking:
                    return resourceDictionary["busy"] as Style;
                default:
                    return resourceDictionary["unknown"] as Style;
            }   
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
