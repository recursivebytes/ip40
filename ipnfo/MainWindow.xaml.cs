using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ipnfo
{
    /// <summary>
    /// Classic View
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Creates the View
        /// </summary>
        public MainWindow()
        {

            
            InitializeComponent();
            DataContextChanged += MainWindow_DataContextChanged;
           
        }

        #region Sorting

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;                  
                    Sort(header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            try
            {
                ICollectionView dataView = CollectionViewSource.GetDefaultView(lv.ItemsSource);

                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();
            }
            catch { }
        }

#endregion


        void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
                ((MainViewModel)DataContext).PropertyChanged += mvm_PropertyChanged;

            MainViewModel m = DataContext as MainViewModel;

            if (m != null)
            {
                m.FireAllPropertiesChanged();
            }    
        }

        void mvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IPRangeStart" || e.PropertyName == "IPRangeEnd")
                mvm_PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("StartStopButtonText"));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainViewModel)DataContext).FillRange();
            ((MainViewModel)DataContext).ChangeClassCNetwork(((MainViewModel)DataContext).Config.IPRangeStart.ToIP());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            About a = new About();
            a.Owner = this;
            a.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            a.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(DataContext!=null)
            ((MainViewModel)DataContext).Config.Save();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border b = (Border)sender;
            ((MainViewModel)DataContext).CallService((HostInformation)b.Tag, (PortInformation)b.DataContext);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             Process.Start("explorer.exe", "::{7007ACC7-3202-11D1-AAD2-00805FC1270E}");            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string mmc = Environment.GetFolderPath(Environment.SpecialFolder.Windows)+"\\System32\\"+Thread.CurrentThread.CurrentCulture.ToString()+"\\WF.msc";
            Process.Start(mmc);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start("control.exe", "/name Microsoft.NetworkAndSharingCenter");            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                    
        }

        /// <summary>
        /// ClickHandler for the Copy-MenuItems. Copies Information to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void CopyText(object sender, RoutedEventArgs args)
        {
            try
            {
                MenuItem m = sender as MenuItem;
                if (m != null)
                {
                    HostInformation hi = ((HostDummy)m.DataContext).Host;
                    if (hi != null)
                    {
                        if (m.Name == "ip")
                            Clipboard.SetText(hi.ToString());
                        else if (m.Name == "mac")
                            Clipboard.SetText(hi.MAC.ToFormattedMAC());
                        else if (m.Name == "host")
                            Clipboard.SetText(hi.Hostname);
                    }
                }
            }
            catch { }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount > 1)
                {
                    HostDummy hd = ((FrameworkElement)sender).DataContext as HostDummy;
                    if (hd != null)
                    {
                        if (hd.Host == null)
                        {
                            HostInformation hi = new HostInformation(((MainViewModel)DataContext).CurrentClassCNetwork.ToLong() + (long)hd.LastOctettInt);
                            hd.Host = hi;
                        }

                        if (hd.Host != null)
                        {
                            if(hd.Host.WakeOnLanCommand.CanExecute(null))
                                hd.Host.WakeOnLanCommand.Execute(null);
                        }
                    }
                }
                else
                {
                    HostDummy hd = ((FrameworkElement)sender).DataContext as HostDummy;
                    if (hd != null)
                    {
                        if (hd.Host == null)
                        {
                            HostInformation hi = new HostInformation(((MainViewModel)DataContext).CurrentClassCNetwork.ToLong() + (long)hd.LastOctettInt);
                            hd.Host = hi;
                        }

                        if (hd.Host != null)
                        {
                            hd.Host.ScanIPCommand.Execute(null);
                        }
                    }
                }
            }
        }



    }
}
