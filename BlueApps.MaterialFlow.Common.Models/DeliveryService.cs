namespace BlueApps.MaterialFlow.Common.Models;

/// <summary>
/// Services like express, classic etc.
/// </summary>
public class DeliveryService
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }
}