using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ipnfo
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
