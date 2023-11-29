using System.Text.Json;
using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Values.Types;

namespace BlueApps.MaterialFlow.Common.Connection.PacketHelper;

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

        var packet = new MessagePacket
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

    public void CreateNoReadContext(params NoRead[] noReads)
    {
        InitGeneralPacket();

        if (GeneralPacket?.NoReads is null)
            GeneralPacket.NoReads = new();

        GeneralPacket.NoReads.AddRange(noReads);

        if (!GeneralPacket.PacketContextes.Any(_ => _ == GeneralPacketContext.NoRead))
            GeneralPacket.PacketContextes.Add(GeneralPacketContext.NoRead);
    }

    private void InitGeneralPacket()
    {
        GeneralPacket ??= new();
        GeneralPacket.PacketContextes ??= new();

        GeneralPacket.KeyCode = ActionKey.NewEntity;
    }
}