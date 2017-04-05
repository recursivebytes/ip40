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
using System.Windows.Media;

namespace ip40
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
