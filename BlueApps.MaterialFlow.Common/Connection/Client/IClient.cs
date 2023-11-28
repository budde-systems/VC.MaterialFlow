using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Connection.Packets.Events;
using BlueApps.MaterialFlow.Common.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Connection.Client
{
    public interface IClient
    {
        event EventHandler<MessagePacketEventArgs> OnReceivingMessage;
        void SendData(MessagePacket messagePacket);
        void AddTopics(params string[] topics);
        //MessagePacket ReceiveData();
    }
}
