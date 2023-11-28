using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueApps.MaterialFlow.Common.Connection.PackteHelper;

namespace BlueApps.MaterialFlow.Common.Connection.Packets
{
    public class MessagePacket
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public string Topic { get; set; }

    }
}
