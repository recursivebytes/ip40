using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mvm = new MainViewModel();
            DataContext = mvm;
            
            mvm.FireAllPropertiesChanged();

            if (mvm.Config.AutoStart)
                if (mvm.StartStopCommand.CanExecute(null))
                    mvm.StartStopCommand.Execute(null);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainViewModel)DataContext).FillRange();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            About a = new About();
            a.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            a.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
    }
}
