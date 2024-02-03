using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Enums;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
using System;
using System.Runtime.CompilerServices;

public class Ship
{
    public int Id {get; private set; }
    private string ShipName { get; set; }
    private string PlaceDestination { get; set; }
    private DateTime ArrivalTime { get; set; }
    private bool Repeat { get; set; }

    private Queue<Container> containers;
    private List<HistoryService> histories;


    /// <summary>
    /// initlisere en ny instans av <see cref="Ship"/>- classe for � holde styr p� skipets informasjon.
    /// </summary>
    /// <param name="id"> id for ship</param>
    /// <param name="shipname"> navnet på shipet</param>
    /// <param name="placedestination">destiniasjonen til shipet</param>
    /// <param name="arrivalTime">ankomst tid for shipet</param>
    /// <param name="repeat"> verdi som vi setter inn om turen skal gjenta seg</param>
    public Ship(int id, string shipname, string placedestination, DateTime arrivalTime, bool repeat)
    {
        Id = id;
        ShipName = shipname;
        PlaceDestination = placedestination;
        ArrivalTime = arrivalTime;
        Repeat = repeat;
        containers = new Queue<Container>();
        histories = new List<HistoryService>();
        
    }

    /// <summary>
    /// Legger til en container til skipet
    /// </summary>
    /// <param name="container"></param>
    public void AddContainer (Container container)
    {
        containers.Enqueue(container);

    }

    public Container MoveContainer()
    {
        return containers.Dequeue();
    }
    
    /// <summary>
    /// legger til en historie til skipet
    /// </summary>
    /// <param name="history"></param>
    public void AddHistory(HistoryService history)
    {
        histories.Add(history);
        
    }

    /// <summary>
    /// Fjerner en historie fra skipet
    /// </summary>
    /// <param name="history"></param>
    public void RemoveHistory(HistoryService history)
    {
        histories.Remove(history);
    }
}