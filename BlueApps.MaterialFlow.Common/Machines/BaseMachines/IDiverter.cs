using BlueApps.MaterialFlow.Common.Models;
using BlueApps.MaterialFlow.Common.Models.Machines;
using BlueApps.MaterialFlow.Common.Models.Types;

namespace BlueApps.MaterialFlow.Common.Machines.BaseMachines;

public interface IDiverter : IMachine
{
    public Scanner RelatedScanner { get; set; }
    public Direction DriveDirection { get; set; }
    public List<Toward> Towards { get; set; }
    //TODO: prop ConnectedDiverters {get... oder ähnliches => Wenn das hier Diverter B von A, B und C ist, dann müssen die connectedDiverters = A und C sein.

    void SetDirection(Direction driveDirection);
    void SetFaultDirection();
    /// <summary>
    /// It is necessary to specify at least one and only one fault-direction at the towards (Toward.FaultDirection = true)
    /// </summary>
    /// <param name="towards"></param>
    void CreateTowards(params Toward[] towards);
    void SetRelatedScanner(Scanner relatedScanner);
    bool DivertersScanner(string scannersBasePosition);
}