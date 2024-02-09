using BlueApps.MaterialFlow.Common.Models.Machines;

namespace BlueApps.MaterialFlow.Common.Machines;

public class Scanner : IMachine
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; } = string.Empty;
    
    public string BasePosition { get; set; }
    
    public string SubPosition { get; set; }
    
    public string Barcode { get; set; } = string.Empty;

    public Scanner(string basePosition, string subPosition)
    {
        BasePosition = basePosition;
        SubPosition = subPosition;
    }

    public override string ToString() => $"{BasePosition} {Id}";
}