using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ipnfo
{
    /// <summary>
    /// Converts a Ping to Text. Used to be able to display "&lt;1 ms" 
    /// </summary>
    public class PingToTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts a Ping to Text. Used to be able to display "&lt;1 ms" 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">if "unit" the Unis (ms) is added at the end</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue || ((HostStatus)values[1]) != HostStatus.Online)
                return "";

            parameter = parameter == null ? "" : parameter;


            if (values[0].ToString() == "0" || values[0].ToString() == "")
                return "<1" + (parameter.ToString().ToLower()=="unit" ? "ms" :"");



            return values[0].ToString() + (parameter.ToString().ToLower() == "unit" ? "ms" : "");
        }

        /// <summary>
        /// not implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
