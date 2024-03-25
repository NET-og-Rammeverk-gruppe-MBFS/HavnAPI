namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using System;
using System.Collections.ObjectModel;

public class Ship
{
    private static int Next = 0;
    public int Id { get; }
    public string ShipName { get; private set; }
    public ShipPlaces PlaceDestination { get; }
    public DateTime ArrivalTime { get; set; }
    internal bool Repeat { get; set; }
    public Status status {get; private set; }
    public int AmountLongContainers { get; private set; }
    public int AmountShortContainers { get; private set; }
    public ShipType Type { get; private set; }

    internal Queue<Container> containers { get; private set; }
    internal Collection<HistoryService> histories { get; private set; }


    /// <summary>
    /// initlisere en ny instans av <see cref="Ship"/>- classe for � holde styr p� skipets informasjon.
    /// </summary>
    /// <param name="id"> id for ship</param>
    /// <param name="shipname"> navnet på shipet</param>
    /// <param name="placedestination">destiniasjonen til shipet</param>
    /// <param name="arrivalTime">ankomst tid for shipet</param>
    /// <param name="repeat"> verdi som vi setter inn om turen skal gjenta seg</param>
    /// <param name="amountOfLongContainers">antall lange containere</param>
    /// <param name="amountOfShortContainers">antall korte containere</param>
    /// <param name="type">type skip</param>
    /// <exception cref="InvalidNameException">Kastes hvis ShipName er tom.</exception>
    /// <exception cref="InvalidPlaceDestinationException">Kastes hvis PlaceDestination er tom.</exception>
    /// <exception cref="InvalidAmountOfContainersException">Kastes hvis AmountContainers er mindre enn 0.</exception>"
    /// <exception cref="InvalidShipTypeDestinationException">Kastes hvis ShipType er forskjellig fra PlaceDestination.Type.</exception>""
    public Ship(string shipname, ShipPlaces placedestination, DateTime arrivalTime, bool repeat, int amountOfLongContainers, int amountOfShortContainers, ShipType type)
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

        if (placedestination.Type != ShipType.all && type != placedestination.Type)
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
        Type = type;
        
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