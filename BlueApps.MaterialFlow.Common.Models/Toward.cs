using BlueApps.MaterialFlow.Common.Models.Types;

namespace BlueApps.MaterialFlow.Common.Models;

public class Toward
{
    public Direction DriveDirection { get; set; }
    
    public RoutePosition RoutePosition { get; set; } = new();
    
    public bool FaultDirection { get; set; }
}