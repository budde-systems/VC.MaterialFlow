using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Connection.Packets.Events;

namespace BlueApps.MaterialFlow.Common.Connection.Client;

public interface IClient
{
    event EventHandler<MessagePacketEventArgs> OnReceivingMessage;
 
    void SendData(MessagePacket messagePacket);

    void AddTopics(params string[]? topics);
    
    //MessagePacket ReceiveData();
}