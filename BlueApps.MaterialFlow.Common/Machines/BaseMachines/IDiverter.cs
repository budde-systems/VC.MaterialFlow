using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Models.Machines;
using BlueApps.MaterialFlow.Common.Models.Types;

namespace BlueApps.MaterialFlow.Common.Machines.BaseMachines;

public interface IDiverter : IMachine
{
    public Scanner RelatedScanner { get; set; }
    
    public Direction DriveDirection { get; set; }
    
    public List<Toward> Towards { get; }
    
    void SetDirection(Direction driveDirection);
    
    Direction GetFaultDirection();
    
    void SetFaultDirection();
    
    /// <summary>
    /// It is necessary to specify at least one and only one fault-direction at the towards (Toward.FaultDirection = true)
    /// </summary>
    /// <param name="towards"></param>
    void CreateTowards(params Toward[] towards);
    
    void SetRelatedScanner(Scanner relatedScanner);
    
    bool DivertersScanner(string scannersBasePosition);
}