namespace BlueApps.MaterialFlow.Common.Values.Types;

public enum GeneralPacketContext : byte
{
    NoRead = 0,
    EmergencyStop = 1,
    DiverterReferencing = 2,
    Acknowledge = 3,
    Message = 4,
    DockedElement = 5
}