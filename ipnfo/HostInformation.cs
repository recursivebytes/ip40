using csutils.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace ipnfo
{
    /// <summary>
    /// Class the represents a Host
    /// </summary>
    public class HostInformation : Base
    {
        /// <summary>
        /// For serialisation
        /// </summary>
        public HostInformation()
        {
            OpenPorts = new List<PortInformation>();
        }

        /// <summary>
        /// Creates new Host
        /// </summary>
        /// <param name="ip">IP Address (as a long). Use extensionmethod IPAddress.ToLong() to calculate this value </param>
        public HostInformation(long ip)
        {
            OpenPorts = new List<PortInformation>();
            IP = ip;
        }

        /// <summary>
        /// Creates a new host by a given IP
        /// </summary>
        /// <param name="ip"></param>
        public HostInformation(string ip)
        {
            OpenPorts = new List<PortInformation>();
            IP = IPAddress.Parse(ip).ToLong();
        }

        /// <summary>
        /// The IP of the Host
        /// </summary>
        public long IP
        {
            get { return Get<long>("IP"); }
            set { Set("IP", value); OnPropertyChanged("IP"); }
        }

        /// <summary>
        /// The MAC Address of the Host
        /// </summary>
        public byte[] MACBytes
        {
            get { return MAC.GetAddressBytes(); }
            set { Set("MAC", new PhysicalAddress(value)); OnPropertyChanged("MAC"); }
        }

        /// <summary>
        /// Fancy wrapper for the MAC
        /// </summary>
        [XmlIgnore]
        public PhysicalAddress MAC
        {
            get { return Get<PhysicalAddress>("MAC"); }
            set { Set("MAC", value); OnPropertyChanged("MAC"); OnPropertyChanged("CanWOL"); }
        }

        /// <summary>
        /// Status of the Host. It represents the actual status of the Host. For Status including Pending-remark, use HostInformation.VisibleStatus
        /// </summary>
        [XmlIgnore]
        public HostStatus Status
        {
            get { return Get<HostStatus>("Status"); }
            set { Set("Status", value); OnPropertyChanged("Status"); OnPropertyChanged("VisibleStatus"); OnPropertyChanged("CanWOL"); }
        }

        /// <summary>
        /// Flag to determine if the Host is remarked for a scan, but not scanned yet
        /// </summary>
        [XmlIgnore]
        public bool Pending
        {
            get { return Get<bool>("Pending"); }
            set { Set("Pending", value); OnPropertyChanged("Pending"); OnPropertyChanged("VisibleStatus"); }
        }

        /// <summary>
        /// Roundtrip time of the Host (Ping) in ms
        /// </summary>
        [XmlIgnore]
        public int Ping
        {
            get { return Get<int>("Ping"); }
            set { Set("Ping", value); OnPropertyChanged("Ping"); }
        }

        /// <summary>
        /// Name of the Host
        /// </summary>
        public string Hostname
        {
            get { return Get<string>("Hostname"); }
            set { Set("Hostname", value); OnPropertyChanged("Hostname"); }
        }

        /// <summary>
        /// Value that determines if a WOL message can be sent to the host
        /// </summary>
        public bool CanWOL
        {
            get
            {
                return MAC != null && (Status == HostStatus.Offline ||  Status == HostStatus.Unknown);
            }
        }

        /// <summary>
        /// Status for the View. Includes the HostStatus.Pending value based on HostInformation.Pending
        /// </summary>
        public HostStatus VisibleStatus
        {
            get
            {
                return Pending ? HostStatus.Pending : Status;
            }
        }

        /// <summary>
        /// Textual representation of this Host
        /// </summary>
        public string Text
        {
            get
            {
                return IP.ToIP().ToString();
            }
        }

        /// <summary>
        /// Textual representation of the last octett
        /// </summary>
        public string LastOctett
        {
            get
            {
                return string.Format("{0}", (int)IP.ToIP().GetAddressBytes()[3]);
            }
        }

        /// <summary>
        /// returns Text
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Text/*+", "+string.Join("|",OpenPorts.Select(s=>s.ShortName))*/;
        }

        /// <summary>
        /// List of open ports (result from portscan)
        /// </summary>
        [XmlIgnore]
        public List<PortInformation> OpenPorts
        {
            get { return Get<List<PortInformation>>("OpenPorts"); }
            set { Set("OpenPorts", value); OnPropertyChanged("OpenPorts"); }
        }

        private ICommand cmdWakeOnLan;
        /// <summary>
        /// Command that sends the magic packet
        /// </summary>
        public ICommand WakeOnLanCommand
        {
            get
            {
                if (cmdWakeOnLan == null)
                    cmdWakeOnLan = new RelayCommand(p => OnWakeOnLan(p), p => CanWakeOnLan());
                return cmdWakeOnLan;
            }
        }

        private bool CanWakeOnLan()
        {
            return CanWOL;
        }

        private void OnWakeOnLan(object parameter)
        {
            WakeOnLan(MAC.GetAddressBytes());
            Task.Run(() => { CheckStatus(1000,10); });
        }

        private ICommand cmdSMB;
        /// <summary>
        /// Command that opens the IP in Windows Explorer to view File shares
        /// </summary>
        public ICommand SMBCommand
        {
            get
            {
                if (cmdSMB == null)
                    cmdSMB = new RelayCommand(p => OnSMB(p), p => CanSMB());
                return cmdSMB;
            }
        }

        private bool CanSMB()
        {
            return true;
        }

        private void OnSMB(object parameter)
        {
            Process.Start("explorer.exe", @"\\"+IP.ToIP().ToString());
        }


        private ICommand cmdScanIP;
        /// <summary>
        /// Scans this Host
        /// </summary>
        public ICommand ScanIPCommand
        {
            get
            {
                if (cmdScanIP == null)
                    cmdScanIP = new RelayCommand(p => OnScanIP(p), p => CanScanIP());
                return cmdScanIP;
            }
        }

        private bool CanScanIP()
        {
            return Status != HostStatus.Checking;
        }

        private void OnScanIP(object parameter)
        {            
            Task.Run(() => { CheckStatus(0,1); });
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
        /// Scans this Host. 
        /// Use delay and rounds for a repeated, polling-like check (i.e. to get notified when the WOL was successful).
        /// Uses 1s as Ping Timeout.
        /// </summary>
        /// <param name="delay">delay to start the scan in ms</param>
        /// <param name="rounds">number of times the whole scan is repeated (including delay)</param>
        private async void CheckStatus(int delay, int rounds)
        {
            Status = HostStatus.Checking;

            //n rounds
            for(int i=0; i<rounds; i++)
            {
                //wait delay
                if(delay>0)
                    await Task.Delay(delay);

                //send ping
                HostInformation hi = await MainViewModel.CheckHost(this, null, 1000, false, HostStatus.Checking);

                //if successful, more rounds aren't needed
                if(hi.Status == HostStatus.Online)
                {
                    Status = HostStatus.Online;
                    return;
                }
            }

            //no answer
            Status = HostStatus.Offline;
        }

        /// <summary>
        /// Sends a Wake-On-Lan packet to the specified MAC address.
        /// </summary>
        /// <param name="mac">Physical MAC address to send WOL packet to.</param>
        private static void WakeOnLan(byte[] mac)
        {
            // WOL packet is sent over UDP 255.255.255.0:40000.
            UdpClient client = new UdpClient();
            client.Connect(IPAddress.Broadcast, 40000);

            // WOL packet contains a 6-bytes trailer and 16 times a 6-bytes sequence containing the MAC address.
            byte[] packet = new byte[17 * 6];

            // Trailer of 6 times 0xFF.
            for (int i = 0; i < 6; i++)
                packet[i] = 0xFF;

            // Body of magic packet contains 16 times the MAC address.
            for (int i = 1; i <= 16; i++)
                for (int j = 0; j < 6; j++)
                    packet[i * 6 + j] = mac[j];

            // Send WOL packet.
            client.Send(packet, packet.Length);
        }
    }
}
