using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Values.Types;
using System.Text.Json;

namespace BlueApps.MaterialFlow.Common.Connection.PackteHelper
{
    public class GeneralMessagePacketHelper : MessagePacketHelper
    {
        public override string InTopic { get; set; }
        public override string OutTopic { get; set; }

        public GeneralPacket? GeneralPacket { get; set; }

        public GeneralMessagePacketHelper(string inTopic, string outTopic)
        {
            InTopic = inTopic;
            OutTopic = outTopic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override MessagePacket GetPacketData()
        {
            if (GeneralPacket is null)
                throw new ArgumentNullException(nameof(GeneralPacket));

            MessagePacket packet = new MessagePacket()
            {
                Topic = OutTopic,
                Data = JsonSerializer.Serialize(GeneralPacket),
            };

            return packet;
        }

        public override void SetPacketData(MessagePacket message)
        {
            if (message != null && !string.IsNullOrEmpty(message.Data))
            {
                GeneralPacket = JsonSerializer.Deserialize<GeneralPacket>(message.Data);
            }
        }

        public void ClearGeneralPacketContext()
        {
            GeneralPacket?.PacketContextes?.Clear();
            GeneralPacket?.NoReads?.Clear();
        }            

        public void CreateNoReadContext(params NoRead[] noreads)
        {
            IntiGeneralPacket();

            if (GeneralPacket?.NoReads is null)
                GeneralPacket.NoReads = new();

            GeneralPacket.NoReads.AddRange(noreads);

            if (!GeneralPacket.PacketContextes.Any(_ => _ == GeneralPacketContext.NoRead))
                GeneralPacket.PacketContextes.Add(GeneralPacketContext.NoRead);
        }

        private void IntiGeneralPacket()
        {
            if (GeneralPacket is null)
                GeneralPacket = new();

            if (GeneralPacket.PacketContextes is null)
                GeneralPacket.PacketContextes = new();

            GeneralPacket.KeyCode = ActionKey.NewEntity;
        }
    }
}
