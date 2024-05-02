namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using System;
using System.Collections.ObjectModel;

public class Ship
{
    private static int Next = 0;
    /// <summary>
    /// ID til et skip. ID er autogenerert
    /// </summary>
    public int Id { get; private set;}
    /// <summary>
    /// Navnet til skipet
    /// </summary>
    public string ShipName { get; private set; }
    /// <summary>
    /// Destinasjonen til skipet. Dette er et ShipPlace objekt
    /// </summary>
    public ShipPlaces PlaceDestination { get; private set; }
    /// <summary>
    /// Tiden når skipet skal ankomme destinasjonen
    /// </summary>
    public DateTime ArrivalTime { get; private set; }
    internal bool Repeat { get; set; }
    /// <summary>
    /// Nåværende status til skipet
    /// </summary>
    public Status status {get; private set; }
    internal int AmountLongContainers { get; set; }
    internal int AmountShortContainers { get; set; }
    /// <summary>
    /// Totalt antall containere i skipet
    /// </summary>
    public int TotalContainers { get; private set; }
    /// <summary>
    /// Det forteller hva slags type skip det er
    /// </summary>
    public ShipType Type { get; private set; }

    internal Queue<Container> containers { get; private set; }
    /// <summary>
    /// Liste over alle loggførte plassering.
    /// </summary>
    public Collection<HistoryService> histories { get; private set; }


    /// <summary>
    /// For laget et skip
    /// </summary>
    /// <param name="shipname">Navnet til skipet</param>
    /// <param name="placedestination"> Destinasjon til skipet. OBS du må ha en dockspace eller unloadingspace objekt før du kan lage skip objektet</param>
    /// <param name="arrivalTime">Når skipet skal nå destinasjon</param>
    /// <param name="repeat"> Om skipet skal ha repeterende seilinger eller ikke</param>
    /// <param name="amountOfLongContainers">Antall lange ISO containere</param>
    /// <param name="amountOfShortContainers">Antall korte ISO containere</param>
    /// <param name="shipType">Type for hva skal type skip det er</param>
    /// <exception cref="InvalidNameException">Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidDestinationException">hvis du referer til et destinasjon som ikke eksisterer</exception>
    /// <exception cref="InvalidAmountOfContainersException">Hvis du gir ugyldig antall korte og/eller lange ISO containere som f.eks -1</exception>
    /// <exception cref="InvalidShipTypeDestinationException">Hvis du gir skip objektet et destinasjon som ikke tillater ship-en med typen du valgte for dette skip objektet</exception>
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

        if (placedestination.Shiptype != ShipType.All && shipType != placedestination.Shiptype)
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
    internal void MakeContainers()
    {
        containers.Clear();
        for (int i = 0; i < AmountLongContainers; i++)
        {
            containers.Enqueue(new Container(ContainerType.Long));
        }
        for (int i = 0; i < AmountShortContainers; i++)
        {
            containers.Enqueue(new Container(ContainerType.Short));
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
    /// <param name="history">Historikk objektet som skal bli lagt til</param>
    internal void AddHistory(HistoryService history)
    {
        histories.Add(history);
        
    }

    /// <summary>
    /// Fjerner en historie fra skipet
    /// </summary>
    /// <param name="history">Historikk objektet som skal bli slettet</param>
    internal void RemoveHistory(HistoryService history)
    {
        histories.Remove(history);
    }
}