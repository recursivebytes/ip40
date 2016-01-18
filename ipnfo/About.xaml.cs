using System;
using System.Collections.Generic;
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
    /// AboutBox of the classic view
    /// </summary>
    public partial class About : Window
    {
        /// <summary>
        /// Creates new Box
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink h = (Hyperlink)sender;
            Process.Start(h.TargetName);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            date.Text = RetrieveLinkerTimestamp().ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Get compile date of assembly
        /// </summary>
        /// <remarks>
        ///  Source: http://stackoverflow.com/questions/1600962/displaying-the-build-date
        /// </remarks>
        /// <returns></returns>
        internal static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }
    }


}
