namespace BlueApps.MaterialFlow.Common.Connection.Packets.Events;

public class MessagePacketEventArgs : EventArgs
{
    /// <summary>
    /// Received at
    /// </summary>
    public DateTime AtDate { get; set; } = DateTime.Now;
    public string? ClientId { get; set; }
    public MessagePacket? Message { get; set; }
}