using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    public class HostDummy : Base
    {
        public HostDummy(int lo)
        {
            lastoctett = lo;
        }

        public HostStatus Status
        {
            get
            {
                if (lastoctett == 0 || lastoctett == 255 )
                    return HostStatus.Disabled;

                if (Host == null)
                    return HostStatus.Unknown;

                return Host.Pending ? HostStatus.Pending : Host.Status;
            }
        }

        public HostInformation Host
        {
            get { return Get<HostInformation>("Host"); }
            set { Set("Host", value); OnPropertyChanged("Host");  }
        }


       

        private int lastoctett;
        public string LastOctett
        {
            get
            {
                if (Host != null)
                {
                    
                        return Host.LastOctett;
                }
                else
                {
                    if (lastoctett == 0 || lastoctett == 255)
                        return "";
                    else
                        return lastoctett.ToString();
                }
            }
            set
            {
                lastoctett =  int.Parse(value);
            }
        }

        public int LastOctettInt
        {
            get
            {
                return lastoctett;
            }
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public enum HostStatus
    {
        Unknown,
        Pending,
        Online, 
        Offline,
        Checking,
        Disabled
    }

}
