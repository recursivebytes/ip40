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

    public enum HostStatus : int
    {
        Unknown = 6,
        Pending = 3,
        Online = 1, 
        Offline = 4,
        Checking = 2,
        Disabled = 5
    }

}
