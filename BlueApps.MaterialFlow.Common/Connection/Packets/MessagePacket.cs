namespace BlueApps.MaterialFlow.Common.Connection.Packets;

public class MessagePacket
{
    public string Id { get; set; }
    public string Data { get; set; }
    public string Topic { get; set; }

    public override string ToString() => $"{Id}:{Data}:{Topic}";
}