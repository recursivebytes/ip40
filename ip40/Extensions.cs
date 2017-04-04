using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ip40
{
    /// <summary>
    /// Extensionmethods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts an IP Address to a long 
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static long ToLong(this IPAddress addr)
        {
            if (addr == null)
                return 0;
            byte[] b = addr.GetAddressBytes();
            return b[0] * 16777216L + b[1] * 65536L + b[2] * 256L + b[3];
        }

        /// <summary>
        /// Converts an IP Address to a long, ignores the last octett. Is the equivalent of the Network-Address in a Class C Network
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static long ToLongNetwork(this IPAddress addr)
        {
            if (addr == null)
                return 0;

            byte[] b = addr.GetAddressBytes();
            return b[0] * 16777216L + b[1] * 65536L + b[2] * 256L;
        }

        /// <summary>
        /// Converts a long  back to an IP Address
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static IPAddress ToIP(this long addr)
        {
            byte[] b = new byte[4];
            b[0] = (byte)(addr / 16777216);
            addr = addr % 16777216;
            b[1] = (byte)(addr / 65536);
            addr = addr % 65536;
            b[2] = (byte)(addr / 256);
            addr = addr % 256;
            b[3] = (byte)addr;

            return new IPAddress(b);
        }

        /// <summary>
        /// Converts a number (given in Bytes) to a speed string
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToSpeed(this long num)
        {
            string[] unit = new string[] { "B/s", "KB/s","MB/s","GB/s","TB/s" };

            long current = num;
            int i=0;
            for (; i < unit.Length && current >= 1000; i++)
                current /= 1000;
            return string.Format("{0} {1}", current, unit[i]);
        }

        /// <summary>
        /// Formats a MAC Address
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static string ToFormattedMAC(this PhysicalAddress addr)
        {
            if (addr == null)
                return "";

            string mac = addr.ToString();
            for (int i = mac.Length - 2; i > 0; i -= 2)
            {
                mac = mac.Insert(i, "-");
            }
            return mac;
        }


    }
}
