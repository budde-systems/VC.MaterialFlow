namespace BlueApps.MaterialFlow.Common.Models.EventArgs;

public class BarcodeScanEventArgs : System.EventArgs, IDataEventArgs
{
    public List<string>? Barcodes { get; set; }
    
    public string? Position { get; set; }
    
    public DateTime AtTime { get; set; } = DateTime.Now;
    
    public string? Message { get; set; }
    
    public int PacketTracing { get; set; }
}