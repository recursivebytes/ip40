using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    /// <summary>
    /// Status of a Host
    /// </summary>
    public enum HostStatus : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 6,
        /// <summary>
        /// Pending, used for not yet processed but remarked IPs. Mainly used for a big range scan
        /// </summary>
        Pending = 3,
        /// <summary>
        /// Online
        /// </summary>
        Online = 1,
        /// <summary>
        /// Not available (offline)
        /// </summary>
        Offline = 4,
        /// <summary>
        /// Host is currently checked
        /// </summary>
        Checking = 2,
        /// <summary>
        /// Disabled. Mainly used for unscannable IPs
        /// </summary>
        Disabled = 5
    }
}
