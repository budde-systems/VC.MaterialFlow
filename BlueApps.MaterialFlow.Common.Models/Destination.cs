﻿namespace BlueApps.MaterialFlow.Common.Models;

public class Destination
{
    public int Id { get; set; }

    /// <summary>
    /// Target name
    /// </summary>
    public string? Name { get; set; }  
    
    public string? UI_Id{ get; set; }

    public List<Carrier>? Carriers { get; set; }

    public List<Country>? Countries { get; set; }

    public List<ClientReference>? ClientReferences { get; set; }   
    
    public List<DeliveryService>? DeliveryServices { get; set; }

    public bool Active { get; set; }

    /// <summary>
    /// The load factor of the destination
    /// </summary>
    public double LoadFactor { get; set; }
}