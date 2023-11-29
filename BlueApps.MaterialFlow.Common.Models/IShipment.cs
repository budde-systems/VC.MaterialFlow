namespace BlueApps.MaterialFlow.Common.Models;

public interface IShipment
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string TransportationReference { get; set; }
    public string TrackingCode { get; set; }
    public string Carrier { get; set; }
    public string Country { get; set; }
    //public Dimensions ShipmentDimensions { get; set; }
}