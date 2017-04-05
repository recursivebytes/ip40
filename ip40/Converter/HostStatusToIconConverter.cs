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
