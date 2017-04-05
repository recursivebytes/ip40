//  IP40 - IP Network Scanner
//  Copyright (C) 2015-2017  Stefan T.
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;


using csutils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ip40
{
    /// <summary>
    /// Configuration class for 
    /// </summary>
    public class Config : Base
    {
        /// <summary>
        /// Path to the Configfile (without filename)
        /// </summary>
        public static readonly string PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\ip40\\";
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
            AutoStart = false;
            ShowClassCView = true;
            ShowSystemInformation = true;
            PingTimeout = 500;
            UseICMP = true;
            IPRangeStart = new IPAddress(new byte[] { 192, 168, 1, 1 }).ToLong();
            IPRangeEnd = new IPAddress(new byte[] { 192, 168, 1, 254 }).ToLong();
            RecentHosts = new List<HostInformation>();
            GUIType = ip40.GUIType.Auto;
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
