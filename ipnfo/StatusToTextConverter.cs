using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ipnfo
{
    /// <summary>
    /// Converter that converts a HostStatus to a string. Used to display the status in words
    /// </summary>
    public class StatusToTextConverter : IValueConverter
    {
        /// <summary>
        /// Convert to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
