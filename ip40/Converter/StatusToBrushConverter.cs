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
