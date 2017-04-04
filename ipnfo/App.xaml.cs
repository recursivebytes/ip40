using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ipnfo
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The main view model of the instance
        /// </summary>
        public MainViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets the human readable Version of the Version
        /// </summary>
        public static string Version
        {
            get
            {
               return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion; 
            }
        }

        /// <summary>
        /// Gets the formatted Compile Date
        /// </summary>
        public static string CompileDate
        {
            get
            {
                return RetrieveLinkerTimestamp().ToString("dd.MM.yyyy");
            }
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


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModel = new MainViewModel();

           //determine GUI Type if set to automatic
            GUIType gt;
            if (ViewModel.Config.GUIType == GUIType.Auto)
            {
                try
                {
                    //use Modern design if touchscreen found
                    gt = IsTouchEnabled() ? GUIType.Modern : GUIType.Classic;
                }
                catch
                {
                    //fallback just in case IsTouchEnabled() horribly fails
                    gt = GUIType.Classic;
                }
            }
            else
                gt = ViewModel.Config.GUIType;

            //open the appropriate window and attach the ViewModel
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

        const int SM_MAXIMUMTOUCHES = 95;

        /// <summary>
        /// checks if the Device has a Touchscreen
        /// </summary>
        /// <returns></returns>
        public static bool IsTouchEnabled()
        {
            Version v = Environment.OSVersion.Version;

            //first try, obtain information via P/Invoke from the System. Only supported for Win7 and newer
            //checking for anything above of OSVersion 6.0
            if( (v.Major >= 6 && v.Minor >= 1) || v.Major >= 7)
                return GetSystemMetrics(SM_MAXIMUMTOUCHES) > 0;

            //second try, use the TabletDevice-class
            foreach (TabletDevice tabletDevice in Tablet.TabletDevices)
            {
                if (tabletDevice.Type == TabletDeviceType.Touch)
                    return true;
            }

            //touchscreen not found, assume there is none
            return false;
        }
    }
}
