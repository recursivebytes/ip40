using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ipnfo
{
    static class TextBlockHelper
    {
        public static bool GetCopyOnClick(TextBlock tb)
        {
            return (bool)tb.GetValue(CopyOnClickProperty);
        }

        public static void SetCopyOnClick(
          TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(CopyOnClickProperty, value);
        }

        public static readonly DependencyProperty CopyOnClickProperty =
            DependencyProperty.RegisterAttached(
            "CopyOnClick",
            typeof(bool),
            typeof(TextBlockHelper),
            new UIPropertyMetadata(false, OnCopyOnClickChanged));

        static void OnCopyOnClickChanged(
          DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            TextBlock item = depObj as TextBlock;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
            {
                item.MouseDown += OnCopyOnClick;
                item.ToolTip = "Klicken zum Kopieren";
                item.MouseEnter += item_MouseEnter;
                item.MouseLeave += item_MouseLeave;
            }
            else
            {
                item.MouseDown -= OnCopyOnClick;
            }
        }

        static void item_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock item = e.OriginalSource as TextBlock;
            if (item != null )
                item.Background = Brushes.Transparent;
        }

        private static readonly Brush b = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        static void item_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock item = e.OriginalSource as TextBlock;
            if (item != null && !string.IsNullOrEmpty(item.Text))
                item.Background = b;
        }

        static void OnCopyOnClick(object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            TextBlock item = e.OriginalSource as TextBlock;
            if(item!=null)
                Clipboard.SetText(item.Text);
        }
    }
}
