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
    /// Selector to switch the Tabs in the modern view
    /// </summary>
    public class TouchWindowContentSelector : DataTemplateSelector
    {
        /// <summary>
        /// Class C Grid Template
        /// </summary>
        public DataTemplate GridTemplate { get; set; }
        /// <summary>
        /// List Tab Template
        /// </summary>
        public DataTemplate ListTemplate { get; set; }
        /// <summary>
        /// Options Tab Template
        /// </summary>
        public DataTemplate OptionsTemplate { get; set; }

        /// <summary>
        /// Selects the template
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if(item == null)
                return base.SelectTemplate(item, container);

            switch(((MainViewModel)item).View)
            {
                case TouchView.Grid:
                    return GridTemplate;
                case TouchView.List:
                    return ListTemplate;
                case TouchView.Options:
                    return OptionsTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
