using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
using System;
using System.Runtime.CompilerServices;

public class Ship
{
    private static int Next = 0;
    public int Id { get; }
    public string ShipName { get; private set; }
    public ShipPlaces PlaceDestination { get; }
    public DateTime ArrivalTime { get; set; }
    internal bool Repeat { get; set; }
    public int AmountContainers { get; private set; }

    internal Queue<Container> containers { get; private set; }
    internal List<HistoryService> histories { get; private set; }


    /// <summary>
    /// initlisere en ny instans av <see cref="Ship"/>- classe for � holde styr p� skipets informasjon.
    /// </summary>
    /// <param name="id"> id for ship</param>
    /// <param name="shipname"> navnet på shipet</param>
    /// <param name="placedestination">destiniasjonen til shipet</param>
    /// <param name="arrivalTime">ankomst tid for shipet</param>
    /// <param name="repeat"> verdi som vi setter inn om turen skal gjenta seg</param>
    public Ship(string shipname, ShipPlaces placedestination, DateTime arrivalTime, bool repeat, int ammountOfContainers)
    {
        Id = Interlocked.Increment(ref Next);
        ShipName = shipname;
        PlaceDestination = placedestination;
        ArrivalTime = arrivalTime;
        Repeat = repeat;
        containers = new Queue<Container>();
        histories = new List<HistoryService>();
        AmountContainers = ammountOfContainers;

        
    }

    /// <summary>
    /// Legger til en container til skipet
    /// </summary>
    /// <param name="container"></param>
    internal void MakeContainers ()
    {
        containers.Clear();
        for (int i = 0; i < AmountContainers; i++)
        {
            containers.Enqueue(new Container());
        }

    }

    /// <summary>
    /// Fjerner container objekt fra kø
    /// </summary>
    /// <returns container etter å ha blitt fjernet></returns>
    internal Container MoveContainer()
    {
        return containers.Dequeue();
    }
    
    /// <summary>
    /// legger til en historie til skipet
    /// </summary>
    /// <param name="history"></param>
    internal void AddHistory(HistoryService history)
    {
        histories.Add(history);
        
    }

    /// <summary>
    /// Fjerner en historie fra skipet
    /// </summary>
    /// <param name="history"></param>
    internal void RemoveHistory(HistoryService history)
    {
        histories.Remove(history);
    }
}