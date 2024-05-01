namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using System;
using System.Collections.ObjectModel;

public class Ship
{
    private static int Next = 0;
    public int Id { get; private set;}
    public string ShipName { get; private set; }
    public ShipPlaces PlaceDestination { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    internal bool Repeat { get; set; }
    public Status status {get; private set; }
    internal int AmountLongContainers { get; set; }
    internal int AmountShortContainers { get; set; }
    public int TotalContainers { get; private set; }
    public ShipType Type { get; private set; }

    internal Queue<Container> containers { get; private set; }
    public Collection<HistoryService> histories { get; private set; }


    /// <summary>
    /// Lager et skip objekt
    /// </summary>
    /// <param name="shipname"></param>
    /// <param name="placedestination"></param>
    /// <param name="arrivalTime"></param>
    /// <param name="repeat"></param>
    /// <param name="amountOfLongContainers"></param>
    /// <param name="amountOfShortContainers"></param>
    /// <param name="shipType"></param>
    /// <exception cref="InvalidNameException"></exception>
    /// <exception cref="InvalidDestinationException"></exception>
    /// <exception cref="InvalidAmountOfContainersException"></exception>
    /// <exception cref="InvalidShipTypeDestinationException"></exception>
    public Ship(string shipname, ShipPlaces placedestination, DateTime arrivalTime, bool repeat, int amountOfLongContainers, int amountOfShortContainers, ShipType shipType)
    {
        if (string.IsNullOrEmpty(shipname))
        {
            throw new InvalidNameException("ShipName cannot be empty");
        }

        if (placedestination == null)
        {
            throw new InvalidDestinationException("ShipDestination cannot be null");
        }

        if (amountOfLongContainers < 0 || amountOfShortContainers < 0)
        {
            throw new InvalidAmountOfContainersException("AmountOfContainers must be greater than or equal to 0");
        }

        if (placedestination.Shiptype != ShipType.all && shipType != placedestination.Shiptype)
        {
            throw new InvalidShipTypeDestinationException("ShipType must be the same as the destination type, or destination must allow all types");
        }

        Id = Interlocked.Increment(ref Next);
        ShipName = shipname;
        PlaceDestination = placedestination;
        ArrivalTime = arrivalTime;
        Repeat = repeat;
        containers = new Queue<Container>();
        histories = new Collection<HistoryService>();
        AmountLongContainers = amountOfLongContainers;
        AmountShortContainers = amountOfShortContainers;
        TotalContainers = amountOfLongContainers + amountOfShortContainers;
        Type = shipType;
        
    }

    /// <summary>
    /// Legger til containere i skipet basert på antall lange og korte containers
    /// </summary>
    internal void MakeContainers ()
    {
        containers.Clear();
        for (int i = 0; i < AmountLongContainers; i++)
        {
            containers.Enqueue(new Container(ContainerType.LONG));
        }
        for (int i = 0; i < AmountShortContainers; i++)
        {
            containers.Enqueue(new Container(ContainerType.SHORT));
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