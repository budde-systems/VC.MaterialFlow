using BlueApps.MaterialFlow.Common.Connection.Packets;

namespace BlueApps.MaterialFlow.Common.Connection.PackteHelper
{
    public abstract class MessagePacketHelper
    {
        /// <summary>
        /// The mqtt topic for incomming messages
        /// </summary>
        public abstract string InTopic { get; set; }
        /// <summary>
        /// The mqtt topic for outgoing messages
        /// </summary>
        public abstract string OutTopic { get; set; }

        public abstract void SetPacketData(MessagePacket message);
        public abstract MessagePacket GetPacketData();
    }
}
