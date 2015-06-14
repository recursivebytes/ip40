using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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


            MainWindow mainView = new MainWindow();
            mainView.Show();
            mainView.DataContext = ViewModel;
            
        }
    }
}
