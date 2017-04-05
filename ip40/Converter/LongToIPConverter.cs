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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ip40
{
    /// <summary>
    /// Converter that converts the long-representation of an IP to a textual IP. 
    /// </summary>
    public class LongToIPConverter : IValueConverter
    {
        /// <summary>
        /// Converts to an IP.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">if "lan" the Output starts with an "\\", if "url" the output begins with "http://". </param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                long l = (long)value;
                string p = parameter == null ? "" : parameter.ToString();

                switch(p.ToLower())
                {
                    case "lan":
                        return "\\\\"+l.ToIP().ToString();
                    case "url":
                        return "http://" + l.ToIP().ToString();
                    default:
                        return l.ToIP().ToString();
                }              
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        /// <summary>
        /// Converts an IP to it's long representation if possible
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = (string)value;
            Match m = Regex.Match(str, @"(\d{1,3})(\.(\d{1,3}))?(\.(\d{1,3}))?(\.(\d{1,3}))?", RegexOptions.Compiled);
            if (m.Success)
            {
                try
                {
                    IPAddress ad = new IPAddress(new byte[] { (byte)int.Parse(m.Groups[1].Value), (byte)int.Parse(m.Groups[3].Value), (byte)int.Parse(m.Groups[5].Value), (byte)int.Parse(m.Groups[7].Value) });
                    return ad.ToLong();
                }
                catch
                {
                    return Binding.DoNothing;
                }
            }
            else
                return Binding.DoNothing;
        }
    }
}
