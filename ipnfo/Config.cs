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
    /// <summary>
    /// Configuration class for 
    /// </summary>
    public class Config : Base
    {
        /// <summary>
        /// Path to the Configfile (without filename)
        /// </summary>
        public static readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\ip46\\";
        /// <summary>
        /// Config file name only
        /// </summary>
        public static readonly string FILE = "config.xml";

        /// <summary>
        /// If true, the IP Address Range of a changed Ethernet Adapter will be automatically filled into the textboxes
        /// </summary>
        public bool AutoFillRange
        {
            get { return Get<bool>("AutoFillRange"); }
            set { Set("AutoFillRange", value); OnPropertyChanged("AutoFillRange"); }
        }

        /// <summary>
        /// if true, the Class C Grid will be shown. Classic View only, in TouchWindow always available.
        /// </summary>
        public bool ShowClassCView
        {
            get { return Get<bool>("ShowClassCView"); }
            set { Set("ShowClassCView", value); OnPropertyChanged("ShowClassCView"); }
        }

        /// <summary>
        /// If true, display Information about the Ethernetadapter and Internet. Classic View only.
        /// </summary>
        public bool ShowSystemInformation
        {
            get { return Get<bool>("ShowSystemInformation"); }
            set { Set("ShowSystemInformation", value); OnPropertyChanged("ShowSystemInformation"); }
        }

        /// <summary>
        /// Timeout for a Ping Request for scans
        /// </summary>
        public int PingTimeout
        {
            get { return Get<int>("PingTimeout"); }
            set { Set("PingTimeout", value); OnPropertyChanged("PingTimeout"); }
        }

        /// <summary>
        /// Auto-scrolls the Class C Grid to the next Page if needed while scanning
        /// </summary>
        public bool AutoScroll
        {
            get { return Get<bool>("AutoScroll"); }
            set { Set("AutoScroll", value); OnPropertyChanged("AutoScroll"); }
        }

        /// <summary>
        /// If true, Internet Connection will be checked as well
        /// </summary>
        public bool CheckInternet
        {
            get { return Get<bool>("CheckInternet"); }
            set { Set("CheckInternet", value); OnPropertyChanged("CheckInternet"); }
        }

        /// <summary>
        /// Maximum Number of parallel Pings
        /// </summary>
        public int MaxParallelConnections
        {
            get { return Get<int>("MaxParallelConnections"); }
            set { Set("MaxParallelConnections", value); OnPropertyChanged("MaxParallelConnections"); }
        }

        /// <summary>
        /// If true, the Default Ethernet Adapter will be scanned at startup
        /// </summary>
        public bool AutoStart
        {
            get { return Get<bool>("AutoStart"); }
            set { Set("AutoStart", value); OnPropertyChanged("AutoStart"); }
        }

        /// <summary>
        /// If true, Ports will be checked in the scanning process
        /// </summary>
        public bool PortScan
        {
            get { return Get<bool>("PortScan"); }
            set { Set("PortScan", value); OnPropertyChanged("PortScan"); }
        }

        /// <summary>
        /// Start IP of Scan Range 
        /// </summary>
        public long IPRangeStart
        {
            get { return Get<long>("IPRangeStart"); }
            set { Set("IPRangeStart", value); OnPropertyChanged("IPRangeStart");  }
        }

        /// <summary>
        /// End IP of Scan Range
        /// </summary>
        public long IPRangeEnd
        {
            get { return Get<long>("IPRangeEnd"); }
            set { Set("IPRangeEnd", value); OnPropertyChanged("IPRangeEnd");  }
        }

        /// <summary>
        /// If true ICMP will be used to check Host status
        /// </summary>
        public bool UseICMP
        {
            get { return Get<bool>("UseICMP"); }
            set { Set("UseICMP", value); OnPropertyChanged("UseICMP"); }
        }

        /// <summary>
        /// Known Hosts
        /// </summary>
        public List<HostInformation> RecentHosts
        {
            get { return Get<List<HostInformation>>("RecentHosts"); }
            set { Set("RecentHosts", value); OnPropertyChanged("RecentHosts"); }
        }

        /// <summary>
        /// Type of View. Changes need a restart to take effect
        /// </summary>
        public GUIType GUIType
        {
            get { return Get<GUIType>("GUIType"); }
            set { Set("GUIType", value); OnPropertyChanged("GUIType"); }
        }

        /// <summary>
        /// Constructor. Fills in default values
        /// </summary>
        public Config()
        {
            AutoScroll = true;
            MaxParallelConnections = 64;
            CheckInternet = true;
            AutoFillRange = true;
            AutoStart = true;
            PortScan = false;
            ShowClassCView = true;
            ShowSystemInformation = true;
            PingTimeout = 500;
            UseICMP = true;
            IPRangeStart = new IPAddress(new byte[] { 192, 168, 1, 1 }).ToLong();
            IPRangeEnd = new IPAddress(new byte[] { 192, 168, 1, 254 }).ToLong();
            RecentHosts = new List<HostInformation>();
            PortInformation = new List<PortInformation>();
            GUIType = ipnfo.GUIType.Auto;

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

        /// <summary>
        /// Saves the config file. Always deletes and overrides existing file
        /// </summary>
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

        /// <summary>
        /// List of Hosts in this Session. Will not be saved
        /// </summary>
        [XmlIgnore]
        public List<PortInformation> PortInformation
        {
            get { return Get<List<PortInformation>>("PortInformation"); }
            set { Set("PortInformation", value); OnPropertyChanged("PortInformation"); }
        }

        /// <summary>
        /// Loads the config out of it's file if available. If not or error new Instance with default values will be returned. Returns always a valid config
        /// </summary>
        /// <returns></returns>
        public static Config Load()
        {
            Config c  = new Config();

            try
            {
                if (File.Exists(PATH + FILE))
                {
                    XmlSerializer s = new XmlSerializer(typeof(Config));
                    using (StreamReader sr = new StreamReader(File.OpenRead(PATH + FILE)))
                    {
                        c = (Config)s.Deserialize(sr);
                    }
                }
            }
            catch
            {

            }

            return c;
        }

        /// <summary>
        /// not implemented
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
