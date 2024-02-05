using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Values.Types;

namespace BlueApps.MaterialFlow.Common.Connection.Packets;

public class GeneralPacket : ActionPacket
{
    public List<GeneralPacketContext> PacketContextes { get; set; }
    public List<NoRead>? NoReads { get; set; }
}