using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    public class PortInformation : Base
    {
        public PortInformation(string name, string description, ProtocolType type, params int[] ports)
        {
            Checked = true;
            Name = name;
            Description = description;
            Type = type;
            Ports = ports;
        }

        public int[] Ports
        {
            get { return Get<int[]>("Ports"); }
            set { Set("Ports", value); OnPropertyChanged("Ports"); }
        }

        public string Name
        {
            get { return Get<string>("Name"); }
            set { Set("Name", value); OnPropertyChanged("Name"); }
        }

        public string Description
        {
            get { return Get<string>("Description"); }
            set { Set("Description", value); OnPropertyChanged("Description"); }
        }

        
        public bool Checked
        {
            get { return Get<bool>("Checked"); }
            set { Set("Checked", value); OnPropertyChanged("Checked"); }
        }

        public bool HasService
        {
            get { return Get<bool>("HasService"); }
            set { Set("HasService", value); OnPropertyChanged("HasService"); }
        }


        public ProtocolType Type
        {
            get { return Get<ProtocolType>("Type"); }
            set { Set("Type", value); OnPropertyChanged("Type"); }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is PortInformation))
                return false;

            return Name.Equals(((PortInformation)obj).Name);
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
