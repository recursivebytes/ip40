using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    /// <summary>
    /// Wrapper class for the NetworkInterface. Provides some more features and extensions
    /// </summary>
    public class NIC : Base
    {
        /// <summary>
        /// Creates a new NIC and extracts Data from the Interface
        /// </summary>
        /// <param name="nic"></param>
        public NIC(NetworkInterface nic)
        {
            NetworkInterface = nic;          
            MAC = NetworkInterface.GetPhysicalAddress();
            Speed = NetworkInterface.Speed;

            if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in NetworkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        SubnetMask = ip.IPv4Mask.ToString();

                        var gateway = NetworkInterface.GetIPProperties().GatewayAddresses.FirstOrDefault();
                        var dns = NetworkInterface.GetIPProperties().DnsAddresses.FirstOrDefault();
                        UsesDHCP = NetworkInterface.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
                        if (gateway != null)
                            Gateway = gateway.Address.ToString();
                        if (dns != null)
                            DNS = dns.ToString();

                        IP = ip.Address.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Speed of the NIC (in B/s)
        /// </summary>
        public long Speed
        {
            get { return Get<long>("Speed"); }
            set { Set("Speed", value); OnPropertyChanged("Speed"); }
        }

        /// <summary>
        /// true, if DHCP is used (dynamic IP)
        /// </summary>
        public bool UsesDHCP
        {
            get { return Get<bool>("UsesDHCP"); }
            set { Set("UsesDHCP", value); OnPropertyChanged("UsesDHCP"); }
        }

        /// <summary>
        /// Original NetworkInterface object
        /// </summary>
        public NetworkInterface NetworkInterface
        {
            get { return Get<NetworkInterface>("NetworkInterface"); }
            set { Set("NetworkInterface", value); OnPropertyChanged("NetworkInterface"); }
        }

        /// <summary>
        /// IP of the Interface
        /// </summary>
        public string IP
        {
            get { return Get<string>("IP"); }
            set { Set("IP", value); OnPropertyChanged("IP"); }
        }

        /// <summary>
        /// Default-Gateway
        /// </summary>
        public string Gateway
        {
            get { return Get<string>("Gateway"); }
            set { Set("Gateway", value); OnPropertyChanged("Gateway"); }
        }

        /// <summary>
        /// Default-DNS
        /// </summary>
        public string DNS
        {
            get { return Get<string>("DNS"); }
            set { Set("DNS", value); OnPropertyChanged("DNS"); }
        }

        /// <summary>
        /// Subnetmask of the Interface
        /// </summary>
        public string SubnetMask
        {
            get { return Get<string>("SubnetMask"); }
            set { Set("SubnetMask", value); OnPropertyChanged("SubnetMask"); }
        }

        /// <summary>
        /// MAC Address of the Interface
        /// </summary>
        public PhysicalAddress MAC
        {
            get { return Get<PhysicalAddress>("MAC"); }
            set { Set("MAC", value); OnPropertyChanged("MAC"); }
        }

        /// <summary>
        /// Name of the Interface
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return NetworkInterface == null ? "" : NetworkInterface.Name;
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
