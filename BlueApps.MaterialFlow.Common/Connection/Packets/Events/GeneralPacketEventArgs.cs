using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Connection.Packets.Events
{
    public class GeneralPacketEventArgs : EventArgs
    {
        public GeneralPacket? GeneralPacket { get; set; }
    }
}
