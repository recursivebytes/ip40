using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ipnfo
{
    public class HostStatusToIconConverter : IValueConverter
    {
        static ImageSource unknown = new BitmapImage(new Uri("pack://application:,,,/Icons/status_unknown.png"));
        static ImageSource online = new BitmapImage(new Uri("pack://application:,,,/Icons/status.png"));
        static ImageSource checking = new BitmapImage(new Uri("pack://application:,,,/Icons/status-away.png"));
        static ImageSource offline = new BitmapImage(new Uri("pack://application:,,,/Icons/status-busy.png"));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            HostStatus hs = (HostStatus)value;
            switch(hs)
            {
                case HostStatus.Offline:
                    return offline;
                case HostStatus.Online:
                    return online;
                case HostStatus.Checking:
                    return checking;
                default:
                    return unknown;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
