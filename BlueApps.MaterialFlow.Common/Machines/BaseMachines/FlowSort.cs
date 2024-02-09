using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Models.Types;

namespace BlueApps.MaterialFlow.Common.Machines.BaseMachines;

public class FlowSort : IDiverter
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }
    
    public string BasePosition { get; set; }
    
    public string SubPosition { get; set; }
    
    public Direction DriveDirection { get; set; }

    public List<Toward> Towards { get; } = new();
    
    public Scanner RelatedScanner { get; set; }

    public virtual void SetDirection(Direction driveDirection)
    {
        DriveDirection = driveDirection;
    }

    public Direction GetFaultDirection() => Towards.FirstOrDefault(t => t.FaultDirection)?.DriveDirection ?? Direction.StraightAhead;

    /// <summary>
    /// The fault direction is marked in the Towards. Should an error occur <br/>
    /// (e.g. Towards is null), is the default drivedirection = StraightAhead.
    /// </summary>
    public virtual void SetFaultDirection() => DriveDirection = GetFaultDirection();

    public virtual void CreateTowards(params Toward[] towards)
    {
        var faultCount = towards.Count(x => x.FaultDirection);

        if (faultCount == 0) throw new ArgumentException("FaultDirection is required");
        if (faultCount > 1) throw new ArgumentException("Only one FaultDirection was expected");

        Towards.Clear();
        Towards.AddRange(towards);
    }

    public virtual void SetRelatedScanner(Scanner relatedScanner)
    {
        RelatedScanner = relatedScanner ?? throw new ArgumentNullException(nameof(relatedScanner));
    }

    public bool DivertersScanner(string scannersBasePosition) => RelatedScanner.BasePosition == scannersBasePosition;

    public override string ToString() => $"{BasePosition}:{SubPosition} [{Name}]";
}