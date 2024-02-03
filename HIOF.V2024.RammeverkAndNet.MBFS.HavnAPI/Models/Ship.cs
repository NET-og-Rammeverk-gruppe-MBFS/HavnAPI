using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Enums;
using System;

public class Ship
{
    public int Id { get; set; }
    public string ShipName { get; set; }
    public string PlaceDestination { get; set; }
    public DateTime ArrivalTime { get; set; }
    public bool Repeat { get; set; }

    private List<Container> containers;
    private List<History> histories;


    public Ship(int id, string shipname, string placedestination, dateTime arrivalTime, bool repeat)
    {
        Id = id;
        ShipName = shipname;
        PlaceDestination = placedestination;
        ArrivalTime = arrivalTime;
        Repeat = repeat;
        containers = new List<Container>();
        histories = new List<History>();
        
    }

    /// <summary>
    /// Legger til en container til skipet
    /// </summary>
    /// <param name="container"></param>
    public void AddContainer (Container container)
    {
        containers.Add(container);
    }
    /// <summary>
    /// legger til en historie til skipet
    /// </summary>
    /// <param name="history"></param>
    public void AddHistory(History history)
    {
        histories.Add(history);
    }

}