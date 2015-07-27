using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ipnfo
{
    public class TouchWindowContentSelector : DataTemplateSelector
    {
        public DataTemplate GridTemplate { get; set; }
        public DataTemplate ListTemplate { get; set; }
        public DataTemplate OptionsTemplate { get; set; }

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
