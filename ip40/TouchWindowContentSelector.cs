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
