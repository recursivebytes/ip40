using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace ipnfo
{
    /// <summary>
    /// Interaktionslogik für TouchMainWindow.xaml
    /// </summary>
    public partial class TouchMainWindow : MetroWindow
    {

        
        
        public TouchMainWindow()
        {
            InitializeComponent();
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


    }

    public enum TouchView
    {
        Grid,
        List,
        Options,
        About
    }
}
