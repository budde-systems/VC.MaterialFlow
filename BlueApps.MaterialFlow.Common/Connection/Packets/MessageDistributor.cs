using BlueApps.MaterialFlow.Common.Connection.Packets.Events;
using BlueApps.MaterialFlow.Common.Connection.PackteHelper;
using BlueApps.MaterialFlow.Common.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Connection.Packets
{
    public abstract class MessageDistributor
    {
        public abstract event EventHandler<BarcodeScanEventArgs> BarcodeScanned;
        public abstract event EventHandler<WeightScanEventArgs> WeigtScanned;
        public abstract event EventHandler<UnsubscribedPacketEventArgs> UnsubscribedPacket;
        public abstract event EventHandler<ErrorcodeEventArgs> ErrorcodeTriggered;

        protected readonly List<MessagePacketHelper> _packetHelpers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packetHelpers"></param>
        /// <exception cref="ArgumentException"></exception>
        public MessageDistributor(List<MessagePacketHelper> packetHelpers)
        {
            _packetHelpers = packetHelpers;

            if (packetHelpers == null || !packetHelpers.Any())
            {
                throw new ArgumentException("PacketHelpers cannot be empty or null!");
            }
        }

        public abstract void DistributeIncommingMessages(object sender, MessagePacketEventArgs messageEvent);
    }
}
