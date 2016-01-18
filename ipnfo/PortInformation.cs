using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    /// <summary>
    /// Class that represents a network service bound to a port or a series of ports
    /// </summary>
    public class PortInformation : Base
    {
        /// <summary>
        /// Creates new Service Definition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="ports"></param>
        public PortInformation(string name, string description, ProtocolType type, params int[] ports)
        {
            Checked = true;
            Name = name;
            Description = description;
            Type = type;
            Ports = ports;
        }

        /// <summary>
        /// Ports associated with the service
        /// </summary>
        public int[] Ports
        {
            get { return Get<int[]>("Ports"); }
            set { Set("Ports", value); OnPropertyChanged("Ports"); }
        }

        /// <summary>
        /// Name of the service
        /// </summary>
        public string Name
        {
            get { return Get<string>("Name"); }
            set { Set("Name", value); OnPropertyChanged("Name"); }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return Get<string>("Description"); }
            set { Set("Description", value); OnPropertyChanged("Description"); }
        }

        /// <summary>
        /// value that indicates if this Service should be checked in a portscan
        /// </summary>
        public bool Checked
        {
            get { return Get<bool>("Checked"); }
            set { Set("Checked", value); OnPropertyChanged("Checked"); }
        }

        /// <summary>
        /// value that indicates if there's a action for this service to call (like open the explorer or start a program)
        /// </summary>
        public bool HasService
        {
            get { return Get<bool>("HasService"); }
            set { Set("HasService", value); OnPropertyChanged("HasService"); }
        }

        /// <summary>
        /// ProtocolType. Normally always TCP
        /// </summary>
        public ProtocolType Type
        {
            get { return Get<ProtocolType>("Type"); }
            set { Set("Type", value); OnPropertyChanged("Type"); }
        }

        /// <summary>
        /// override to identify the object by it's name
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// override to compare the object by it's name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(!(obj is PortInformation))
                return false;

            return Name.Equals(((PortInformation)obj).Name);
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
