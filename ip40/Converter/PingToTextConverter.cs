//  IP40 - IP Network Scanner
//  Copyright (C) 2015-2017  Stefan T.
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ip40
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
