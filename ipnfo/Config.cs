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
            set { Set("IPRangeStart", value); OnPropertyChanged("IPRangeStart");  }
        }

        public long IPRangeEnd
        {
            get { return Get<long>("IPRangeEnd"); }
            set { Set("IPRangeEnd", value); OnPropertyChanged("IPRangeEnd");  }
        }

        public bool UseICMP
        {
            get { return Get<bool>("UseICMP"); }
            set { Set("UseICMP", value); OnPropertyChanged("UseICMP"); }
        }

        public List<HostInformation> RecentHosts
        {
            get { return Get<List<HostInformation>>("RecentHosts"); }
            set { Set("RecentHosts", value); OnPropertyChanged("RecentHosts"); }
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
            UseICMP = false;
            IPRangeStart = new IPAddress(new byte[] { 192, 168, 1, 0 }).ToLong();
            IPRangeEnd = new IPAddress(new byte[] { 192, 168, 1, 255 }).ToLong();
            RecentHosts = new List<HostInformation>();
            PortInformation = new List<PortInformation>();

            PortInformation.Add(new PortInformation("HTTP", "Webserver", System.Net.Sockets.ProtocolType.Tcp, 80, 8080) { HasService = true });
            PortInformation.Add(new PortInformation("FTP", "File Transfer Protocol", System.Net.Sockets.ProtocolType.Tcp, 21, 20));
            PortInformation.Add(new PortInformation("SSH", "Secure Shell", System.Net.Sockets.ProtocolType.Tcp, 22));
            PortInformation.Add(new PortInformation("Telnet", "Telnet", System.Net.Sockets.ProtocolType.Tcp, 23));
            //PortInformation.Add(new PortInformation("SMTP", "SMTP", "Simple Mail Transfer Protocol", System.Net.Sockets.ProtocolType.Tcp, 25));
            //PortInformation.Add(new PortInformation("DNS", "DNS", "DNS-Dienste", System.Net.Sockets.ProtocolType.Tcp, 53));
            PortInformation.Add(new PortInformation("TFTP", "TFTP", System.Net.Sockets.ProtocolType.Udp, 69));
            //PortInformation.Add(new PortInformation("POP2", "POP2", "POP2", System.Net.Sockets.ProtocolType.Tcp, 109));
            //PortInformation.Add(new PortInformation("POP3", "POP3", "POP3", System.Net.Sockets.ProtocolType.Tcp, 110));
            //PortInformation.Add(new PortInformation("IMAP", "IMAP", "IMAP Protocol", System.Net.Sockets.ProtocolType.Tcp, 143));
            PortInformation.Add(new PortInformation("SMB", "SMB / Samba Dateifreigaben", System.Net.Sockets.ProtocolType.Tcp, 445) { HasService = true });


            PortInformation.Add(new PortInformation("LDAP", "LDAP-Verzeichnisdienst", System.Net.Sockets.ProtocolType.Tcp, 389));
            //PortInformation.Add(new PortInformation("RIP", "RIP", "Routing-Information Protocol", System.Net.Sockets.ProtocolType.Tcp, 520));
            PortInformation.Add(new PortInformation("MSSQL", "Microsoft SQL-Datenbankserver", System.Net.Sockets.ProtocolType.Tcp, 1433, 1434));
            PortInformation.Add(new PortInformation("MySQL", "MySQL-Datenbankserver", System.Net.Sockets.ProtocolType.Tcp, 3306));
            PortInformation.Add(new PortInformation("PostgreSQL", "Postgre-Datenbankserver", System.Net.Sockets.ProtocolType.Tcp, 5432));
            PortInformation.Add(new PortInformation("IRC", "Internet Relay Chat", System.Net.Sockets.ProtocolType.Tcp, 196));
            PortInformation.Add(new PortInformation("FTPS", "FTP über TLS", System.Net.Sockets.ProtocolType.Tcp, 989, 990));
            // PortInformation.Add(new PortInformation("IMAPS", "IMAPS", "IMAP über TLS", System.Net.Sockets.ProtocolType.Tcp, 993));
            PortInformation.Add(new PortInformation("RADIUS", "RADIUS Authentifizierung", System.Net.Sockets.ProtocolType.Tcp, 1812, 1813));
            PortInformation.Add(new PortInformation("SVN", "Subversion", System.Net.Sockets.ProtocolType.Tcp, 3690));


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

        [XmlIgnore]
        public List<PortInformation> PortInformation
        {
            get { return Get<List<PortInformation>>("PortInformation"); }
            set { Set("PortInformation", value); OnPropertyChanged("PortInformation"); }
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
