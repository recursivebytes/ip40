using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    /// <summary>
    /// Wrapper for a HostInformation. Each HostDummy is bound to a tile of the Class C Grid. 
    /// When changing the Class C Network the HostDummy.Host Property gets changed to the corresponding IP. 
    /// This is used to prevent the creation of thousands of dummy HostInformation objects just for being able to scroll through the networks.
    /// HostDummy = IP seen on screen, HostInformation = actual IP that is used
    /// </summary>
    public class HostDummy : Base
    {
        /// <summary>
        /// Creates a new dummy
        /// </summary>
        /// <param name="lo">Host ID (last octett of Class C network)</param>
        public HostDummy(int lo)
        {
            lastoctett = lo;
        }

        /// <summary>
        /// Status of the Host
        /// </summary>
        public HostStatus Status
        {
            get
            {                

                if (Host == null)
                    return HostStatus.Unknown;

                return Host.Pending ? HostStatus.Pending : Host.Status;
            }
        }

        /// <summary>
        /// The actual Host behind the dummy
        /// </summary>
        public HostInformation Host
        {
            get { return Get<HostInformation>("Host"); }
            set { Set("Host", value); OnPropertyChanged("Host");  }
        }


       

        private int lastoctett;
        /// <summary>
        /// String representation of the last octett
        /// </summary>
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

        /// <summary>
        /// Int representation of the last octett
        /// </summary>
        public int LastOctettInt
        {
            get
            {
                return lastoctett;
            }
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
