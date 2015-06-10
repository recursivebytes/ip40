using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    public class NIC : Base
    {

        public NIC(NetworkInterface nic)
        {
            NetworkInterface = nic;
            string mac = NetworkInterface.GetPhysicalAddress().ToString();
            for (int i = mac.Length - 2; i > 0; i -= 2)
            {
                mac = mac.Insert(i, "-");
            }
            MAC = mac;
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
                        Console.WriteLine(ip.Address.ToString());
                    }
                }
            }
        }

        public long Speed
        {
            get { return Get<long>("Speed"); }
            set { Set("Speed", value); OnPropertyChanged("Speed"); }
        }


        public bool UsesDHCP
        {
            get { return Get<bool>("UsesDHCP"); }
            set { Set("UsesDHCP", value); OnPropertyChanged("UsesDHCP"); }
        }


        public NetworkInterface NetworkInterface
        {
            get { return Get<NetworkInterface>("NetworkInterface"); }
            set { Set("NetworkInterface", value); OnPropertyChanged("NetworkInterface"); }
        }

        public string IP
        {
            get { return Get<string>("IP"); }
            set { Set("IP", value); OnPropertyChanged("IP"); }
        }

        public string Gateway
        {
            get { return Get<string>("Gateway"); }
            set { Set("Gateway", value); OnPropertyChanged("Gateway"); }
        }

        public string DNS
        {
            get { return Get<string>("DNS"); }
            set { Set("DNS", value); OnPropertyChanged("DNS"); }
        }


        public string SubnetMask
        {
            get { return Get<string>("SubnetMask"); }
            set { Set("SubnetMask", value); OnPropertyChanged("SubnetMask"); }
        }

        public string MAC
        {
            get { return Get<string>("MAC"); }
            set { Set("MAC", value); OnPropertyChanged("MAC"); }
        }

        public override string ToString()
        {
            return NetworkInterface == null ? "" : NetworkInterface.Name;
        }


        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
