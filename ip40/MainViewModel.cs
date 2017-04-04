using csutils.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ip40
{
    /// <summary>
    /// The main view model for the application
    /// </summary>
    public class MainViewModel : Base
    {
        /// <summary>
        /// Address of the Current Class C Network that is shown in the View
        /// TODO: move into view
        /// </summary>
        public IPAddress CurrentClassCNetwork
        {
            get { return Get<IPAddress>("CurrentClassCNetwork"); }
            set { Set("CurrentClassCNetwork", value); OnPropertyChanged("CurrentClassCNetwork"); }
        }

        /// <summary>
        /// Configuration object
        /// </summary>
        public Config Config
        {
            get { return Get<Config>("Config"); }
            set { Set("Config", value); OnPropertyChanged("Config"); }
        }

        /// <summary>
        /// Current Tab shown in the View. Modern view only.
        /// TODO: move into view
        /// </summary>
        public TouchView View
        {
            get { return Get<TouchView>("View"); }
            set { Set("View", value); OnPropertyChanged("View"); }
        }

        /// <summary>
        /// Currently selected Host. Used to display the GridPopup in modern view.
        /// </summary>
        public HostInformation CurrentSelected
        {
            get { return Get<HostInformation>("CurrentSelected"); }
            set { Set("CurrentSelected", value); 
                OnPropertyChanged("CurrentSelected");
            }
        }

        /// <summary>
        /// Invokes an update on the view for all Porperties
        /// </summary>
        public new void FireAllPropertiesChanged()
        {
            base.FireAllPropertiesChanged();
            OnPropertyChanged("HostsOnline");
            OnPropertyChanged("StartStopButtonText");
        }


        private static HostDummy[] dummies;
        /// <summary>
        /// Dummies that represent each tile of the Class C Grid
        /// TODO: move into view
        /// </summary>
        public static HostDummy[] ClassCDummies
        {
            get
            {
                if (dummies == null)
                {
                    dummies = new HostDummy[256];
                    for (int i = 0; i < 256; i++)
                    {
                        dummies[i] = new HostDummy(i);
                    }
                }
                return dummies;
            }
        }

        /// <summary>
        /// Changes the displayed Class C network
        /// TODO: move into view
        /// </summary>
        /// <param name="addr"></param>
        public void ChangeClassCNetwork(IPAddress addr)
        {
            //get the class c address of the given IP addr
           
            byte[] b = addr.GetAddressBytes();

            long start = addr.ToLong() - b[3]; //start at x.y.z.0
            long end = new IPAddress(new byte[] { b[0], b[1], b[2], 255 }).ToLong(); //end at x.y.z.255
            CurrentClassCNetwork = start.ToIP();

            //grab all known Hosts of this range
            HostInformation[] his = Hosts.Where(w => w.IP >= start && w.IP <= end).ToArray();

            //for all IPs of this network...
            for (int i = 0; i < 256; i++)
            {                
                //try to find it in the list of the known Hosts and assign it to the dummy
                HostInformation hi = his.FirstOrDefault(f => f.IP == start + ClassCDummies[i].LastOctettInt);
                if (hi != null)
                {
                    ClassCDummies[i].Host = hi; 
                }
                else
                    ClassCDummies[i].Host = null;

            }
                       
            OnPropertyChanged("ClassCDummies");
        }

        /// <summary>
        /// Updates the Class C grid Hosts
        /// TODO: move into view
        /// </summary>
        public void UpdateClassCDummies()
        {
            //get all known Hosts of the current range
            var start = CurrentClassCNetwork.ToLong();
            var hosts = Hosts.Where(w => w.IP >= start && w.IP <= start + 255);

            //assign them to the grid
            foreach (var h in hosts)
            {
                ClassCDummies[h.IP - start].Host = h;
            }
            OnPropertyChanged("HostsOnline");
        }

        private ICommand cmdNextClassCNetwork;
        /// <summary>
        /// Command that scrolls the Class C grid to the next page
        /// </summary>
        public ICommand NextClassCNetworkCommand
        {
            get
            {
                if (cmdNextClassCNetwork == null)
                    cmdNextClassCNetwork = new RelayCommand(p => OnNextClassCNetwork(p), p => CanNextClassCNetwork());
                return cmdNextClassCNetwork;
            }
        }

        private bool CanNextClassCNetwork()
        {
            return CurrentClassCNetwork.ToLong() >1;
        }

        private void OnNextClassCNetwork(object parameter)
        {
            byte[] b = CurrentClassCNetwork.GetAddressBytes();

            if (b[2] == 254)
            {
                if (b[1] == 254)
                {
                    ChangeClassCNetwork(new IPAddress(new byte[] { (byte)(b[0] + 1), 0, 0, 0 }));
                }
                else
                {
                    ChangeClassCNetwork(new IPAddress(new byte[] { b[0], (byte)(b[1] + 1), 0, 0 }));
                }
            }
            else
            {
                ChangeClassCNetwork(new IPAddress(new byte[] { b[0], b[1], (byte)(b[2] + 1), 0 }));
            }
        }


        private ICommand cmdPreviousClassCNetwork;
        /// <summary>
        /// Command that scrolls the Class C grid to the previous page
        /// </summary>
        public ICommand PreviousClassCNetworkCommand
        {
            get
            {
                if (cmdPreviousClassCNetwork == null)
                    cmdPreviousClassCNetwork = new RelayCommand(p => OnPreviousClassCNetwork(p), p => CanPreviousClassCNetwork());
                return cmdPreviousClassCNetwork;
            }
        }

        private bool CanPreviousClassCNetwork()
        {
            return CurrentClassCNetwork.ToLong() > 1;
        }

        private void OnPreviousClassCNetwork(object parameter)
        {
            byte[] b = CurrentClassCNetwork.GetAddressBytes();

            if (b[2] == 0)
            {
                if (b[1] == 0)
                {
                    ChangeClassCNetwork(new IPAddress(new byte[] { (byte)(b[0] - 1), 254, 0, 0 }));
                }
                else
                {
                    ChangeClassCNetwork(new IPAddress(new byte[] { b[0], (byte)(b[1] - 1), 254, 0 }));
                }
            }
            else
            {
                ChangeClassCNetwork(new IPAddress(new byte[] { b[0], b[1], (byte)(b[2] - 1), 0 }));
            }
        }


        /// <summary>
        /// List of all known Hosts
        /// </summary>
        public ObservableCollection<HostInformation> Hosts
        {
            get { return Get<ObservableCollection<HostInformation>>("Hosts"); }
            set { Set("Hosts", value); OnPropertyChanged("Hosts"); }
        }

        /// <summary>
        /// List of all known Hosts that are online. Subset of Hosts.
        /// </summary>
        public IEnumerable<HostInformation> HostsOnline
        {
            get
            {
                return Hosts.Where(w => w.Status == HostStatus.Online || (w.Status == HostStatus.Offline && w.MAC != null) || (w.Status == HostStatus.Unknown && w.MAC != null)).OrderBy(o => o.Status);
            }
        }

        /// <summary>
        /// Creates the Model
        /// </summary>
        public MainViewModel()
        {
            //init config, NICs and Hosts
            Config = Config.Load();
            Hosts = new ObservableCollection<HostInformation>();
            foreach (var hi in Config.RecentHosts)
            {
                hi.Status = HostStatus.Unknown;
                Hosts.Add(hi);
            }
            
            NICs = new ObservableCollection<NIC>();
            ScanProgress = 0;            
            
            Task.Run(() => { DiscoverNICs(); });
           
            
        }

        /// <summary>
        /// not implemented
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Fills the IP Range Textboxes with the selected NIC IP Range. Does currently work only for Class C Networks
        /// </summary>
        public void FillRange()
        {
            if (!Config.AutoFillRange)
                return;

            try
            {
                string ip = CurrentNIC.IP;
                if (!string.IsNullOrEmpty(ip))
                {
                    long start = IPAddress.Parse(ip).ToLongNetwork()+1;
                    long end = start + 253;
                    Config.IPRangeStart = start;
                    Config.IPRangeEnd = end;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// This computer name
        /// </summary>
        public string Computername
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        /// Current selected Network Interface
        /// </summary>
        public NIC CurrentNIC
        {
            get { return Get<NIC>("CurrentNIC"); }
            set { Set("CurrentNIC", value); OnPropertyChanged("CurrentNIC"); FillRange(); }
        }

        /// <summary>
        /// Value that determines if a scan is currently running
        /// </summary>
        public bool IsAnalyzing
        {
            get { return Get<bool>("IsAnalyzing"); }
            set { Set("IsAnalyzing", value); OnPropertyChanged("IsAnalyzing"); OnPropertyChanged("ScanIPCommand"); OnPropertyChanged("StartStopButtonText"); }
        }

        /// <summary>
        /// value that indicates the scan progress. Range [0,100]
        /// </summary>
        public double ScanProgress
        {
            get { return Get<double>("ScanProgress"); }
            set { Set("ScanProgress", value); OnPropertyChanged("ScanProgress"); }
        }

        /// <summary>
        /// Status of Googles DNS 
        /// </summary>
        public HostStatus GoogleDNSStatus
        {
            get { return Get<HostStatus>("GoogleDNSStatus"); }
            set { Set("GoogleDNSStatus", value); OnPropertyChanged("GoogleDNSStatus"); }
        }

        /// <summary>
        /// Status of the default DNS
        /// </summary>
        public HostStatus DNSStatus
        {
            get { return Get<HostStatus>("DNSStatus"); }
            set { Set("DNSStatus", value); OnPropertyChanged("DNSStatus"); }
        }

        /// <summary>
        /// Status of the Gateway
        /// </summary>
        public HostStatus GatewayStatus
        {
            get { return Get<HostStatus>("GatewayStatus"); }
            set { Set("GatewayStatus", value); OnPropertyChanged("GatewayStatus"); }
        }

        /// <summary>
        /// Status of the Internet Connection
        /// </summary>
        public HostStatus InternetStatus
        {
            get { return Get<HostStatus>("InternetStatus"); }
            set { Set("InternetStatus", value); OnPropertyChanged("InternetStatus"); }
        }

        private ICommand cmdClearRecentHosts;
        /// <summary>
        /// Command that clears the list of known hosts
        /// </summary>
        public ICommand ClearRecentHostsCommand
        {
            get
            {
                if (cmdClearRecentHosts == null)
                    cmdClearRecentHosts = new RelayCommand(p => OnClearRecentHosts(p), p => CanClearRecentHosts());
                return cmdClearRecentHosts;
            }
        }

        private bool CanClearRecentHosts()
        {
            return Config.RecentHosts.Count>0;
        }

        private void OnClearRecentHosts(object parameter)
        {
            Config.RecentHosts.Clear();
        }
	

        /// <summary>
        /// Current Text of the Start Stop Button. Depends on IsAnalyzing
        /// </summary>
        public string StartStopButtonText
        {
            get
            {
                if (IsAnalyzing)
                {
                    return "Abbrechen";
                }
                else
                {
                    return string.Format("Scannen", IPRangeCount);
                }
            }
        }

        /// <summary>
        /// Number of Hosts in the current IP range
        /// </summary>
        public int IPRangeCount
        {
            get
            {
                return Math.Max(0, (int)(Config.IPRangeEnd - Config.IPRangeStart));
            }
        }

        /// <summary>
        /// List of available Network Interfaces
        /// </summary>
        public ObservableCollection<NIC> NICs
        {
            get { return Get<ObservableCollection<NIC>>("NICs"); }
            set { Set("NICs", value); OnPropertyChanged("NICs"); }
        }


        private ICommand cmdStartStop;
        /// <summary>
        /// Command that starts or stops the scan
        /// </summary>
        public ICommand StartStopCommand
        {
            get
            {
                if (cmdStartStop == null)
                    cmdStartStop = new RelayCommand(p => OnStartStop(p), p => CanStartStop());
                return cmdStartStop;
            }
        }

        private bool CanStartStop()
        {
            return CurrentNIC!=null;
        }

        private void OnStartStop(object parameter)
        {
            if (IsAnalyzing)
            {
                AbortScan();
            }
            else
            {
                if (Config.UseICMP)
                    Task.Run(() => { ScanICMP(); }); //Ping every Host in range
                else
                    Task.Run(() => { ScanARP(); }); //Pings only Hosts that are known by ARP cache
            }
        }


        

        //regex that identifies uninteresting NICs
        Regex nicnameRegex = new Regex(@"(Pseudo|Tunneling)", RegexOptions.Compiled);

        /// <summary>
        /// Discover all available Network Interfaces
        /// </summary>
        public void DiscoverNICs()
        {
            NICs.Clear();
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    if (nicnameRegex.IsMatch(nic.Name))
                        continue;

                    NIC n = new NIC(nic);
                    NICs.Add(n);
                }
            }
            CurrentNIC = NICs.FirstOrDefault();
        }

        /// <summary>
        /// Value that indicates if a running scan should be aborted at the next suitable moment
        /// </summary>
        public bool Stop
        {
            get { return Get<bool>("Stop"); }
            set { Set("Stop", value); OnPropertyChanged("Stop"); }
        }

        /// <summary>
        /// Scan only known Hosts.
        /// TODO: does currently not check the status
        /// </summary>
        public async void ScanARP()
        {
            IsAnalyzing = true;

            var l = GetAllDevicesOnLAN();
            ScanProgress = 0;

            var his = l.Select(s => new HostInformation(s.Key.ToString()) { MAC = s.Value });
            Hosts.Clear();
            foreach (var h in his)
                Hosts.Add(h);

            UpdateClassCDummies();

            if (Config.CheckInternet && CurrentNIC != null)
            {
                await CheckConnectivity();
            }
            ScanProgress = 100;

            
            IsAnalyzing = false;
            OnPropertyChanged("HostsOnline");
            OnPropertyChanged("ClassCDummies");
            FireAllPropertiesChanged();
        }

        /// <summary>
        /// Performs the IP scan
        /// </summary>
        public async void ScanICMP()
        {
            IsAnalyzing = true;

            //set Class C grid to the right page
            ChangeClassCNetwork(Config.IPRangeStart.ToIP().ToLongNetwork().ToIP());

            //fills the list of the Hosts with actual HostInformations for all IPs that are scanned
            for (long i = Config.IPRangeStart; i <= Config.IPRangeEnd; i++)
            {
                var hi = Hosts.FirstOrDefault(f => f.IP == i);

                if (hi == null)
                {
                    hi = new HostInformation(i);
                    hi.Pending = true;
                    Hosts.Add(hi);
                }
                else
                {
                    hi.Pending = true;
                }
            }

            //update the underlaying Hosts of the dummies
            UpdateClassCDummies();

            //check internet if needed
            if (Config.CheckInternet && CurrentNIC != null)
            {
                await CheckConnectivity();
            }

            int hostcount = Hosts.Where(w => w.Pending).Count();
            int current = 0;
            int check = 1;
            while (check != 0 && !Stop) //as long there are unchecked IPs...
            {
                //grab a bunch of pending IPs
                List<HostInformation> z = Hosts.Where(w => w.Pending).Take(Config.MaxParallelConnections).ToList();
                z.ForEach(e => e.Status = HostStatus.Checking);
                List<Task<HostInformation>> tasks = new List<Task<HostInformation>>();
                foreach (var t in z)
                    tasks.Add(CheckHost(t,Config.PingTimeout));

                //and execute the scan for them
                Task.WaitAll(tasks.ToArray());

                //invoke view update
                OnPropertyChanged("HostsOnline");
                check = z.Count();
                current += check;
                ScanProgress = ((double)current / (double)hostcount) * 100;

                //scroll the the next page on the Class C grid if needed
                if (Config.AutoScroll && check > 0 && z.FirstOrDefault() != null)
                    ChangeClassCNetwork(z.First().IP.ToIP().ToLongNetwork().ToIP());
            }

            //now get the MACs of all newly found Hosts and store them. Update RecentHosts as well.
            var l = GetAllDevicesOnLAN();
            foreach(var d in l)
            {
                var f = Hosts.FirstOrDefault(a => a.IP == d.Key.ToLong());
                if (f != null)
                    f.MAC = d.Value;
                else
                {
                    f = new HostInformation(d.Key.ToLong()) { MAC = d.Value };
                    Hosts.Add(f);
                }
                if (!Config.RecentHosts.Any(a => a.MAC.ToString() == f.MAC.ToString()))
                    Config.RecentHosts.Add(f);
            }

            
            //reset Pending status and do cleanup
            foreach (var h in Hosts)
                h.Pending = false;
            ScanProgress = Stop ? 0 : 100;
            Stop = false;
            IsAnalyzing = false;
            FireAllPropertiesChanged();
        }

        /// <summary>
        /// Checks if the Internet is available
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckConnectivity()
        {            
            try
            {
                Ping p = new Ping();
                PingReply pr = p.Send(new IPAddress(new byte[] { 8, 8, 8, 8 }), 2000);
                GoogleDNSStatus = pr.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                GoogleDNSStatus = HostStatus.Offline;
            }

            try
            {
                Ping p2 = new Ping();
                PingReply pr2 = await p2.SendPingAsync(CurrentNIC.Gateway,2000);
                var entry = Dns.GetHostEntry("example.com");
                GatewayStatus = entry.AddressList.Length>0 && pr2.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                GatewayStatus = HostStatus.Offline;
            }

            try
            {
                Ping p3 = new Ping();
                PingReply pr3 = await p3.SendPingAsync(CurrentNIC.DNS, 2000);
                DNSStatus = pr3.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                DNSStatus = HostStatus.Offline;
            }
            try
            {
                Ping p4 = new Ping();
                PingReply pr4 = await p4.SendPingAsync("google.de",2000);
                InternetStatus = pr4.Status == IPStatus.Success && DNSStatus == HostStatus.Online ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                InternetStatus = HostStatus.Offline;
            }
            return true;
        }

        /// <summary>
        /// Performs the individual scan an a Host
        /// </summary>
        /// <param name="hi">The Host to be scanned</param>
        /// <param name="PingTimeout">Timeout for Ping</param>
        /// <param name="elseStatus">Default-Status if not successful (no answer)</param>
        /// <returns></returns>
        public static async Task<HostInformation> CheckHost(HostInformation hi,  int PingTimeout, HostStatus elseStatus = HostStatus.Offline)
        {
            try
            {
                Ping p = new Ping();
                IPAddress target = hi.IP.ToIP();
                PingReply pr = await p.SendPingAsync(target, PingTimeout); //the actual ping
                hi.Status = pr.Status == IPStatus.Success ? HostStatus.Online : elseStatus; //set the status
                hi.Ping = (int)pr.RoundtripTime; //and the ping

                //retrieve some more information if Host is online
                if (hi.Status == HostStatus.Online)
                {
                    try
                    {
                        //get the Hostname
                        IPHostEntry hostEntry = Dns.GetHostEntry(target);
                        hi.Hostname = hostEntry.HostName;
                    }
                    catch
                    {

                    }                   
                }

                hi.Pending = false;
                return hi;
            }
            catch
            {
                return hi;
            }
        }
        
        /// <summary>
        /// Sets the Flag to abort the scan
        /// </summary>
        public void AbortScan()
        {
            Stop = true;
        }

        
        #region ARP Table

        /// <summary>
        /// MIB_IPNETROW structure returned by GetIpNetTable
        /// DO NOT MODIFY THIS STRUCTURE.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct MIB_IPNETROW
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwIndex;
            [MarshalAs(UnmanagedType.U4)]
            public int dwPhysAddrLen;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac0;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac1;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac2;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac3;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac4;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac5;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac6;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac7;
            [MarshalAs(UnmanagedType.U4)]
            public int dwAddr;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }

        /// <summary>
        /// GetIpNetTable external method
        /// </summary>
        /// <param name="pIpNetTable"></param>
        /// <param name="pdwSize"></param>
        /// <param name="bOrder"></param>
        /// <returns></returns>
        [DllImport("IpHlpApi.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern int GetIpNetTable(IntPtr pIpNetTable,
              [MarshalAs(UnmanagedType.U4)] ref int pdwSize, bool bOrder);

        /// <summary>
        /// Error codes GetIpNetTable returns that we recognise
        /// </summary>
        const int ERROR_INSUFFICIENT_BUFFER = 122;
        /// <summary>
        /// Get the IP and MAC addresses of all known devices on the LAN
        /// </summary>
        /// <remarks>
        /// 1) This table is not updated often - it can take some human-scale time 
        ///    to notice that a device has dropped off the network, or a new device
        ///    has connected.
        /// 2) This discards non-local devices if they are found - these are multicast
        ///    and can be discarded by IP address range.
        /// </remarks>
        /// <returns></returns>
        private static Dictionary<IPAddress, PhysicalAddress> GetAllDevicesOnLAN()
        {
            Dictionary<IPAddress, PhysicalAddress> all = new Dictionary<IPAddress, PhysicalAddress>();
            // Add this PC to the list...
            all.Add(GetIPAddress(), GetMacAddress());
            int spaceForNetTable = 0;
            // Get the space needed
            // We do that by requesting the table, but not giving any space at all.
            // The return value will tell us how much we actually need.
            GetIpNetTable(IntPtr.Zero, ref spaceForNetTable, false);
            // Allocate the space
            // We use a try-finally block to ensure release.
            IntPtr rawTable = IntPtr.Zero;
            try
            {
                rawTable = Marshal.AllocCoTaskMem(spaceForNetTable);
                // Get the actual data
                int errorCode = GetIpNetTable(rawTable, ref spaceForNetTable, false);
                if (errorCode != 0)
                {
                    // Failed for some reason - can do no more here.
                    throw new Exception(string.Format(
                      "Unable to retrieve network table. Error code {0}", errorCode));
                }
                // Get the rows count
                int rowsCount = Marshal.ReadInt32(rawTable);
                IntPtr currentBuffer = new IntPtr(rawTable.ToInt64() + Marshal.SizeOf(typeof(Int32)));
                // Convert the raw table to individual entries
                MIB_IPNETROW[] rows = new MIB_IPNETROW[rowsCount];
                for (int index = 0; index < rowsCount; index++)
                {
                    rows[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() +
                                                (index * Marshal.SizeOf(typeof(MIB_IPNETROW)))
                                               ),
                                                typeof(MIB_IPNETROW));
                }
                // Define the dummy entries list (we can discard these)
                PhysicalAddress virtualMAC = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
                PhysicalAddress broadcastMAC = new PhysicalAddress(new byte[] { 255, 255, 255, 255, 255, 255 });
                foreach (MIB_IPNETROW row in rows)
                {
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    byte[] rawMAC = new byte[] { row.mac0, row.mac1, row.mac2, row.mac3, row.mac4, row.mac5 };
                    PhysicalAddress pa = new PhysicalAddress(rawMAC);
                    if (!pa.Equals(virtualMAC) && !pa.Equals(broadcastMAC) && !IsMulticast(ip))
                    {
                        //Console.WriteLine("IP: {0}\t\tMAC: {1}", ip.ToString(), pa.ToString());
                        if (!all.ContainsKey(ip))
                        {
                            all.Add(ip, pa);
                        }
                    }
                }
            }
            finally
            {
                // Release the memory.
                Marshal.FreeCoTaskMem(rawTable);
            }
            return all;
        }

        /// <summary>
        /// Gets the IP address of the current PC
        /// </summary>
        /// <returns></returns>
        private static IPAddress GetIPAddress()
        {
            String strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress ip in addr)
            {
                if (!ip.IsIPv6LinkLocal)
                {
                    return (ip);
                }
            }
            return addr.Length > 0 ? addr[0] : null;
        }

        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        private static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if the specified IP address is a multicast address
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsMulticast(IPAddress ip)
        {
            bool result = true;
            if (!ip.IsIPv6Multicast)
            {
                byte highIP = ip.GetAddressBytes()[0];
                if (highIP < 224 || highIP > 239)
                {
                    result = false;
                }
            }
            return result;
        }

#endregion
    }
}
