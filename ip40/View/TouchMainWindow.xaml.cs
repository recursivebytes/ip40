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


using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ip40
{
    /// <summary>
    /// Modern View
    /// </summary>
    public partial class TouchMainWindow : MetroWindow
    {

        
        /// <summary>
        /// creates the view
        /// </summary>
        public TouchMainWindow()
        {
            InitializeComponent();
            DataContextChanged += TouchMainWindow_DataContextChanged;
        }

        void TouchMainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel m = DataContext as MainViewModel;

            if (m != null)
            {
                ((MainViewModel)DataContext).ChangeClassCNetwork(((MainViewModel)DataContext).Config.IPRangeStart.ToIP());
                m.FireAllPropertiesChanged();
            }    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((MainViewModel)DataContext).View = TouchView.List;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((MainViewModel)DataContext).View = TouchView.Grid;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((MainViewModel)DataContext).View = TouchView.Options;
        }


        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext != null)
                ((MainViewModel)DataContext).Config.Save();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).CurrentSelected = null;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HostDummy hd = ((FrameworkElement)sender).DataContext as HostDummy;
            if(hd!=null)
            {
                
                if(hd.Host==null)
                {

                    ((MainViewModel)DataContext).CurrentSelected = new HostInformation(((MainViewModel)DataContext).CurrentClassCNetwork.ToLong() + hd.LastOctettInt);
                }
                else
                    ((MainViewModel)DataContext).CurrentSelected = hd.Host;

                ((MainViewModel)DataContext).CurrentSelected.ScanIPCommand.Execute(null);
            }
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainViewModel)DataContext).FillRange();
            ((MainViewModel)DataContext).ChangeClassCNetwork(((MainViewModel)DataContext).Config.IPRangeStart.ToIP());
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://gnu.org/licenses");
        }

    }

    /// <summary>
    /// Tabs in the Modern View
    /// </summary>
    public enum TouchView
    {
        /// <summary>
        /// Class C grid
        /// </summary>
        Grid,
        /// <summary>
        /// List view
        /// </summary>
        List,
        /// <summary>
        /// settings and about
        /// </summary>
        Options
    }
}
