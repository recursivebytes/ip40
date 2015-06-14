using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    public static class Extensions
    {
        public static long ToLong(this IPAddress addr)
        {
            byte[] b = addr.GetAddressBytes();
            return b[0] * 16777216L + b[1] * 65536L + b[2] * 256L + b[3];
        }

        public static long ToLongNetwork(this IPAddress addr)
        {
            byte[] b = addr.GetAddressBytes();
            return b[0] * 16777216L + b[1] * 65536L + b[2] * 256L;
        }

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

        public static string ToSpeed(this long num)
        {
            string[] unit = new string[] { "B/s", "KB/s","MB/s","GB/s","TB/s" };

            long current = num;
            int i=0;
            for (; i < unit.Length && current >= 1000; i++)
                current /= 1000;
            return string.Format("{0} {1}", current, unit[i]);
        }

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
