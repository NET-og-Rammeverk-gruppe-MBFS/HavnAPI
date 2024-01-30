using System;

public class Ship
{
    public int ShipId { get; set; }
    public int ContainerCapacity { get; set; }

    public Ship{}

    public Ship(int shipId, int containerCapatity)
    {
        ShipId = shipId;
        ContainerCapacity = containerCapatity;
    }
}