using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    public class HostInformation : Base
    {
        public HostInformation(long ip)
        {
            IP = ip;
        }

        public HostInformation(string ip)
        {
            IP = IPAddress.Parse(ip).ToLong();
        }

        public long IP
        {
            get { return Get<long>("IP"); }
            set { Set("IP", value); OnPropertyChanged("IP"); }
        }


        public HostStatus Status
        {
            get { return Get<HostStatus>("Status"); }
            set { Set("Status", value); OnPropertyChanged("Status"); OnPropertyChanged("VisibleStatus"); }
        }

        public bool Pending
        {
            get { return Get<bool>("Pending"); }
            set { Set("Pending", value); OnPropertyChanged("Pending"); OnPropertyChanged("VisibleStatus"); }
        }

        public int Ping
        {
            get { return Get<int>("Ping"); }
            set { Set("Ping", value); OnPropertyChanged("Ping"); }
        }

        public string Hostname
        {
            get { return Get<string>("Hostname"); }
            set { Set("Hostname", value); OnPropertyChanged("Hostname"); }
        }


        public HostStatus VisibleStatus
        {
            get
            {
                return Pending ? HostStatus.Pending : Status;
            }
        }

        public string Text
        {
            get
            {
                return IP.ToIP().ToString();
            }
        }

        public string LastOctett
        {
            get
            {
                return string.Format(".{0}", (int)IP.ToIP().GetAddressBytes()[3]);
            }
        }

        public override string ToString()
        {
            return Text;
        }


        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
