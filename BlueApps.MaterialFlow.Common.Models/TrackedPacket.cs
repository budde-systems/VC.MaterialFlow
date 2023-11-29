namespace BlueApps.MaterialFlow.Common.Models
{
    public class TrackedPacket
    {
        public int TracedPacketId { get; set; }
        public string? DestinationName { get; set; }
        public int ShipmentId { get; set; }
        public string SectorId { get; set; }
        public string SectorName { get; set; }

        public TrackedPacket(int tracedPacketId)
        {
            TracedPacketId = tracedPacketId;
        }
    }
}
