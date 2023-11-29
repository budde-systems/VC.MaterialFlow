namespace BlueApps.MaterialFlow.Common.Values.Types;

public enum ActionKey : byte
{
    PLC = 0,
    RequestShipments = 1, //Shipment werden beim WebSerice angefragt
    UpdateShipments = 2, //Neue Shipment kommen an die geupdatet werden müssen
    RequestConfiguration = 3,
    UpdateConfiguration = 4,
    RequestedEntity = 5,
    NewEntity = 6,
    UpdatedEntity = 7,
}