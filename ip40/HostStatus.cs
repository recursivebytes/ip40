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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ip40
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
