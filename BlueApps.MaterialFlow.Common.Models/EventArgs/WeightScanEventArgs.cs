namespace BlueApps.MaterialFlow.Common.Models.EventArgs
{
    public class WeightScanEventArgs : System.EventArgs
    {
        public double Weight { get; set; }
        public List<string>? Barcodes { get; set; }
        public string? Position { get; set; }
        public DateTime AtTime { get; set; } = DateTime.Now;
        public int PacketTracing { get; set; }
    }
}
