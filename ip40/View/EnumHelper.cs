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
using System.Windows.Controls;

namespace ip40
{
    /// <summary>
    /// Contains some nice helpers for enums to bind it to a combobox
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type GetEnum(DependencyObject obj)
        {
            return (Type)obj.GetValue(EnumProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetEnum(DependencyObject obj, string value)
        {
            obj.SetValue(EnumProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EnumProperty = DependencyProperty.RegisterAttached("Enum", typeof(Type), typeof(EnumHelper), new PropertyMetadata(null, OnEnumChanged));


        private static void OnEnumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as ItemsControl;
            if (control != null)
            {
                if (e.NewValue != null)
                {
                    var _enum = Enum.GetValues(e.NewValue as Type);
                    control.ItemsSource = _enum;
                }
            }
        }
    }
}
