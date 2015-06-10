using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
