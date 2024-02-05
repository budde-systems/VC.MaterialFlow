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
    public List<Toward> Towards { get; set; }
    public Scanner RelatedScanner { get; set; }

    public virtual void SetDirection(Direction driveDirection)
    {
        DriveDirection = driveDirection;
    }

    /// <summary>
    /// The fault direction is marked in the Towards. Should an error occur <br/>
    /// (e.g. Towards is null), is the default drivedirection = StraightAhead.
    /// </summary>
    public virtual void SetFaultDirection()
    {
        if (Towards is null)
            DriveDirection = Direction.StraightAhead;

        DriveDirection = Towards?.FirstOrDefault(_ => _.FaultDirection)?.DriveDirection ?? Direction.StraightAhead;
    }

    public virtual void CreateTowards(params Toward[] towards)
    {
        if (towards is null || towards.Length == 0)
            Towards = new List<Toward>();
        else
        {
            if (!towards.Any(x => x.FaultDirection))
            {
                //TODO: exception
            }
            else if (towards.Where(x => x.FaultDirection).Count() > 1)
            {
                //TODO: exception
            }
            else
            {
                Towards = towards.ToList();
            }
        }
    }

    public virtual void SetRelatedScanner(Scanner relatedScanner)
    {
        if (relatedScanner is null)
            throw new ArgumentNullException(nameof(relatedScanner));

        RelatedScanner = relatedScanner;
    }

    public bool DivertersScanner(string scannersBasePosition) =>
        RelatedScanner.BasePosition == scannersBasePosition;

    public override string ToString() => $"{BasePosition}:{SubPosition} [{Name}]";
}