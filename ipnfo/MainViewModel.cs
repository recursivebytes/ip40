using csutils.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ipnfo
{
    public class MainViewModel : Base
    {
        public IPAddress CurrentClassCNetwork
        {
            get { return Get<IPAddress>("CurrentClassCNetwork"); }
            set { Set("CurrentClassCNetwork", value); OnPropertyChanged("CurrentClassCNetwork"); }
        }

        public Config Config
        {
            get { return Get<Config>("Config"); }
            set { Set("Config", value); OnPropertyChanged("Config"); }
        }


        private static HostDummy[] dummies;
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

        public void ChangeClassCNetwork(IPAddress addr)
        {
            CurrentClassCNetwork = addr;
            byte[] b = addr.GetAddressBytes();

            long start = addr.ToLong();
            long end = new IPAddress(new byte[] { b[0], b[1], b[2], 255 }).ToLong();

            HostInformation[] his = Hosts.Where(w => w.IP >= start && w.IP <= end).ToArray();

            for (int i = 0; i < 256; i++)
            {
                //ClassCDummies[i].Text = string.Format("{0}.{1}.{2}.{3}", (int)b[0], (int)b[1], (int)b[2], i);
                //ClassCDummies[i].MVM = this;

                HostInformation hi = his.FirstOrDefault(f => f.IP == start + ClassCDummies[i].LastOctettInt);
                if (hi != null)
                {
                    ClassCDummies[i].Host = hi;
                }
                else
                    ClassCDummies[i].Host = null;
                //ClassCDummies[i].FireVisibleStatusUpdate();

            }

            OnPropertyChanged("ClassCDummies");
        }


        public void UpdateClassCDummies()
        {
            var start = CurrentClassCNetwork.ToLong();
            var hosts = Hosts.Where(w => w.IP >= start && w.IP <= start + 255);

            foreach (var h in hosts)
            {
                ClassCDummies[h.IP - start].Host = h;
            }
            OnPropertyChanged("HostsOnline");
        }

        private ICommand cmdNextClassCNetwork;
        /// <summary>
        /// NextClassCNetwork Command
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
            return true;
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
        /// PreviousClassCNetwork Command
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
            return true;
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



        public ObservableCollection<HostInformation> Hosts
        {
            get { return Get<ObservableCollection<HostInformation>>("Hosts"); }
            set { Set("Hosts", value); OnPropertyChanged("Hosts"); }
        }

        public IEnumerable<HostInformation> HostsOnline
        {
            get
            {
                return Hosts.Where(w => w.Status == HostStatus.Online);
            }
        }

        public MainViewModel()
        {
            Config = Config.Load();
            Hosts = new ObservableCollection<HostInformation>();
            NICs = new ObservableCollection<NIC>();
            

            ChangeClassCNetwork(new IPAddress(new byte[] { 192, 168, 178, 0 }));
            Task.Run(() => { DiscoverNICs(); });
        }

        public override object Clone()
        {
            throw new NotImplementedException();

        }


        public void FillRange()
        {
            if (!Config.AutoFillRange)
                return;

            try
            {
                string ip = CurrentNIC.IP;
                if (!string.IsNullOrEmpty(ip))
                {
                    long start = IPAddress.Parse(ip).ToLongNetwork();
                    long end = start + 255;
                    Config.IPRangeStart = start;
                    Config.IPRangeEnd = end;
                }
            }
            catch
            {

            }
        }


        public string Computername
        {
            get { return Environment.MachineName; }
        }

        public NIC CurrentNIC
        {
            get { return Get<NIC>("CurrentNIC"); }
            set { Set("CurrentNIC", value); OnPropertyChanged("CurrentNIC"); FillRange(); }
        }


        public bool IsAnalyzing
        {
            get { return Get<bool>("IsAnalyzing"); }
            set { Set("IsAnalyzing", value); OnPropertyChanged("IsAnalyzing"); OnPropertyChanged("StartStopButtonText"); }
        }


        public double ScanProgress
        {
            get { return Get<double>("ScanProgress"); }
            set { Set("ScanProgress", value); OnPropertyChanged("ScanProgress"); }
        }

        public HostStatus GoogleDNSStatus
        {
            get { return Get<HostStatus>("GoogleDNSStatus"); }
            set { Set("GoogleDNSStatus", value); OnPropertyChanged("GoogleDNSStatus"); }
        }

        public HostStatus DNSStatus
        {
            get { return Get<HostStatus>("DNSStatus"); }
            set { Set("DNSStatus", value); OnPropertyChanged("DNSStatus"); }
        }

        public HostStatus GatewayStatus
        {
            get { return Get<HostStatus>("GatewayStatus"); }
            set { Set("GatewayStatus", value); OnPropertyChanged("GatewayStatus"); }
        }

        public HostStatus InternetStatus
        {
            get { return Get<HostStatus>("InternetStatus"); }
            set { Set("InternetStatus", value); OnPropertyChanged("InternetStatus"); }
        }


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
                    return string.Format("Scannen ({0})", IPRangeCount);
                }
            }
        }

        public int IPRangeCount
        {
            get
            {
                return Math.Max(0, (int)(Config.IPRangeEnd - Config.IPRangeStart));
            }
        }


        public ObservableCollection<NIC> NICs
        {
            get { return Get<ObservableCollection<NIC>>("NICs"); }
            set { Set("NICs", value); OnPropertyChanged("NICs"); }
        }


        private ICommand cmdStartStop;
        /// <summary>
        /// StartStop Command
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
            return true;
        }

        private void OnStartStop(object parameter)
        {
            if (IsAnalyzing)
            {
                AbortScan();
            }
            else
            {
                Task.Run(() => { Scan(); });
            }
        }


        Regex nicnameRegex = new Regex(@"(Pseudo|Tunneling)", RegexOptions.Compiled);

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
        }

        public bool Stop
        {
            get { return Get<bool>("Stop"); }
            set { Set("Stop", value); OnPropertyChanged("Stop"); }
        }


        public async void Scan()
        {
            IsAnalyzing = true;

            ChangeClassCNetwork(Config.IPRangeStart.ToIP().ToLongNetwork().ToIP());

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

            UpdateClassCDummies();

            int hostcount = Hosts.Where(w => w.Pending).Count();
            int current = 0;
            int check = 1;
            while (check != 0 && !Stop)
            {
                List<HostInformation> z = Hosts.Where(w => w.Pending).Take(Config.MaxParallelConnections).ToList();
                z.ForEach(e => e.Status = HostStatus.Checking);
                List<Task<HostInformation>> tasks = new List<Task<HostInformation>>();
                foreach (var t in z)
                    tasks.Add(CheckHost(t));

                Task.WaitAll(tasks.ToArray());
                OnPropertyChanged("HostsOnline");

                check = z.Count();
                current += check;
                ScanProgress = ((double)current / (double)hostcount) * 100;

                if (Config.AutoScroll && check > 0 && z.FirstOrDefault() != null)
                    ChangeClassCNetwork(z.First().IP.ToIP().ToLongNetwork().ToIP());

            }

            if (Config.CheckInternet && CurrentNIC != null)
            {
                await CheckConnectivity();
            }

            foreach (var h in Hosts)
                h.Pending = false;
            ScanProgress = Stop ? 0 : 100;
            Stop = false;
            IsAnalyzing = false;
            FireAllPropertiesChanged();
        }

        public async Task<bool> CheckConnectivity()
        {
            try
            {
                Ping p = new Ping();
                PingReply pr = await p.SendPingAsync("8.8.8.8", Config.PingTimeout);
                GoogleDNSStatus = pr.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                GoogleDNSStatus = HostStatus.Offline;
            }

            try
            {
                Ping p2 = new Ping();
                PingReply pr2 = await p2.SendPingAsync(CurrentNIC.Gateway, Config.PingTimeout);
                GatewayStatus = pr2.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                GatewayStatus = HostStatus.Offline;
            }

            try
            {
                Ping p3 = new Ping();
                PingReply pr3 = await p3.SendPingAsync(CurrentNIC.DNS, Config.PingTimeout);
                DNSStatus = pr3.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                DNSStatus = HostStatus.Offline;
            }
            try
            {
                Ping p4 = new Ping();
                PingReply pr4 = await p4.SendPingAsync("google.de", Config.PingTimeout);
                InternetStatus = pr4.Status == IPStatus.Success && DNSStatus == HostStatus.Online ? HostStatus.Online : HostStatus.Offline;
            }
            catch
            {
                InternetStatus = HostStatus.Offline;
            }
            return true;
        }

        public async Task<HostInformation> CheckHost(HostInformation hi)
        {
            Ping p = new Ping();
            IPAddress target = hi.IP.ToIP();
            PingReply pr = await p.SendPingAsync(target, Config.PingTimeout);
            hi.Status = pr.Status == IPStatus.Success ? HostStatus.Online : HostStatus.Offline;
            hi.Ping = (int)pr.RoundtripTime;

            if (hi.Status == HostStatus.Online)
            {
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(target);
                    hi.Hostname = hostEntry.HostName;
                }
                catch
                {

                }

                if (Config.PortScan)
                {

                }
            }


            hi.Pending = false;

            /*await Task.Delay(3000);
            hi.Status = HostStatus.Offline;
            hi.Pending = false;*/
            return hi;
        }



        public void AbortScan()
        {
            Stop = true;
        }
    }
}
