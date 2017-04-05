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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ip40
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
