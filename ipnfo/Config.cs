using csutils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ipnfo
{
    public class Config : Base
    {
        public static readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\ip46\\";
        public static readonly string FILE = "config.xml";

        public bool AutoFillRange
        {
            get { return Get<bool>("AutoFillRange"); }
            set { Set("AutoFillRange", value); OnPropertyChanged("AutoFillRange"); }
        }

        public bool ShowClassCView
        {
            get { return Get<bool>("ShowClassCView"); }
            set { Set("ShowClassCView", value); OnPropertyChanged("ShowClassCView"); }
        }

        public bool ShowSystemInformation
        {
            get { return Get<bool>("ShowSystemInformation"); }
            set { Set("ShowSystemInformation", value); OnPropertyChanged("ShowSystemInformation"); }
        }


        public int PingTimeout
        {
            get { return Get<int>("PingTimeout"); }
            set { Set("PingTimeout", value); OnPropertyChanged("PingTimeout"); }
        }


        public bool AutoScroll
        {
            get { return Get<bool>("AutoScroll"); }
            set { Set("AutoScroll", value); OnPropertyChanged("AutoScroll"); }
        }

        public bool CheckInternet
        {
            get { return Get<bool>("CheckInternet"); }
            set { Set("CheckInternet", value); OnPropertyChanged("CheckInternet"); }
        }

        public int MaxParallelConnections
        {
            get { return Get<int>("MaxParallelConnections"); }
            set { Set("MaxParallelConnections", value); OnPropertyChanged("MaxParallelConnections"); }
        }

        public bool AutoStart
        {
            get { return Get<bool>("AutoStart"); }
            set { Set("AutoStart", value); OnPropertyChanged("AutoStart"); }
        }

        public bool PortScan
        {
            get { return Get<bool>("PortScan"); }
            set { Set("PortScan", value); OnPropertyChanged("PortScan"); }
        }

        public long IPRangeStart
        {
            get { return Get<long>("IPRangeStart"); }
            set { Set("IPRangeStart", value); OnPropertyChanged("IPRangeStart"); OnPropertyChanged("IPRangeCount"); OnPropertyChanged("StartStopButtonText"); }
        }

        public long IPRangeEnd
        {
            get { return Get<long>("IPRangeEnd"); }
            set { Set("IPRangeEnd", value); OnPropertyChanged("IPRangeEnd"); OnPropertyChanged("IPRangeCount"); OnPropertyChanged("StartStopButtonText"); }
        }

        public Config()
        {
            AutoScroll = true;
            MaxParallelConnections = 64;
            CheckInternet = true;
            AutoFillRange = true;
            AutoStart = true;
            PortScan = false;
            ShowClassCView = false;
            ShowSystemInformation = true;
            PingTimeout = 500;
            IPRangeStart = new IPAddress(new byte[] { 192, 168, 1, 0 }).ToLong();
            IPRangeEnd = new IPAddress(new byte[] { 192, 168, 1, 255 }).ToLong();
        }

        public void Save()
        {
            if (!Directory.Exists(PATH))
                Directory.CreateDirectory(PATH);

            File.Delete(PATH + FILE);

            XmlSerializer s = new XmlSerializer(typeof(Config));
            using(StreamWriter sw = new StreamWriter(File.OpenWrite(PATH+FILE)))
            {
                s.Serialize(sw, this);
            }            
        }

        public static Config Load()
        {
            Config c  = new Config();

            if (File.Exists(PATH + FILE))
            {
                XmlSerializer s = new XmlSerializer(typeof(Config));
                using (StreamReader sr = new StreamReader(File.OpenRead(PATH + FILE)))
                {
                    c = (Config)s.Deserialize(sr);
                }
            }

            return c;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
