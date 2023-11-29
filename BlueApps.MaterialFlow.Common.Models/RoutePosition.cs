namespace BlueApps.MaterialFlow.Common.Models;

public class RoutePosition
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public Destination Destination { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="destination"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SetRoutePosition(Destination destination)
    {
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        Name = destination.Name;
    }
}