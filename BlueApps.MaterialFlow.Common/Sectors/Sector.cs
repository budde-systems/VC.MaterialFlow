using BlueApps.MaterialFlow.Common.Connection.Client;
using BlueApps.MaterialFlow.Common.Machines;
using BlueApps.MaterialFlow.Common.Machines.BaseMachines;
using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Models.EventArgs;
using Microsoft.Extensions.Logging;

namespace BlueApps.MaterialFlow.Common.Sectors;

public abstract class Sector
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; init; }

    public string BasePosition { get; set; }

    public Scanner BarcodeScanner { get; set; }

    public List<Scanner> BarcodeScanners { get; set; } //TODO: Diese prop verwenden, statt einen Scanner!

    public ICollection<IDiverter> Diverters { get; set; }

    public List<TrackedPacket> TrackedPackets { get; } = new();

    public List<short> RelatedErrorcodes { get; set; } = new();

    /// <summary>
    /// Sector logic is active
    /// </summary>
    public bool IsActive { get; set; }

    public event EventHandler<TrackedPacket>? NewPackageInSector;

    protected MqttClient _client;
    protected ILogger<Sector> _logger;

    protected Sector(MqttClient client, ILogger<Sector> logger, string name, string basePosition)
    {
        Name = name;
        BasePosition = basePosition;
        _client = client;
        _logger = logger;
    }

    protected void AddTrackedPacket(int tracedPacketId, int shipmentId, string? destinationName = null)
    {
        var tracking = new TrackedPacket(tracedPacketId);

        if (!string.IsNullOrEmpty(destinationName))
            tracking.DestinationName = destinationName;

        if (shipmentId > 0)
            tracking.ShipmentId = shipmentId;

        tracking.SectorId = Id;
        tracking.SectorName = Name;

        TrackedPackets.Add(tracking);

        NewPackageInSector?.Invoke(this, tracking);
    }

    public bool RemoveTrackedPacket(int trackedPacketId = 0, int shipmentId = 0)
    {
        if (trackedPacketId > 0 && shipmentId == 0)
            return TrackedPackets.RemoveAll(p => p.TracedPacketId == trackedPacketId) > 0;

        if (trackedPacketId == 0 && shipmentId > 0)
            return TrackedPackets.RemoveAll(p => p.ShipmentId == shipmentId) > 0;

        if (trackedPacketId > 0 && shipmentId > 0)
            return TrackedPackets.RemoveAll(p => p.TracedPacketId == trackedPacketId && p.ShipmentId == shipmentId) > 0;

        return false;
    }            

    public bool TrackedPacketExists(int tracedPacketId = 0, int shipmentId = 0)
    {
        var exist = false;

        if (tracedPacketId > 0)
            exist = TrackedPackets.Any(p => p.TracedPacketId == tracedPacketId);

        if (shipmentId > 0)
            exist = TrackedPackets.Any(p => p.ShipmentId == shipmentId);

        return exist;
    }

    protected string? GetDestinationOfTrackedPacket(int packetTracing) => TrackedPackets.FirstOrDefault(p => p.TracedPacketId == packetTracing)?.DestinationName;

    protected bool ErrorInThisSector(short errorCode) => RelatedErrorcodes?.Any(s => s == errorCode) ?? false;

    public abstract Scanner CreateScanner();

    public abstract ICollection<IDiverter> CreateDiverters();

    public abstract void AddRelatedErrorCodes();

    public abstract void Barcode_Scanned(object? sender, BarcodeScanEventArgs scan);

    public virtual void Weight_Scanned(object? sender, WeightScanEventArgs scan) { }

    public abstract void UnsubscribedPacket(object? sender, UnsubscribedPacketEventArgs unsubscribedPacket);

    protected abstract void ErrorHandling(short errorCode);
        
    public virtual void ErrorTriggered(object? sender, ErrorcodeEventArgs error)
    {
        if (error.Errorcodes != null)
        {
            foreach (var code in error.Errorcodes)
            {
                if (ErrorInThisSector(code))
                    ErrorHandling(code);
            }
        }
    }

    public override string ToString() => $"{Name}|{BasePosition}";
}