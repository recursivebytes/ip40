using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ip40
{
    /// <summary>
    /// Converts a Status to a ellipse-style.
    /// </summary>
    public class HostStatusToIconConverter : IValueConverter
    {
        /// <summary>
        /// Converts forward. Uses the offline, online, busy and unknown styles in IconResources.xaml to choose from
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

        /// <summary>
        /// not implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
