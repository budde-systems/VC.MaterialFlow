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
    public List<TrackedPacket> TrackedPackets { get; set; }
    public List<short> RelatedErrorcodes { get; set; } = new();
    /// <summary>
    /// Sector logic is active
    /// </summary>
    public bool IsActive { get; set; }

    public event EventHandler<TrackedPacket> NewPackageInSector;

    protected MqttClient _client;
    protected ILogger<Sector> _logger;

    protected Sector(MqttClient client, ILogger<Sector> logger, string name, string baseposition)
    {
        Name = name;
        BasePosition = baseposition;
        _client = client;
        _logger = logger;
    }

    protected void AddTrackedPacket(int tracedPacketId, int shipmentId, string? destinationName = null)
    {
        if (TrackedPackets is null)
            TrackedPackets = new List<TrackedPacket>();

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
            return TrackedPackets?.RemoveAll(_ => _.TracedPacketId == trackedPacketId) > 0;

        if (trackedPacketId == 0 && shipmentId > 0)
            return TrackedPackets?.RemoveAll(_ => _.ShipmentId == shipmentId) > 0;

        if (trackedPacketId > 0 && shipmentId > 0)
            return TrackedPackets?.RemoveAll(_ => _.TracedPacketId == trackedPacketId && _.ShipmentId == shipmentId) > 0;

        return false;
    }            

    public bool TrackedPacketExists(int tracedPacketId = 0, int shipmentId = 0)
    {
        var exist = false;

        if (tracedPacketId > 0)
            exist = TrackedPackets?.Any(_ => _.TracedPacketId == tracedPacketId) ?? false;

        if (shipmentId > 0)
            exist = TrackedPackets?.Any(_ => _.ShipmentId == shipmentId) ?? false;

        return exist;
    }

    protected string? GetDestinationOfTrackedPacket(int packetTracing) =>
        TrackedPackets.FirstOrDefault(_ => _.TracedPacketId == packetTracing)?.DestinationName;

    protected bool ErrorInThisSector(short errorcode) => RelatedErrorcodes?.Any(_ => _ == errorcode) ?? false;

    public abstract Scanner CreateScanner();
    public abstract ICollection<IDiverter> CreateDiverters();
    public abstract void AddRelatedErrorcodes();
    public abstract void Barcode_Scanned(object? sender, BarcodeScanEventArgs scan);
    public virtual void Weight_Scanned(object? sender, WeightScanEventArgs scan) { }
    public abstract void UnsubscripedPacket(object? sender, UnsubscribedPacketEventArgs unsubscribedPacket);
    protected abstract void ErrorHandling(short errorcode);
        
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

    public override string ToString() => $"{Name} : Base {BasePosition}";
}