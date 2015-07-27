using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace ipnfo
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public MainViewModel ViewModel { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModel = new MainViewModel();

           
            GUIType gt;
            if (ViewModel.Config.GUIType == GUIType.Auto)
            {
                try
                {
                    gt = GetSystemMetrics(95) > 0 ? GUIType.Modern : GUIType.Classic;
                }
                catch
                {
                    gt = GUIType.Classic;
                }
            }
            else
                gt = ViewModel.Config.GUIType;

            if (gt == GUIType.Modern)
            {
                TouchMainWindow mainView = new TouchMainWindow();
                mainView.Show();
                mainView.DataContext = ViewModel;
            }
            else
            {
                MainWindow mainView = new MainWindow();
                mainView.Show();
                mainView.DataContext = ViewModel;
            }

            
        }

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int smIndex);
    }
}
