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

namespace ipnfo
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink h = (Hyperlink)sender;
            Process.Start(h.TargetName);
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

        private void version_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock b = sender as TextBlock;
            if(b!=null)
            {
                b.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString(2) + " Beta";
            }
        }

        private void date_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock b = sender as TextBlock;
            if (b != null)
            {
                b.Text = About.RetrieveLinkerTimestamp().ToString("dd.MM.yyyy");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
          
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
