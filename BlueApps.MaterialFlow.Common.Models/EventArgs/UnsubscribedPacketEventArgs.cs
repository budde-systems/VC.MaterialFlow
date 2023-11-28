using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models.EventArgs
{
    public class UnsubscribedPacketEventArgs : System.EventArgs
    {
        public int PacketTracing { get; set; }
        public DateTime AtTime { get; set; } = DateTime.Now;
    }
}
