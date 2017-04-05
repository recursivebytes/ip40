using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ip40
{
    /// <summary>
    /// Converter that converts a HostStatus to a brush. Used to colorize the tiles of the Class C grid by the status
    /// </summary>
    public class StatusToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts to a Brush
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
